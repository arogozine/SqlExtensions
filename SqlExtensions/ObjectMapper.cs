using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;
using System.Data.Common;
using System.Data;
using System.Collections;

namespace SqlExtensions
{
    internal delegate void SetMethodDelegate<TObject>(TObject instance, object value);

    public static class ObjectMapper<TObject>
        where TObject : class, new()
    {
        /// <summary>
        /// For a specific type TObject,
        /// Mapping of [Method Name -> Setter Function]
        /// </summary>
        private readonly static IReadOnlyDictionary<string, SetMethodDelegate<TObject>> SetterCache;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static ObjectMapper() {
            var dictionary = new Dictionary<string, SetMethodDelegate<TObject>>();

            foreach (var property in GetSetMethods())
            {
                SetMethodDelegate<TObject> setter = GenerateCompiledSetter(property.SetMethod);
                dictionary[CleanString(property.Name)] = setter;
            }

            SetterCache = dictionary;
        }

        private static string CleanString(string input)
        {
            char[] arr = new char[input.Length];
            int j = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                char ic = input[i];
                if (char.IsLetterOrDigit(ic))
                {
                    ic = char.ToLowerInvariant(ic);
                    arr[j++] = ic;
                }
            }

            return new string(arr, 0, j);
        }

        private static SetMethodDelegate<TObject> GenerateCompiledSetter(MethodInfo method)
        {
            ParameterInfo parameterInfo = method.GetParameters()[0];

            // instance parameter
            ParameterExpression instance = Expression
                .Parameter(typeof(TObject), "instance");

            // object input paramter
            ParameterExpression parameter = Expression
                .Parameter(typeof(object), "setParam");

            // convert object to appropriate type for "set;"
            UnaryExpression convertedParameter = Expression
                .Convert(parameter, parameterInfo.ParameterType);

            // call expression
            MethodCallExpression methodCallExp = Expression
                .Call(instance, method, convertedParameter);

            // convert to a set method delegate
            return Expression.Lambda<SetMethodDelegate<TObject>>(methodCallExp, instance, parameter)
                .Compile();
        }

        private static IEnumerable<PropertyInfo> GetSetMethods()
        {
            // Get public instance set methods for object 
            return typeof(TObject)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.SetMethod != null);
        }

        private static SetMethodDelegate<TObject>[] GetSetters(DbDataReader reader)
        {
            var setters = new SetMethodDelegate<TObject>[reader.VisibleFieldCount];

            for (int i = 0; i < reader.VisibleFieldCount; i++)
            {
                string fieldName = CleanString(reader.GetName(i));

                SetMethodDelegate<TObject> setter;
                SetterCache.TryGetValue(fieldName, out setter);
                setters[i] = setter;
            }

            return setters;
        }

        private static TObject MapSingle(IDataReader reader, SetMethodDelegate<TObject>[] setters)
        {
            var item = new TObject();

            for (int i = 0; i < setters.Length; i++)
            {
                SetMethodDelegate<TObject> setter = setters[i];
                if (setter != null)
                {
                    if (!reader.IsDBNull(i))
                    {
                        // If there is a setter for that field
                        // call the setter with the value from the reader
                        object value = reader[i];
                        setter(item, value);
                    }
                }
            }

            return item;
        }

        public static TObject Map(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            TObject obj = null;

            // Cache Setters
            SetMethodDelegate<TObject>[] setters = GetSetters(reader);

            if (reader.Read())
            {
                obj = MapSingle(reader, setters);
            }

            return obj;
        }

        public static async Task<TObject> MapAsync(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            TObject obj = null;

            // Cache Setters
            SetMethodDelegate<TObject>[] setters = GetSetters(reader);

            if (await reader.ReadAsync())
            {
                obj = MapSingle(reader, setters);
            }

            return obj;
        }

        public static IReadOnlyList<TObject> MapAll(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var itemList = new List<TObject>();
            
            // Cache Setters
            var setters = GetSetters(reader);

            while (reader.Read())
            {
                var item = MapSingle(reader, setters);
                itemList.Add(item);
            }

            return itemList;
        }

        public static async Task<IReadOnlyList<TObject>> MapAllAsync(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var itemList = new List<TObject>();
            var setters = GetSetters(reader);

            while (await reader.ReadAsync())
            {
                TObject row = MapSingle(reader, setters);
                itemList.Add(row);
            }
            
            return itemList;
        }
    }

    /// <summary>
    /// Non Generic Wrapper for
    /// <seealso cref="ObjectMapper{TObject}"/>
    /// </summary>
    public static class ObjectMapper {

        private static readonly Dictionary<string, Func<DbDataReader, object>>
            mapDict = new Dictionary<string, Func<DbDataReader, object>>();

        private static readonly Dictionary<string, Func<DbDataReader, IEnumerable>>
            mapAllDict = new Dictionary<string, Func<DbDataReader, IEnumerable>>();

        private static readonly Dictionary<string, Func<DbDataReader, Task>>
            mapAsyncDict = new Dictionary<string, Func<DbDataReader, Task>>();

        private static Func<DbDataReader, TOut> CompileFunction<TOut>(Type genericType, string methodName)
        {
            var method = typeof(ObjectMapper<>).MakeGenericType(genericType)
                .GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);

            var parameter = Expression.Parameter(typeof(DbDataReader), "reader");
            var expressionCall = Expression.Call(method, parameter);
            var convertedExpression = Expression.Convert(expressionCall, typeof(TOut));

            return Expression.Lambda<Func<DbDataReader, TOut>>(convertedExpression, parameter)
                .Compile();
        }

        private static void GenerateCompiledMappers(Type genericType)
        {
            Func<DbDataReader, object> map = 
                CompileFunction<object>(genericType, nameof(ObjectMapper<object>.Map));

            Func<DbDataReader, IEnumerable> mapAll = 
                CompileFunction<IEnumerable>(genericType, nameof(ObjectMapper<object>.Map));

            Func<DbDataReader, Task> mapAsync =
                CompileFunction<Task>(genericType, nameof(ObjectMapper<object>.MapAsync));

            mapDict.Add(genericType.Name, map);
            mapAsyncDict.Add(genericType.Name, mapAsync);
            mapAllDict.Add(genericType.Name, mapAll);
        }

        public static Func<DbDataReader, object> Map(Type genericType)
        {
            if (genericType == null)
            {
                throw new ArgumentNullException(nameof(genericType));
            }

            if (!mapDict.TryGetValue(genericType.Name, out Func<DbDataReader, object> mapper))
            {
                GenerateCompiledMappers(genericType);
                mapper = mapDict[genericType.Name];
            }

            return mapper;
        }

        public static Func<DbDataReader, Task> MapAsync(Type genericType)
        {
            if (genericType == null)
            {
                throw new ArgumentNullException(nameof(genericType));
            }

            if (!mapAsyncDict.TryGetValue(genericType.Name, out Func<DbDataReader, Task> mapper))
            {
                GenerateCompiledMappers(genericType);
                mapper = mapAsyncDict[genericType.Name];
            }

            return mapper;
        }

        public static Func<DbDataReader, IEnumerable> MapAll(Type genericType)
        {
            if (genericType == null)
            {
                throw new ArgumentNullException(nameof(genericType));
            }

            if (!mapAllDict.TryGetValue(genericType.Name, out Func<DbDataReader, IEnumerable> mapper))
            {
                GenerateCompiledMappers(genericType);
                mapper = mapAllDict[genericType.Name];
            }

            return mapper;
        }
    }
}
