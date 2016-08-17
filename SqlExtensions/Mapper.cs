using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExtensions
{
    public static class Mapper
    {
        public static IReadOnlyDictionary<string, string> StringSingle(IDataRecord reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var dict = new Dictionary<string, string>(reader.FieldCount, StringComparer.CurrentCultureIgnoreCase);

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                string value = reader.IsDBNull(i) ? null : reader.GetValue(i).ToString();
                dict[name] = value;
            }

            return dict;
        }

        public static IReadOnlyDictionary<string, object> ObjectSingle(IDataRecord reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var dict = new Dictionary<string, object>(reader.FieldCount, StringComparer.CurrentCultureIgnoreCase);

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                object value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                dict[name] = value;
            }

            return dict;
        }

        public static dynamic DynamicSingle(IDataRecord reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            IDictionary<string, object> dict = new System.Dynamic.ExpandoObject();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                object value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                dict[name] = value;
            }

            return dict;
        }

        public static IReadOnlyList<IReadOnlyDictionary<string, string>> String(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var list = new List<IReadOnlyDictionary<string, string>>();

            while (reader.Read())
            {
                var dict = StringSingle(reader);
                list.Add(dict);
            }

            return list;
        }

        public static async Task<IReadOnlyList<IReadOnlyDictionary<string, string>>> StringAsync(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var list = new List<IReadOnlyDictionary<string, string>>();

            while (await reader.ReadAsync())
            {
                var dict = StringSingle(reader);
                list.Add(dict);
            }

            return list;
        }

        public static IReadOnlyList<IReadOnlyDictionary<string, object>> Object(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var list = new List<IReadOnlyDictionary<string, object>>();

            while (reader.Read())
            {
                var dict = ObjectSingle(reader);
                list.Add(dict);
            }

            return list;
        }

        public static async Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> ObjectAsync(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var list = new List<IReadOnlyDictionary<string, object>>();

            while (await reader.ReadAsync())
            {
                var dict = ObjectSingle(reader);
                list.Add(dict);
            }

            return list;
        }

        public static IReadOnlyList<dynamic> Dynamic(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var list = new List<dynamic>();

            while (reader.Read())
            {
                var dict = DynamicSingle(reader);
                list.Add(dict);
            }

            return list;
        }

        public static async Task<IReadOnlyList<dynamic>> DynamicAsync(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var list = new List<dynamic>();

            while (await reader.ReadAsync())
            {
                var dict = DynamicSingle(reader);
                list.Add(dict);
            }

            return list;
        }
    }
}
