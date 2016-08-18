using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using SqlExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Properties;

namespace UnitTests
{
    public class OfficesTricky
    {
        public string OfficeCode { set { /* no op*/ } }
        public string City { get {
                return "FooBar";
            } }

        private string Phone { get; set; }
        internal string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Territory { get; set; }
    }


    [TestClass]
    public class MySqlTests
    {
        MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder
        {
            Database = "classicmodels",
            UserID = "John",
            Password = Settings.Default.Password,
            Server = "localhost",
            Port = 3306,
        };

        [TestMethod]
        public void TestConnect() {
            using (var conn = new MySqlConnection(connectionString.GetConnectionString(true)))
            {
                conn.Open();
            }
        }

        [TestMethod]
        public void TestGenerator() {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            connector.UsingTransaction(db => { });
        }

        [TestMethod]
        public void QueryList()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            IReadOnlyList<Offices> test = connector.QueryList("SELECT * FROM classicmodels.offices", ObjectMapper<Offices>.MapAll);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void StringDictionary()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            IReadOnlyList<IReadOnlyDictionary<string, string>> test = connector.QueryList("SELECT * FROM classicmodels.offices", Mapper.String);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void ObjectDictionary()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            IReadOnlyList<IReadOnlyDictionary<string, object>> test = connector.QueryList("SELECT * FROM classicmodels.offices", Mapper.Object);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public async Task ObjectDictionaryAsync() {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            IReadOnlyList<IReadOnlyDictionary<string, object>> test = await connector.QueryListAsync("SELECT * FROM classicmodels.offices", Mapper.ObjectAsync);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void ObjectDynamic()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            IReadOnlyList<dynamic> test = connector.QueryList("SELECT * FROM classicmodels.offices", Mapper.Dynamic);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public async Task ObjectDynamicAsync()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            IReadOnlyList<dynamic> test = await connector.QueryListAsync("SELECT * FROM classicmodels.offices", Mapper.DynamicAsync);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public async Task AsyncQuery()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            IReadOnlyList<Offices> test = await connector.QueryListAsync("SELECT * FROM classicmodels.offices", ObjectMapper<Offices>.MapAllAsync);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void QuerySingleParams()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            Offices office = connector.QuerySingle("SELECT * FROM `classicmodels`.`offices` WHERE `OfficeCode` = @OfficeCode", ObjectMapper<Offices>.Map, new { OfficeCode = "1" });
            Assert.IsNotNull(office);
        }

        [TestMethod]
        public void QuerySingleIEnumerable()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            var parameters = new Dictionary<string, string> { { "OfficeCode", "1" } };
            Offices office = connector.QuerySingle("SELECT * FROM `classicmodels`.`offices` WHERE `OfficeCode` = @OfficeCode", ObjectMapper<Offices>.Map, parameters);
            Assert.IsNotNull(office);
        }

        [TestMethod]
        public void QuerySingleIEnumerable2()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            var parameters = new Dictionary<string, string> { { "OfficeCode", "1" } };
            object office = connector
                .QuerySingle("SELECT * FROM `classicmodels`.`offices` WHERE `OfficeCode` = @OfficeCode", ObjectMapper.Map(typeof(Offices)), parameters);
            Assert.IsNotNull(office as Offices);
        }

        [TestMethod]
        public void QueryComplexList()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            IReadOnlyList<OfficesTricky> test = connector.QueryList("SELECT * FROM classicmodels.offices", ObjectMapper<OfficesTricky>.MapAll);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void TestUpdate1()
        {
            var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));
            connector.UsingConnection(conn => conn.NonQuery(@"UPDATE `classicmodels`.`offices` SET `postalCode`= '94081' WHERE `officeCode`= @officeCode;", new { officeCode = "1" }));
        }

    }
}
