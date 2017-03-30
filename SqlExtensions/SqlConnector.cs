using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SqlExtensions
{
    public class SqlConnector
    {
        private readonly Func<DbConnection> generator;

        public SqlConnector(Func<DbConnection> generator)
        {
            this.generator = generator;
        }

        #region Using Connection

        public void UsingConnection(Action<DbConnection> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (DbConnection conn = generator())
            {
                conn.Open();
                action(conn);
            }
        }

        public TOut UsingConnection<TOut>(Func<DbConnection, TOut> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            using (DbConnection conn = generator())
            {
                conn.Open();
                return func(conn);
            }
        }

        public async Task UsingConnectionAsync(Func<DbConnection, Task> asyncAction)
        {
            using (DbConnection conn = generator())
            {
                await conn.OpenAsync();
                await asyncAction(conn);
            }
        }

        public async Task<TOut> UsingConnectionAsync<TOut>(Func<DbConnection, Task<TOut>> asyncFunc)
        {
            using (DbConnection conn = generator())
            {
                await conn.OpenAsync();
                return await asyncFunc(conn);
            }
        }

        #endregion

        #region Using Transaction

        public void UsingTransaction(Action<DbTransaction> action)
            => UsingConnection(conn => conn.UsingTransaction(action));

        public void UsingTransaction(Action<DbTransaction> action, IsolationLevel isolationLevel)
            => UsingConnection(conn => conn.UsingTransaction(action, isolationLevel));

        public TOut UsingTransaction<TOut>(Func<DbTransaction, TOut> func)
            => UsingConnection(conn => conn.UsingTransaction(func));

        public TOut UsingTransaction<TOut>(Func<DbTransaction, TOut> func, IsolationLevel isolationLevel)
            => UsingConnection(conn => conn.UsingTransaction(func, isolationLevel));

        public async Task UsingTransactionAsync(Func<DbTransaction, Task> asyncAction)
            => await UsingConnectionAsync(conn => conn.UsingTransactionAsync(asyncAction));

        public async Task UsingTransactionAsync(Func<DbTransaction, Task> asyncAction, IsolationLevel isolationLevel)
            => await UsingConnectionAsync(conn => conn.UsingTransactionAsync(asyncAction, isolationLevel));

        public async Task<TOut> UsingTransactionAsync<TOut>(Func<DbTransaction, Task<TOut>> asyncFunction)
            => await UsingConnectionAsync(conn => conn.UsingTransactionAsync(asyncFunction));

        public async Task<TOut> UsingTransactionAsync<TOut>(Func<DbTransaction, Task<TOut>> asyncFunction, IsolationLevel isolationLevel)
            => await UsingConnectionAsync(conn => conn.UsingTransactionAsync(asyncFunction, isolationLevel));

        #endregion

        #region Query List DbCommand

        public IReadOnlyList<TOut> QueryList<TOut>(string query, Func<DbCommand, IReadOnlyList<TOut>> func)
            => UsingConnection(conn => conn.QueryList(query, func));

        public IReadOnlyList<TOut> QueryList<TOut>(string query, Func<DbCommand, IReadOnlyList<TOut>> func, object parameters)
            => UsingConnection(conn => conn.QueryList(query, func, parameters));

        public IReadOnlyList<TOut> QueryList<TOut, TValue>(string query, Func<DbCommand, IReadOnlyList<TOut>> func, params (string, TValue)[] parameters)
            => UsingConnection(conn => conn.QueryList(query, func, parameters));

        public IReadOnlyList<TOut> QueryList<TOut, TValue>(string query, Func<DbCommand, IReadOnlyList<TOut>> func, IEnumerable<(string, TValue)> parameters)
            => UsingConnection(conn => conn.QueryList(query, func, parameters));

        public async Task<IReadOnlyList<TOut>> QueryListAsync<TOut>(string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func)
            => await UsingConnectionAsync(conn => conn.QueryListAsync(query, func));

        public async Task<IReadOnlyList<TOut>> QueryListAsync<TOut>(string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, object parameters)
            => await UsingConnectionAsync(conn => conn.QueryListAsync(query, func, parameters));

        public async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, params (string, TValue)[] parameters)
            => await UsingConnectionAsync(conn => conn.QueryListAsync(query, func, parameters));

        public async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, IEnumerable<(string, TValue)> parameters)
            => await UsingConnectionAsync(conn => conn.QueryListAsync(query, func, parameters));

        #endregion

        #region Query List DbDataReader

        public IReadOnlyList<TOut> QueryList<TOut>(string query, Func<DbDataReader, IReadOnlyList<TOut>> func)
            => UsingConnection(conn => conn.QueryList(query, func));

        public IReadOnlyList<TOut> QueryList<TOut>(string query, Func<DbDataReader, IReadOnlyList<TOut>> func, object parameters)
            => UsingConnection(conn => conn.QueryList(query, func, parameters));

        public IReadOnlyList<TOut> QueryList<TOut, TValue>(string query, Func<DbDataReader, IReadOnlyList<TOut>> func, params (string, TValue)[] parameters)
            => UsingConnection(conn => conn.QueryList(query, func, parameters));

        public IReadOnlyList<TOut> QueryList<TOut, TValue>(string query, Func<DbDataReader, IReadOnlyList<TOut>> func, IEnumerable<(string, TValue)> parameters)
            => UsingConnection(conn => conn.QueryList(query, func, parameters));

        public IReadOnlyList<TOut> QueryList<TOut, TValue>(string query, Func<DbCommand, IReadOnlyList<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => UsingConnection(conn => conn.QueryList(query, func, parameters));

        public async Task<IReadOnlyList<TOut>> QueryListAsync<TOut>(string query, Func<DbDataReader, Task<IReadOnlyList<TOut>>> func)
            => await UsingConnectionAsync(conn => conn.QueryListAsync(query, func));

        public async Task<IReadOnlyList<TOut>> QueryListAsync<TOut>(string query, Func<DbDataReader, Task<IReadOnlyList<TOut>>> func, object parameters)
            => await UsingConnectionAsync(conn => conn.QueryListAsync(query, func, parameters));

        public async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(string query, Func<DbDataReader, Task<IReadOnlyList<TOut>>> func, params (string, TValue)[] parameters)
            => await UsingConnectionAsync(conn => conn.QueryListAsync(query, func, parameters));

        public async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(string query, Func<DbDataReader, Task<IReadOnlyList<TOut>>> func, IEnumerable<(string, TValue)> parameters)
            => await UsingConnectionAsync(conn => conn.QueryListAsync(query, func, parameters));

        public async Task<IReadOnlyList<TOut>> QueryListAsync<TOut, TValue>(string query, Func<DbCommand, Task<IReadOnlyList<TOut>>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => await UsingConnectionAsync(conn => conn.QueryListAsync(query, func, parameters));

        #endregion

        #region Query Single DbCommand

        public TOut QuerySingle<TOut>(string query, Func<DbCommand, TOut> func)
            => UsingConnection(conn => conn.QuerySingle(query, func));

        public TOut QuerySingle<TOut>(string query, Func<DbCommand, TOut> func, object parameters)
            => UsingConnection(conn => conn.QuerySingle(query, func, parameters));

        public TOut QuerySingle<TOut, TValue>(string query, Func<DbCommand, TOut> func, params (string, TValue)[] parameters)
            => UsingConnection(conn => conn.QuerySingle(query, func, parameters));

        public TOut QuerySingle<TOut, TValue>(string query, Func<DbCommand, TOut> func, IEnumerable<(string, TValue)> parameters)
            => UsingConnection(conn => conn.QuerySingle(query, func, parameters));

        public TOut QuerySingle<TOut, TValue>(string query, Func<DbCommand, TOut> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => UsingConnection(conn => conn.QuerySingle(query, func, parameters));

        public async Task<TOut> QuerySingleAsync<TOut>(string query, Func<DbCommand, Task<TOut>> func)
            => await UsingConnectionAsync(conn => conn.QuerySingle(query, func));

        public async Task<TOut> QuerySingleAsync<TOut>(string query, Func<DbCommand, Task<TOut>> func, object parameters)
            => await UsingConnectionAsync(conn => conn.QuerySingle(query, func, parameters));

        public async Task<TOut> QuerySingleAsync<TOut, TValue>(string query, Func<DbCommand, Task<TOut>> func, params (string, TValue)[] parameters)
            => await UsingConnectionAsync(conn => conn.QuerySingle(query, func, parameters));

        public async Task<TOut> QuerySingleAsync<TOut, TValue>(string query, Func<DbCommand, Task<TOut>> func, IEnumerable<(string, TValue)> parameters)
            => await UsingConnectionAsync(conn => conn.QuerySingle(query, func, parameters));

        public async Task<TOut> QuerySingleAsync<TOut, TValue>(string query, Func<DbCommand, Task<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => await UsingConnectionAsync(conn => conn.QuerySingle(query, func, parameters));

        #endregion

        #region Query Single DbDataReader

        public TOut QuerySingle<TOut>(string query, Func<DbDataReader, TOut> func)
            => UsingConnection(conn => conn.QuerySingle(query, func));

        public TOut QuerySingle<TOut>(string query, Func<DbDataReader, TOut> func, object parameters)
            => UsingConnection(conn => conn.QuerySingle(query, func, parameters));

        public TOut QuerySingle<TOut, TValue>(string query, Func<DbDataReader, TOut> func, params (string, TValue)[] parameters)
            => UsingConnection(conn => conn.QuerySingle(query, func, parameters));

        public TOut QuerySingle<TOut, TValue>(string query, Func<DbDataReader, TOut> func, IEnumerable<(string, TValue)> parameters)
            => UsingConnection(conn => conn.QuerySingle(query, func, parameters));

        public TOut QuerySingle<TOut, TValue>(string query, Func<DbDataReader, TOut> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => UsingConnection(conn => conn.QuerySingle(query, func, parameters));

        public async Task<TOut> QuerySingleAsync<TOut>(string query, Func<DbDataReader, Task<TOut>> func)
            => await UsingConnectionAsync(conn => conn.QuerySingle(query, func));

        public async Task<TOut> QuerySingleAsync<TOut>(string query, Func<DbDataReader, Task<TOut>> func, object parameters)
            => await UsingConnectionAsync(conn => conn.QuerySingle(query, func, parameters));

        public async Task<TOut> QuerySingleAsync<TOut, TValue>(string query, Func<DbDataReader, Task<TOut>> func, params (string, TValue)[] parameters)
            => await UsingConnectionAsync(conn => conn.QuerySingle(query, func, parameters));

        public async Task<TOut> QuerySingleAsync<TOut, TValue>(string query, Func<DbDataReader, Task<TOut>> func, IEnumerable<(string, TValue)> parameters)
            => await UsingConnectionAsync(conn => conn.QuerySingle(query, func, parameters));

        public async Task<TOut> QuerySingleAsync<TOut, TValue>(string query, Func<DbDataReader, Task<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => await UsingConnectionAsync(conn => conn.QuerySingle(query, func, parameters));

        #endregion
    }
}
