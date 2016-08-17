using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SqlExtensions
{
    public static class DbConnectionExtAsync
    {
        #region Commit Transaction

        public static async Task CommitTransactionAsync(this DbConnection conn, Func<DbTransaction, Task> action)
            => await conn.UsingTransactionAsync(tran => tran.AutoCommitAsync(action));

        public static async Task CommitTransactionAsync(this DbConnection conn, Func<DbTransaction, Task> action, IsolationLevel isolationLevel)
            => await conn.UsingTransactionAsync(tran => tran.AutoCommitAsync(action), isolationLevel);

        public static async Task<T> CommitTransactionAsync<T>(this DbConnection conn, Func<DbTransaction, Task<T>> function)
            => await conn.UsingTransactionAsync(tran => tran.AutoCommitAsync(function));

        public static async Task<T> CommitTransactionAsync<T>(this DbConnection conn, Func<DbTransaction, Task<T>> function, IsolationLevel isolationLevel)
            => await conn.UsingTransactionAsync(tran => tran.AutoCommitAsync(function), isolationLevel);

        #endregion

        #region Using Transaction

        public static async Task UsingTransactionAsync(this DbConnection conn, Func<DbTransaction, Task> action)
        {
            using (var tran = conn.BeginTransaction())
            {
                await action(tran);
            }
        }

        public static async Task UsingTransactionAsync(this DbConnection conn, Func<DbTransaction, Task> action, IsolationLevel isolationLevel)
        {
            using (var tran = conn.BeginTransaction(isolationLevel))
            {
                await action(tran);
            }
        }

        public static async Task<T> UsingTransactionAsync<T>(this DbConnection conn, Func<DbTransaction, Task<T>> function)
        {
            using (var tran = conn.BeginTransaction())
            {
                return await function(tran);
            }
        }

        public static async Task<T> UsingTransactionAsync<T>(this DbConnection conn, Func<DbTransaction, Task<T>> function, IsolationLevel isolationLevel)
        {
            using (var tran = conn.BeginTransaction(isolationLevel))
            {
                return await function(tran);
            }
        }

        #endregion

        #region Using DbCommand

        public static async Task<IReadOnlyList<TOut>> UsingCommandAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, params Tuple<string, TValue>[] parameters)
        {
            return await UsingCommandAsync(conn, query, func, (IEnumerable<Tuple<string, TValue>>)parameters);
        }

        public static async Task<IReadOnlyList<TOut>> UsingCommandAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, IEnumerable<Tuple<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return await func(dbCommand);
            }
        }

        public static async Task<IReadOnlyList<TOut>> UsingCommandAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return await func(dbCommand);
            }
        }

        public static async Task<IReadOnlyList<TOut>> UsingCommandAsync<TOut>(this DbConnection conn, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, object parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return await func(dbCommand);
            }
        }

        public static async Task<TOut> UsingCommandAsync<TOut>(this DbConnection conn, string query, Func<DbCommand, Task<TOut>> func)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.CommandText = query;
                return await func(dbCommand);
            }
        }

        #endregion

        #region Using DbCommand Action

        public static async Task UsingCommandAsync<TValue>(this DbConnection conn, string query, Func<DbCommand, Task> action, params Tuple<string, TValue>[] parameters)
        {
            await UsingCommandAsync(conn, query, action, (IEnumerable<Tuple<string, TValue>>)parameters);
        }

        public static async Task UsingCommandAsync<TValue>(this DbConnection conn, string query, Func<DbCommand, Task> action, IEnumerable<Tuple<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                await action(dbCommand);
            }
        }

        public static async Task UsingCommandAsync<TValue>(this DbConnection conn, string query, Func<DbCommand, Task> action, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                await action(dbCommand);
            }
        }

        public static async Task UsingCommandAsync(this DbConnection conn, string query, Func<DbCommand, Task> action, object parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                await action(dbCommand);
            }
        }

        #endregion

        #region Using DbCommand Function

        public static async Task<TOut> UsingCommandAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, Task<TOut>> func, params Tuple<string, TValue>[] parameters)
        {
            return await UsingCommandAsync(conn, query, func, (IEnumerable<Tuple<string, TValue>>)parameters);
        }

        public static async Task<TOut> UsingCommandAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, Task<TOut>> func, IEnumerable<Tuple<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return await func(dbCommand);
            }
        }

        public static async Task<TOut> UsingCommandAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, Task<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return await func(dbCommand);
            }
        }

        public static async Task<TOut> UsingCommandAsync<TOut>(this DbConnection conn, string query, Func<DbCommand, Task<TOut>> func, object parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return await func(dbCommand);
            }
        }

        #endregion

        #region Query List DbCommand

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut>(this DbConnection conn, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func)
            => await conn.UsingCommandAsync(query, func);

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut>(this DbConnection conn, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, object parameters)
            => await conn.UsingCommandAsync(query, func, parameters);

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, params Tuple<string, TValue>[] parameters)
            => await conn.UsingCommandAsync(query, func, parameters);

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, IEnumerable<Tuple<string, TValue>> parameters)
            => await conn.UsingCommandAsync(query, func, parameters);

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => await conn.UsingCommandAsync(query, func, parameters);

        #endregion

        #region Query List DbDataReader

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut>(this DbConnection conn, string query, Func<DbDataReader, Task<IReadOnlyList<TOut>>> func)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.CommandText = query;
                return await dbCommand.QueryListAsync(func);
            }
        }

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut>(this DbConnection conn, string query, Func<DbDataReader, Task<IReadOnlyList<TOut>>> func, object parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return await dbCommand.QueryListAsync(func);
            }
        }

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbDataReader, Task<IReadOnlyList<TOut>>> func, params Tuple<string, TValue>[] parameters)
        {
            return await QueryListAsync(conn, query, func, (IEnumerable<Tuple<string, TValue>>)parameters);
        }

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbDataReader, Task<IReadOnlyList<TOut>>> func, IEnumerable<Tuple<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return await dbCommand.QueryListAsync(func);
            }
        }

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbDataReader, Task<IReadOnlyList<TOut>>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return await dbCommand.QueryListAsync(func);
            }
        }

        #endregion

        #region Query Single DbCommand

        public static async Task<TOut> QuerySingleAsync<TOut>(this DbConnection conn, string query, Func<DbCommand, Task<TOut>> func)
            => await conn.UsingCommandAsync(query, func);

        public static async Task<TOut> QuerySingleAsync<TOut>(this DbConnection conn, string query, Func<DbCommand, Task<TOut>> func, object parameters)
            => await conn.UsingCommandAsync(query, func, parameters);

        public static async Task<TOut> QuerySingleAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, Task<TOut>> func, params Tuple<string, TValue>[] parameters)
            => await conn.UsingCommandAsync(query, func, parameters);

        public static async Task<TOut> QuerySingleAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, Task<TOut>> func, IEnumerable<Tuple<string, TValue>> parameters)
            => await conn.UsingCommandAsync(query, func, parameters);

        public static async Task<TOut> QuerySingleAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, Task<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => await conn.UsingCommandAsync(query, func, parameters);

        #endregion

        #region Query Single DbDataReader

        public static async Task<TOut> QuerySingleAsync<TOut>(this DbConnection conn, string query, Func<DbDataReader, Task<TOut>> func)
            => await conn.UsingCommandAsync(query, cmd => cmd.QuerySingle(func));

        public static async Task<TOut> QuerySingleAsync<TOut>(this DbConnection conn, string query, Func<DbDataReader, Task<TOut>> func, object parameters)
            => await conn.UsingCommandAsync(query, cmd => cmd.QuerySingle(func), parameters);

        public static async Task<TOut> QuerySingleAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbDataReader, Task<TOut>> func, params Tuple<string, TValue>[] parameters)
            => await conn.UsingCommandAsync(query, cmd => cmd.QuerySingle(func), parameters);

        public static async Task<TOut> QuerySingleAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbDataReader, Task<TOut>> func, IEnumerable<Tuple<string, TValue>> parameters)
            => await conn.UsingCommandAsync(query, cmd => cmd.QuerySingle(func), parameters);

        public static async Task<TOut> QuerySingleAsync<TOut, TValue>(this DbConnection conn, string query, Func<DbDataReader, Task<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => await conn.UsingCommandAsync(query, cmd => cmd.QuerySingle(func), parameters);

        #endregion

        #region NonQuery

        public static async Task<int> NonQueryAsync(this DbConnection conn, string sql, object parameters)
            => await conn.UsingCommandAsync(sql, cmd => cmd.ExecuteNonQueryAsync(), parameters);

        public static async Task<int> NonQueryAsync<TValue>(this DbConnection conn, string sql, params Tuple<string, TValue>[] parameters)
            => await conn.UsingCommandAsync(sql, cmd => cmd.ExecuteNonQueryAsync(), parameters);

        public static async Task<int> NonQueryAsync<TValue>(this DbConnection conn, string sql, IEnumerable<Tuple<string, TValue>> parameters)
            => await conn.UsingCommandAsync(sql, cmd => cmd.ExecuteNonQueryAsync(), parameters);

        public static async Task<int> NonQueryAsync<TValue>(this DbConnection conn, string sql, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => await conn.UsingCommandAsync(sql, cmd => cmd.ExecuteNonQueryAsync(), parameters);

        #endregion
    }
}
