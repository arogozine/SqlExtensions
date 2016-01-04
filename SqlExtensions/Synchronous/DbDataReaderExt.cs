using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExtensions
{
    public static class DbDataReaderExt
    {
        public static IReadOnlyList<T> QueryList<T>(this DbDataReader reader, Func<IDataRecord, T> func)
        {
            List<T> list = new List<T>();

            while (reader.Read())
            {
                T result = func(reader);
                list.Add(result);
            }

            return list;
        }

        public static T QuerySingle<T>(this DbDataReader reader, Func<IDataRecord, T> func)
            => reader.Read() ? func(reader) : default(T);
    }
}
