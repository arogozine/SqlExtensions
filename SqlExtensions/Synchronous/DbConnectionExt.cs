using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace SqlExtensions
{
    public static class DbConnectionExt
    {
        #region Commit Transaction

        public static void CommitTransaction(this DbConnection conn, Action<DbTransaction> action)
            => conn.UsingTransaction(tran => tran.AutoCommit(action));

        public static void CommitTransaction(this DbConnection conn, Action<DbTransaction> action, IsolationLevel isolationLevel)
            => conn.UsingTransaction(tran => tran.AutoCommit(action), isolationLevel);   

        public static T CommitTransaction<T>(this DbConnection conn, Func<DbTransaction, T> func)
            => conn.UsingTransaction(tran => tran.AutoCommit(func));

        public static T CommitTransaction<T>(this DbConnection conn, Func<DbTransaction, T> func, IsolationLevel isolationLevel)
            => conn.UsingTransaction(tran => tran.AutoCommit(func), isolationLevel);

        #endregion

        #region Using Transaction

        public static void UsingTransaction(this DbConnection conn, Action<DbTransaction> action, IsolationLevel isolationLevel)
        {
            using (var tran = conn.BeginTransaction(isolationLevel))
            {
                action(tran);
            }
        }

        public static void UsingTransaction(this DbConnection conn, Action<DbTransaction> action)
        {
            using (var tran = conn.BeginTransaction())
            {
                action(tran);
            }
        }

        public static T UsingTransaction<T>(this DbConnection conn, Func<DbTransaction, T> func)
        {
            using (var tran = conn.BeginTransaction())
            {
                return func(tran);
            }
        }

        public static T UsingTransaction<T>(this DbConnection conn, Func<DbTransaction, T> func, IsolationLevel isolationLevel)
        {
            using (var tran = conn.BeginTransaction(isolationLevel))
            {
                return func(tran);
            }
        }

        #endregion

        #region Using DbCommand

        public static IReadOnlyList<TOut> UsingCommand<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, IReadOnlyList<TOut>> func, params Tuple<string, TValue>[] parameters)
        {
            return UsingCommand(conn, query, func, (IEnumerable<Tuple<string, TValue>>)parameters);
        }

        public static IReadOnlyList<TOut> UsingCommand<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, IReadOnlyList<TOut>> func, IEnumerable<Tuple<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return func(dbCommand);
            }
        }

        public static IReadOnlyList<TOut> UsingCommand<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, IReadOnlyList<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return func(dbCommand);
            }
        }

        public static IReadOnlyList<TOut> UsingCommand<TOut>(this DbConnection conn, string query, Func<DbCommand, IReadOnlyList<TOut>> func, object parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return func(dbCommand);
            }
        }

        public static TOut UsingCommand<TOut>(this DbConnection conn, string query, Func<DbCommand, TOut> func)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.CommandText = query;
                return func(dbCommand);
            }
        }

        #endregion

        #region Using DbCommand Action

        public static void UsingCommand<TValue>(this DbConnection conn, string query, Action<DbCommand> action, params Tuple<string, TValue>[] parameters)
        {
            UsingCommand(conn, query, action, (IEnumerable<Tuple<string, TValue>>)parameters);
        }

        public static void UsingCommand<TValue>(this DbConnection conn, string query, Action<DbCommand> action, IEnumerable<Tuple<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                action(dbCommand);
            }
        }

        public static void UsingCommand<TValue>(this DbConnection conn, string query, Action<DbCommand> action, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                action(dbCommand);
            }
        }

        public static void UsingCommand(this DbConnection conn, string query, Action<DbCommand> action, object parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                action(dbCommand);
            }
        }

        #endregion

        #region Using DbCommand Function

        public static TOut UsingCommand<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, TOut> func, params Tuple<string, TValue>[] parameters)
        {
            return UsingCommand(conn, query, func, (IEnumerable<Tuple<string, TValue>>)parameters);
        }

        public static TOut UsingCommand<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, TOut> func, IEnumerable<Tuple<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return func(dbCommand);
            }
        }

        public static TOut UsingCommand<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, TOut> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return func(dbCommand);
            }
        }

        public static TOut UsingCommand<TOut>(this DbConnection conn, string query, Func<DbCommand, TOut> func, object parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return func(dbCommand);
            }
        }

        #endregion

        #region Query List DbCommand

        public static IReadOnlyList<TOut> QueryList<TOut>(this DbConnection conn, string query, Func<DbCommand, IReadOnlyList<TOut>> func)
            => conn.UsingCommand(query, func);

        public static IReadOnlyList<TOut> QueryList<TOut>(this DbConnection conn, string query, Func<DbCommand, IReadOnlyList<TOut>> func, object parameters)
            => conn.UsingCommand(query, func);

        public static IReadOnlyList<TOut> QueryList<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, IReadOnlyList<TOut>> func, params Tuple<string, TValue>[] parameters)
            => conn.UsingCommand(query, func, parameters);

        public static IReadOnlyList<TOut> QueryList<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, IReadOnlyList<TOut>> func, IEnumerable<Tuple<string, TValue>> parameters)
            => conn.UsingCommand(query, func, parameters);

        public static IReadOnlyList<TOut> QueryList<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, IReadOnlyList<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => conn.UsingCommand(query, func, parameters);

        #endregion

        #region Query List DbDataReader

        public static IReadOnlyList<TOut> QueryList<TOut>(this DbConnection conn, string query, Func<DbDataReader, IReadOnlyList<TOut>> func, object parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return dbCommand.QueryList(func);
            }
        }

        public static IReadOnlyList<TOut> QueryList<TOut>(this DbConnection conn, string query, Func<DbDataReader, IReadOnlyList<TOut>> func)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.CommandText = query;
                return dbCommand.QueryList(func);
            }
        }

        public static IReadOnlyList<TOut> QueryList<TOut, TValue>(this DbConnection conn, string query, Func<DbDataReader, IReadOnlyList<TOut>> func, params Tuple<string, TValue>[] parameters)
        {
            return QueryList(conn, query, func, (IEnumerable<Tuple<string, TValue>>)parameters);
        }

        public static IReadOnlyList<TOut> QueryList<TOut, TValue>(this DbConnection conn, string query, Func<DbDataReader, IReadOnlyList<TOut>> func, IEnumerable<Tuple<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return dbCommand.QueryList(func);//.UsingReader(func);
            }
        }

        public static IReadOnlyList<TOut> QueryList<TOut, TValue>(this DbConnection conn, string query, Func<DbDataReader, IReadOnlyList<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = conn.CreateCommand())
            {
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return dbCommand.QueryList(func);
            }
        }

        #endregion

        #region Query Single DbCommand

        public static TOut QuerySingle<TOut>(this DbConnection conn, string query, Func<DbCommand, TOut> func)
            => conn.UsingCommand(query, func);

        public static TOut QuerySingle<TOut>(this DbConnection conn, string query, Func<DbCommand, TOut> func, object parameters)
            => conn.UsingCommand(query, func, parameters);

        public static TOut QuerySingle<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, TOut> func, params Tuple<string, TValue>[] parameters)
            => conn.UsingCommand(query, func, parameters);

        public static TOut QuerySingle<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, TOut> func, IEnumerable<Tuple<string, TValue>> parameters)
            => conn.UsingCommand(query, func, parameters);

        public static TOut QuerySingle<TOut, TValue>(this DbConnection conn, string query, Func<DbCommand, TOut> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => conn.UsingCommand(query, func, parameters);

        #endregion

        #region Query Single DbDataReader

        public static TOut QuerySingle<TOut>(this DbConnection conn, string query, Func<DbDataReader, TOut> func)
            => conn.UsingCommand(query, cmd => cmd.QuerySingle(func));

        public static TOut QuerySingle<TOut>(this DbConnection conn, string query, Func<DbDataReader, TOut> func, object parameters)
            => conn.UsingCommand(query, cmd => cmd.QuerySingle(func), parameters);

        public static TOut QuerySingle<TOut, TValue>(this DbConnection conn, string query, Func<DbDataReader, TOut> func, params Tuple<string, TValue>[] parameters)
            => conn.UsingCommand(query, cmd => cmd.QuerySingle(func), parameters);

        public static TOut QuerySingle<TOut, TValue>(this DbConnection conn, string query, Func<DbDataReader, TOut> func, IEnumerable<Tuple<string, TValue>> parameters)
            => conn.UsingCommand(query, cmd => cmd.QuerySingle(func), parameters);

        public static TOut QuerySingle<TOut, TValue>(this DbConnection conn, string query, Func<DbDataReader, TOut> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => conn.UsingCommand(query, cmd => cmd.QuerySingle(func), parameters);

        #endregion

        #region NonQuery

        public static int NonQuery(this DbConnection conn, string sql, object parameters)
            => conn.UsingCommand(sql, cmd => cmd.ExecuteNonQuery(), parameters);

        public static int NonQuery<TValue>(this DbConnection conn, string sql, params Tuple<string, TValue>[] parameters)
            => conn.UsingCommand(sql, cmd => cmd.ExecuteNonQuery(), parameters);

        public static int NonQuery<TValue>(this DbConnection conn, string sql, IEnumerable<Tuple<string, TValue>> parameters)
            => conn.UsingCommand(sql, cmd => cmd.ExecuteNonQuery(), parameters);

        public static int NonQuery<TValue>(this DbConnection conn, string sql, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => conn.UsingCommand(sql, cmd => cmd.ExecuteNonQuery(), parameters);

        #endregion
    }
}
