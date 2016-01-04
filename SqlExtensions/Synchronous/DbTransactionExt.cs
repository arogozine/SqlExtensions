using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace SqlExtensions
{
    public static class DbTransactionExt
    {
        #region Auto Commit

        public static void AutoCommit(this DbTransaction tran, Action<DbTransaction> func)
        {
            try
            {
                func(tran);
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public static TOut AutoCommit<TOut>(this DbTransaction tran, Func<DbTransaction, TOut> func)
        {
            try
            {
                return func(tran);
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        #endregion

        #region Using DbCommand

        public static IReadOnlyList<TOut> UsingCommand<TOut>(this DbTransaction tran, string query, Func<DbCommand, IReadOnlyList<TOut>> func, object parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.Transaction = tran;
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return func(dbCommand);
            }
        }

        public static IReadOnlyList<TOut> UsingCommand<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, IReadOnlyList<TOut>> func, params Tuple<string, TValue>[] parameters)
        {
            return UsingCommand(tran, query, func, (IEnumerable<Tuple<string, TValue>>)parameters);
        }

        public static IReadOnlyList<TOut> UsingCommand<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, IReadOnlyList<TOut>> func, IEnumerable<Tuple<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.Transaction = tran;
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return func(dbCommand);
            }
        }

        public static IReadOnlyList<TOut> UsingCommand<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, IReadOnlyList<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.Transaction = tran;
                dbCommand.AddParameters(parameters);
                dbCommand.CommandText = query;
                return func(dbCommand);
            }
        }

        #endregion

        #region Using DbCommand Action

        public static void UsingCommand(this DbTransaction tran, string query, Action<DbCommand> action, object parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;
                action(dbCommand);
            }
        }

        public static void UsingCommand<TValue>(this DbTransaction tran, string query, Action<DbCommand> action, params Tuple<string, TValue>[] parameters)
        {
            UsingCommand(tran, query, action, (IEnumerable<Tuple<string, TValue>>)parameters);
        }

        public static void UsingCommand<TValue>(this DbTransaction tran, string query, Action<DbCommand> action, IEnumerable<Tuple<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;
                action(dbCommand);
            }
        }

        public static void UsingCommand<TValue>(this DbTransaction tran, string query, Action<DbCommand> action, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;
                action(dbCommand);
            }
        }

        #endregion

        #region Using DbCommand Function

        public static TOut UsingCommand<TOut>(this DbTransaction tran, string query, Func<DbCommand, TOut> func, object parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return func(dbCommand);
            }
        }

        public static TOut UsingCommand<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, TOut> func, params Tuple<string, TValue>[] parameters)
        {
            return UsingCommand(tran, query, func, (IEnumerable<Tuple<string, TValue>>)parameters);
        }

        public static TOut UsingCommand<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, TOut> func, IEnumerable<Tuple<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return func(dbCommand);
            }
        }

        public static TOut UsingCommand<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, TOut> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return func(dbCommand);
            }
        }

        #endregion

        #region Query List DbCommand

        public static IReadOnlyList<TOut> QueryList<TOut>(this DbTransaction tran, string query, Func<DbCommand, IReadOnlyList<TOut>> func, object parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return func(dbCommand);
            }
        }

        public static IReadOnlyList<TOut> QueryList<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, IReadOnlyList<TOut>> func, params Tuple<string, TValue>[] parameters)
        {
            return UsingCommand(tran, query, func, (IEnumerable<Tuple<string, TValue>>)parameters);
        }

        public static IReadOnlyList<TOut> QueryList<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, IReadOnlyList<TOut>> func, IEnumerable<Tuple<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return func(dbCommand);
            }
        }

        public static IReadOnlyList<TOut> QueryList<TOut, TValue>(this DbTransaction tran, string query, Func<DbCommand, IReadOnlyList<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return func(dbCommand);
            }
        }

        #endregion

        #region Query List IDataReader

        public static IReadOnlyList<TOut> QueryList<TOut>(this DbTransaction tran, string query, Func<IDataReader, IReadOnlyList<TOut>> func, object parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return dbCommand.QueryList(func);
            }
        }

        public static IReadOnlyList<TOut> QueryList<TOut, TValue>(this DbTransaction tran, string query, Func<IDataReader, IReadOnlyList<TOut>> func, params Tuple<string, TValue>[] parameters)
        {
            return QueryList(tran, query, func, (IEnumerable<Tuple<string, TValue>>)parameters);
        }

        public static IReadOnlyList<TOut> QueryList<TOut, TValue>(this DbTransaction tran, string query, Func<IDataReader, IReadOnlyList<TOut>> func, IEnumerable<Tuple<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return dbCommand.QueryList(func);
            }
        }

        public static IReadOnlyList<TOut> QueryList<TOut, TValue>(this DbTransaction tran, string query, Func<IDataReader, IReadOnlyList<TOut>> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
        {
            using (var dbCommand = tran.Connection.CreateCommand())
            {
                dbCommand.AddParameters(parameters);

                dbCommand.CommandText = query;
                dbCommand.Transaction = tran;

                return dbCommand.QueryList(func);
            }
        }

        #endregion

        #region Query Single DbCommand

        public static TOut QuerySingle<TOut>(this DbTransaction conn, string query, Func<DbCommand, TOut> func, object parameters)
            => conn.UsingCommand(query, func, parameters);

        public static TOut QuerySingle<TOut, TValue>(this DbTransaction conn, string query, Func<DbCommand, TOut> func, params Tuple<string, TValue>[] parameters)
            => conn.UsingCommand(query, func, parameters);

        public static TOut QuerySingle<TOut, TValue>(this DbTransaction conn, string query, Func<DbCommand, TOut> func, IEnumerable<Tuple<string, TValue>> parameters)
            => conn.UsingCommand(query, func, parameters);

        public static TOut QuerySingle<TOut, TValue>(this DbTransaction conn, string query, Func<DbCommand, TOut> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => conn.UsingCommand(query, func, parameters);

        #endregion

        #region Query Single IDataReader

        public static TOut QuerySingle<TOut>(this DbTransaction conn, string query, Func<IDataReader, TOut> func, object parameters)
            => conn.UsingCommand(query, cmd => cmd.QuerySingle(func), parameters);

        public static TOut QuerySingle<TOut, TValue>(this DbTransaction conn, string query, Func<IDataReader, TOut> func, params Tuple<string, TValue>[] parameters)
            => conn.UsingCommand(query, cmd => cmd.QuerySingle(func), parameters);

        public static TOut QuerySingle<TOut, TValue>(this DbTransaction conn, string query, Func<IDataReader, TOut> func, IEnumerable<Tuple<string, TValue>> parameters)
            => conn.UsingCommand(query, cmd => cmd.QuerySingle(func), parameters);

        public static TOut QuerySingle<TOut, TValue>(this DbTransaction conn, string query, Func<IDataReader, TOut> func, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => conn.UsingCommand(query, cmd => cmd.QuerySingle(func), parameters);

        #endregion

        #region NonQuery

        public static int NonQuery(this DbTransaction tran, string sql, object parameters)
            => tran.UsingCommand(sql, cmd => cmd.ExecuteNonQuery(), parameters);

        public static int NonQuery<TValue>(this DbTransaction tran, string sql, params Tuple<string, TValue>[] parameters)
            => tran.UsingCommand(sql, cmd => cmd.ExecuteNonQuery(), parameters);

        public static int NonQuery<TValue>(this DbTransaction tran, string sql, IEnumerable<Tuple<string, TValue>> parameters)
            => tran.UsingCommand(sql, cmd => cmd.ExecuteNonQuery(), parameters);

        public static int NonQuery<TValue>(this DbTransaction tran, string sql, IEnumerable<KeyValuePair<string, TValue>> parameters)
            => tran.UsingCommand(sql, cmd => cmd.ExecuteNonQuery(), parameters);

        #endregion
    }
}
