using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExtensions
{
    public static class DbDataReaderExtAsync
    {
        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut>(this DbDataReader reader, Func<IDataRecord, TOut> func)
        {
            List<TOut> list = new List<TOut>();

            while (await reader.ReadAsync())
            {
                TOut result = func(reader);
                list.Add(result);
            }

            return list;
        }

        public static async Task<T> QuerySingleAsync<T>(this DbDataReader reader, Func<IDataRecord, T> func)
            => await reader.ReadAsync() ? func(reader) : default(T);
    }
}
