using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;
using System.Data.Common;
using System.Diagnostics;

namespace SqlExtensions
{
    public static class ParamMapper
    {
        private const BindingFlags PublicInstanceFlatten 
            = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        private static readonly PropertyInfo DbParameter_ParameterName 
            = typeof(DbParameter).GetProperty(nameof(DbParameter.ParameterName), PublicInstanceFlatten);

        private static readonly PropertyInfo DbParameter_Value 
            = typeof(DbParameter).GetProperty(nameof(DbParameter.Value), PublicInstanceFlatten);

        private static readonly PropertyInfo DbCommand_Parameters 
            = typeof(DbCommand).GetProperty(nameof(DbCommand.Parameters), PublicInstanceFlatten);

        private static readonly MethodInfo DbCommand_CreateParameter 
            = typeof(DbCommand).GetMethod(nameof(DbCommand.CreateParameter), PublicInstanceFlatten);

        private static readonly MethodInfo DbParameterCollection_Add
            = typeof(DbParameterCollection).GetMethod(nameof(DbParameterCollection.Add), PublicInstanceFlatten);

        private static readonly Dictionary<Type, Action<DbCommand, object>> Cache
            = new Dictionary<Type, Action<DbCommand, object>>();

        public static Action<DbCommand, object> GenerateParameterMap<TParam>(TParam sqlParameterObject)
        {
            if (sqlParameterObject == null)
            {
                throw new ArgumentNullException(nameof(sqlParameterObject));
            }

            Action<DbCommand, object> sqlQueryParameterSetter;

            if (!Cache.TryGetValue(sqlParameterObject.GetType(), out sqlQueryParameterSetter))
            {
                sqlQueryParameterSetter = GenerateParameterMapPrivate(sqlParameterObject);
                Cache[sqlParameterObject.GetType()] = sqlQueryParameterSetter;
            }

            return sqlQueryParameterSetter;
        }

        public static Action<DbCommand, object> GenerateParameterMap(object sqlParameterObject)
        {
            if (sqlParameterObject == null)
            {
                throw new ArgumentNullException(nameof(sqlParameterObject));
            }

            Action<DbCommand, object> sqlQueryParameterSetter;

            if (!Cache.TryGetValue(sqlParameterObject.GetType(), out sqlQueryParameterSetter))
            {
                sqlQueryParameterSetter = GenerateParameterMapPrivate(sqlParameterObject);
                Cache[sqlParameterObject.GetType()] = sqlQueryParameterSetter;
            }

            return sqlQueryParameterSetter;
        }

        public static Func<DbCommand, TAnonymous> GetOutputParameters<TAnonymous>()
        {
            return null;
        }

        public static TAnonymous GenerateTest<TAnonymous>(TAnonymous input)
            where TAnonymous : class
        {










            PropertyInfo[] properties = typeof(TAnonymous).GetProperties(PublicInstanceFlatten)
                .Where(p => p.CanRead)
                .ToArray();

            ParameterExpression anoObj = Expression.Parameter(typeof(TAnonymous), "anoObj");

            var constructor = Expression.New(typeof(TAnonymous).GetConstructors()[0],
                properties.Select(p => Expression.Property(anoObj, p)).ToArray());

            return Expression.Lambda<Func<TAnonymous, TAnonymous>>(constructor, anoObj)
                .Compile()(input);
        }

        private static Action<DbCommand, TAnonymous> GenerateAnonymousOutput<TAnonymous>()
        {
            //Expression.New()

            return null;
        }

        private static Action<DbCommand, object> GenerateParameterMapPrivate(object sqlParameterObject)
        {
            Type sqlParameterObjType = sqlParameterObject.GetType();

            PropertyInfo[] properties = sqlParameterObjType.GetProperties(PublicInstanceFlatten)
                .Where(p => p.CanRead)
                .ToArray();

            if (properties.Length == 0)
            {
                throw new ArgumentException("No Public Properties in Parameter Object", nameof(sqlParameterObject));
            }

            ParameterExpression dbCommandInput = Expression.Parameter(typeof(DbCommand), "dbCommandInput");
            ParameterExpression uncastParameter = Expression.Parameter(typeof(object), "sqlParametersObject");

            // TIn sqlParameters;
            // sqlParameters = (TIn)sqlParametersObject;
            var variable = Expression.Variable(sqlParameterObjType, "sqlParameters");
            var parameterExpr = Expression.Assign(
                variable,
                Expression.Convert(uncastParameter, sqlParameterObjType));

            List<Expression> expressionList = new List<Expression> { parameterExpr };
            expressionList.AddRange(properties.Select(p => SetParameter(dbCommandInput, variable, p)));
            
            // We create the a Block for sqlParameters
            BlockExpression allSetters = Expression.Block(typeof(void),
                new ParameterExpression[] { variable },
                expressionList.Cast<Expression>().ToArray());

            return Expression.Lambda<Action<DbCommand, object>>(allSetters, dbCommandInput, uncastParameter)
                .Compile();
        }

        private static Expression SetParameter(ParameterExpression dbCommandInput, ParameterExpression parameterExpr, PropertyInfo property)
        {
            // var DbParameterForFoo = cmd.CreateParameter();
            var dbParameterExp = Expression.Variable(typeof(DbParameter), string.Format("dbParameterFor{0}", property.Name));
            var dbParameterAssign = Expression.Assign(dbParameterExp, Expression.Call(dbCommandInput, DbCommand_CreateParameter));

            // DbParameterForFoo.Name = "Foo";
            var namePropertyExp = Expression.Property(dbParameterExp, DbParameter_ParameterName);
            var nameAssign = Expression.Assign(namePropertyExp, Expression.Constant(property.Name));

            // DbParameterForFoo.Value = parameters.Foo;
            var valueExpression = Expression.Property(parameterExpr, property);
            var valuePropertyExp = Expression.Property(dbParameterExp, DbParameter_Value);
            var valueAssign = Expression.Assign(valuePropertyExp, valueExpression);

            // dbCommandInput.Parameters.Add(DbParameterForFoo);
            var parametersProperty = Expression.Property(dbCommandInput, DbCommand_Parameters);
            var addParameterExpression = Expression.Call(parametersProperty, DbParameterCollection_Add, dbParameterExp);

            return Expression.Block(typeof(void), new ParameterExpression[] { dbParameterExp },
                new Expression[] { dbParameterAssign, nameAssign, valueAssign, addParameterExpression  });
        }
    }
}
