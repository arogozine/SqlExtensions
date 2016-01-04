using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SqlExtensions;
using MySql.Data.MySqlClient;

namespace TestConsole
{
    class Program
    {
        private static MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder
        {
            Database = "classicmodels",
            UserID = "John",
            Password = "TODO", //TODO
            Server = "localhost",
            Port = 3306,
        };

        static void Main(string[] args)
        {
            var a = new { Test = "test" };
            var b = ParamMapper.GenerateTest(a);

            // var action = ParamMapper.GenerateParameterMap(new { Foo = "bar", });

            //var t = ParamMapper.GenerateParameterMap(new { OfficeCode = "1" });
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            // Offices office = connector.QuerySingle("SELECT * FROM `classicmodels`.`offices` WHERE `OfficeCode` = @OfficeCode", ObjectMapper<Offices>.Map, Tuple.Create("OfficeCode", "1"));
            // Assert.IsNotNull(office);

            var office = connector.QuerySingle("SELECT * FROM `classicmodels`.`offices` WHERE `OfficeCode` = @OfficeCode", Mapper.String, new { OfficeCode = "1" });
            return;
            /*
            TypeConverter.AddEnumConverters(typeof(System.Reflection.BindingFlags));
            byte output = TypeConverter.Convert<int, byte>(1);
            byte? output2 = TypeConverter.Convert<int, byte?>(1);
            byte? output3 = TypeConverter.Convert<int?, byte?>(1);
            byte output4 = TypeConverter.Convert<int?, byte>(1);


            var results = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.Namespace == nameof(System))
                .Where(t => t.IsPublic)
                .Where(t => t.IsValueType)
                .Where(t => typeof(ArgIterator) != t)
                .Where(t => typeof(RuntimeArgumentHandle) != t)
                .Where(t => typeof(TypedReference) != t)
                .Where(t => typeof(void) != t)
                .Where(t => !t.IsGenericType)
                .Where(t => !t.IsEnum)
                .ToList();

            var test = GenerateConverters(results);*/
        }

        static string GenerateConverters(List<Type> types) {
            var sb = new StringBuilder();
            int i = 0;
            foreach (Type from in types)
            {
                /*
                sb
    .Append("GenerateConverter<")
    .Append(from.Name)
    .Append(", Nullable<")
    .Append(from.Name)
    .Append(">>();")
    .Append(Environment.NewLine);
                */
                Console.WriteLine("{0} / {1}", i++, types.Count);
                foreach (Type to in types)
                {
                    if (to == from)
                        continue;
                    
                    CheckedConverter(from, to, sb);
                }
            }

            return sb.ToString();
        }

        static void CheckedConverter(Type from, Type to, StringBuilder sb) {
            
            try
            {
                var func =  GenerateConverter(from, to);
                sb
                    .Append("GenerateConverter<")
                    .Append(from.Name)
                    .Append(", ")
                    .Append(to.Name)
                    .Append(">();")
                    .Append(Environment.NewLine);
            }
            catch (InvalidOperationException) {

            }
            catch (ArgumentException) { }
        }

        static Func<object, object> GenerateConverter(Type from, Type to) {

            // Object
            var fromParam = Expression.Parameter(typeof(object), "from");
            // (TFrom)Object
            var fromCast = Expression.Convert(fromParam, from);
            // (TTo)(TFrom)Object
            var fromToCast = Expression.Convert(fromCast, to);
            // (object)(TTo)(TFrom)Object
            var toObjectCast = Expression.Convert(fromToCast, typeof(object));

            return Expression.Lambda<Func<object, object>>(toObjectCast, fromParam)
                .Compile();
        }
    }
}
