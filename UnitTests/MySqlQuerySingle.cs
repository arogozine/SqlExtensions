using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using SqlExtensions;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class MySqlQuerySingle
    {
        MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder
        {
            Database = "classicmodels",
            UserID = "John",
            Password = "TODO", //TODO
            Server = "localhost",
            Port = 3306,
        };


        [TestMethod]
        public void MapperStringSingle()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));

            IReadOnlyDictionary<string, string> test = connector
                .QuerySingle("SELECT * FROM classicmodels.offices", Mapper.StringSingle);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void MapperStringSingle_AnonymousParameters()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));

            IReadOnlyDictionary<string, string> test = connector
                .QuerySingle("SELECT * FROM classicmodels.offices WHERE territory = @territory", Mapper.StringSingle, new { territory = "NA" });

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void MapperStringSingle_TupleParameters()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));

            IReadOnlyDictionary<string, string> test = connector
                .QuerySingle("SELECT * FROM classicmodels.offices WHERE territory = @territory", Mapper.StringSingle, Tuple.Create("territory", "NA" ));

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void MapperStringSingle_DictionaryParameters()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));

            var dictParam = new Dictionary<string, string> {
                {
                    "territory", "NA"
                }
            };

            IReadOnlyDictionary<string, string> test = connector
                .QuerySingle("SELECT * FROM classicmodels.offices WHERE territory = @territory", Mapper.StringSingle, dictParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void MapperObjectSingle()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));

            IReadOnlyDictionary<string, object> test = connector
                .QuerySingle("SELECT * FROM classicmodels.offices", Mapper.ObjectSingle);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void MapperDynamicSingle()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));

            dynamic test = connector
                .QuerySingle("SELECT * FROM classicmodels.offices", Mapper.DynamicSingle);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void OfficeMapperSingle()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));

            Offices office = connector.QuerySingle("SELECT * FROM classicmodels.offices", ObjectMapper<Offices>.Map);

            Assert.IsNotNull(office);
        }
    }
}
