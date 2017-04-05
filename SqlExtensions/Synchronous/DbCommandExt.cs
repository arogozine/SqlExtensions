using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace SqlExtensions
{
    public static class DbCommandExt
    {
        public static IReadOnlyList<TOut> QueryList<TOut>(this DbCommand cmd, Func<DbDataReader, IReadOnlyList<TOut>> func)
        {
            using (var reader = cmd.ExecuteReader())
            {
                return func(reader);
            }
        }

        public static IReadOnlyList<TOut> QueryList<TOut>(this DbCommand cmd, Func<IDataRecord, TOut> func)
        {
            using (var reader = cmd.ExecuteReader())
            {
                return reader.QueryList(func);
            }
        }

        public static TOut QuerySingle<TOut>(this DbCommand cmd, Func<DbDataReader, TOut> func)
        {
            using (var reader = cmd.ExecuteReader())
            {
                return reader.Read() ? func(reader) : default(TOut);
            }
        }

        public static TOut QuerySingle<TOut>(this DbCommand cmd, Func<IDataRecord, TOut> func)
        {
            using (var reader = cmd.ExecuteReader())
            {
                return reader.Read() ? func(reader) : default(TOut);
            }
        }

        public static void UsingReader(this DbCommand cmd, Action<DbDataReader> action)
        {
            using (var reader = cmd.ExecuteReader())
            {
                action(reader);
            }
        }

        public static void AddParameterWithValue(this DbCommand command, string parameterName, object parameterValue)
        {
            // Copied from,
            // https://social.msdn.microsoft.com/Forums/en-US/d56a4710-3fd1-4039-a0d9-c4c6bd1cd22e/dbcommand-parameters-collection-missing-addwithvalue-method?forum=adodotnetentityframework

            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            command.Parameters.Add(parameter);
        }

        public static void AddParameters(this DbCommand command, object parameters)
        {
            Action<DbCommand, object> mapParameters = ParamMapper.GenerateParameterMap(parameters);
            mapParameters(command, parameters);
        }

        public static void AddParameters<T>(this DbCommand command, IEnumerable<KeyValuePair<string, T>> parameters)
        {
            if (parameters == null)
                return;

            foreach (var keyValue in parameters)
            {
                command.AddParameterWithValue(keyValue.Key, keyValue.Value);
            }
        }

        public static void AddParameters<T>(this DbCommand command, IEnumerable<(string name, T value)> parameters)
        {
            if (parameters == null)
                return;

            foreach ((var name, T value) in parameters)
            {
                command.AddParameterWithValue(name, value);
            }
        }

        public static void AddParameters<T>(this DbCommand command, params (string name, T value)[] parameters)
        {
            if (parameters == null)
                return;

            foreach ((var name, T value) in parameters)
            {
                command.AddParameterWithValue(name, value);
            }
        }
    }
}
