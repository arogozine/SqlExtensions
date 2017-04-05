using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SqlExtensions
{
    public static class DbTransactionExtAsync
    {
        #region Auto Commit

        public static async Task AutoCommitAsync(this DbTransaction tran, Func<DbTransaction, Task> action)
        {
            try
            {
                await action(tran);
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public static async Task<TOut> AutoCommitAsync<TOut>(this DbTransaction tran, Func<DbTransaction, Task<TOut>> func)
        {
            try
            {
                return await func(tran);
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        #endregion

        #region Using DbCommand

        public static async Task<IReadOnlyList<TOut>> UsingCommandAsync<TOut>(this DbTransaction tran, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, object parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.Transaction = tran;
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return await func(dbCommand);
            }
        }
        
        public static async Task<IReadOnlyList<TOut>> UsingCommandAsync<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, IEnumerable<(string name, TValue value)> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.Transaction = tran;
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return await func(dbCommand);
            }
        }

        public static async Task<IReadOnlyList<TOut>> UsingCommandAsync<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.Transaction = tran;
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return await func(dbCommand);
            }
        }

        #endregion

        #region Using DbCommand Action

        public static async Task UsingCommandAsync(this DbTransaction tran, string query, Func<DbCommand, Task> action, object parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;
                await action(dbCommand);
            }
        }

        public static async Task UsingCommandAsync<TValue>(this DbTransaction tran, string query, Func<DbCommand, Task> action, params (string name, TValue value)[] parameters)
        {
            await UsingCommandAsync(tran, query, action, (IEnumerable<(string, TValue)>)parameters);
        }

        public static async Task UsingCommandAsync<TValue>(this DbTransaction tran, string query, Func<DbCommand, Task> action, IEnumerable<(string name, TValue value)> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;
                await action(dbCommand);
            }
        }

        public static async Task UsingCommandAsync<TValue>(this DbTransaction tran, string query, Func<DbCommand, Task> action, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;
                await action(dbCommand);
            }
        }

        #endregion

        #region Using DbCommand Function

        public static async Task<TOut> UsingCommandAsync<TOut>(this DbTransaction tran, string query, Func<DbCommand, Task<TOut>> func, object parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return await func(dbCommand);
            }
        }
        
        public static async Task<TOut> UsingCommandAsync<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, Task<TOut>> func, params (string name, TValue value)[] parameters)
        {
            return await UsingCommandAsync(tran, query, func, (IEnumerable<(string, TValue)>)parameters);
        }

        public static async Task<TOut> UsingCommandAsync<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, Task<TOut>> func, IEnumerable<(string name, TValue value)> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return await func(dbCommand);
            }
        }

        public static async Task<TOut> UsingCommandAsync<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, Task<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return await func(dbCommand);
            }
        }

        #endregion

        #region Query List DbCommand

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut>(this DbTransaction tran, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, object parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return await func(dbCommand);
            }
        }

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, params (string name, TValue value)[] parameters)
        {
            return await UsingCommandAsync(tran, query, func, (IEnumerable<(string, TValue)>)parameters);
        }

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, IEnumerable<(string name, TValue value)> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return await func(dbCommand);
            }
        }

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return await func(dbCommand);
            }
        }

        #endregion

        #region Query List IDataReader

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut>(this DbTransaction tran, string query, Func<IDataReader, Task<IReadOnlyList<TOut>>> func, object parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return await dbCommand.QueryListAsync(func);
            }
        }

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(this DbTransaction tran, string query, Func<IDataReader, Task<IReadOnlyList<TOut>>> func, params (string name, TValue value)[] parameters)
        {
            return await QueryListAsync(tran, query, func, (IEnumerable<(string, TValue)>)parameters);
        }

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(this DbTransaction tran, string query, Func<IDataReader, Task<IReadOnlyList<TOut>>> func, IEnumerable<(string name, TValue value)> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return await dbCommand.QueryListAsync(func);
            }
        }

        public static async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(this DbTransaction tran, string query, Func<IDataReader, Task<IReadOnlyList<TOut>>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return await dbCommand.QueryListAsync(func);
            }
        }

        #endregion

        #region Query Single DbCommand

        public static async Task<TOut> QuerySingle<TOut>(this DbTransaction conn, string query, Func<DbCommand, Task<TOut>> func, object parameters)
            => await conn.UsingCommandAsync(query, func, parameters);

        public static async Task<TOut> QuerySingle<TOut, TValue>(this DbTransaction conn, string query, Func<DbCommand, Task<TOut>> func, params (string name, TValue value)[] parameters)
            => await conn.UsingCommandAsync(query, func, parameters);

        public static async Task<TOut> QuerySingle<TOut, TValue>(this DbTransaction conn, string query, Func<DbCommand, Task<TOut>> func, IEnumerable<(string name, TValue value)> parameters)
            => await conn.UsingCommandAsync(query, func, parameters);

        public static async Task<TOut> QuerySingle<TOut, TValue>(this DbTransaction conn, string query, Func<DbCommand, Task<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => await conn.UsingCommandAsync(query, func, parameters);

        #endregion

        #region Query Single IDataReader

        public static async Task<TOut> QuerySingle<TOut>(this DbTransaction conn, string query, Func<IDataReader, Task<TOut>> func, object parameters)
            => await conn.UsingCommandAsync(query, cmd => cmd.QuerySingleAsync(func), parameters);

        public static async Task<TOut> QuerySingle<TOut, TValue>(this DbTransaction conn, string query, Func<IDataReader, Task<TOut>> func, params (string name, TValue value)[] parameters)
            => await conn.UsingCommandAsync(query, cmd => cmd.QuerySingleAsync(func), parameters);

        public static async Task<TOut> QuerySingle<TOut, TValue>(this DbTransaction conn, string query, Func<IDataReader, Task<TOut>> func, IEnumerable<(string name, TValue value)> parameters)
            => await conn.UsingCommandAsync(query, cmd => cmd.QuerySingleAsync(func), parameters);

        public static async Task<TOut> QuerySingle<TOut, TValue>(this DbTransaction conn, string query, Func<IDataReader, Task<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => await conn.UsingCommandAsync(query, cmd => cmd.QuerySingleAsync(func), parameters);

        #endregion

        #region NonQuery

        public static async Task<int> NonQueryAsync(this DbTransaction tran, string sql, object parameters)
            => await tran.UsingCommandAsync(sql, cmd => cmd.ExecuteNonQueryAsync(), parameters);

        public static async Task<int> NonQueryAsync<TValue>(this DbTransaction tran, string sql, params (string name, TValue value)[] parameters)
            => await tran.UsingCommandAsync(sql, cmd => cmd.ExecuteNonQueryAsync(), parameters);

        public static async Task<int> NonQueryAsync<TValue>(this DbTransaction tran, string sql, IEnumerable<(string name, TValue value)> parameters)
            => await tran.UsingCommandAsync(sql, cmd => cmd.ExecuteNonQueryAsync(), parameters);

        public static async Task<int> NonQueryAsync<TValue>(this DbTransaction tran, string sql, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => await tran.UsingCommandAsync(sql, cmd => cmd.ExecuteNonQueryAsync(), parameters);

        #endregion
    }
}