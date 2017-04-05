using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExtensions
{
    public static class DbCommandExtAsync
    {
        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut>(this DbCommand cmd, Func<DbDataReader, Task<IReadOnlyList<TOut>>> func)
        {
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await func(reader);
            }
        }

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut>(this DbCommand cmd, Func<IDataRecord, TOut> func)
        {
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await reader.QueryListAsync(func);
            }
        }

        public static async Task<TOut> QuerySingleAsync<TOut>(this DbCommand cmd, Func<DbDataReader, Task<TOut>> func)
        {
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await reader.ReadAsync() ? await func(reader) : default(TOut);
            }
        }

        public static async Task<TOut> QuerySingle<TOut>(this DbCommand cmd, Func<IDataRecord, TOut> func)
        {
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await reader.QuerySingleAsync(func);
            }
        }

        public static async Task UsingReaderAsync(this DbCommand cmd, Func<DbDataReader, Task> action)
        {
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                await action(reader);
            }
        }
    }
}
