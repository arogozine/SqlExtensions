using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using SqlExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTests.Properties;

namespace UnitTests
{
    [TestClass]
    public class TestEnvironment
    {
        public static SqlConnector Connector;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context) {

            var connectionString = new MySqlConnectionStringBuilder
            {
                Database = "classicmodels",
                UserID = "John",
                Password = Resources.Password,
                Server = "localhost",
                Port = 3306,
            };

            Connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
        }
    }
}
