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
    [TestCategory(nameof(MySqlTests))]
    public class MySqlTests
    {
        [TestMethod]
        public void TestConnect() {
            MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder
            {
                Database = "classicmodels",
                UserID = "John",
                Password = Resources.Password,
                Server = "localhost",
                Port = 3306,
            };

            using (var conn = new MySqlConnection(connectionString.GetConnectionString(true)))
            {
                conn.Open();
            }
        }

        [TestMethod]
        public void TestGenerator() {
            TestEnvironment.Connector.UsingTransaction(db => { });
        }

        [TestMethod]
        public void QueryList()
        {
            IReadOnlyList<Offices> test = TestEnvironment.Connector.QueryList("SELECT * FROM classicmodels.offices", ObjectMapper<Offices>.MapAll);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void StringDictionary()
        {
            IReadOnlyList<IReadOnlyDictionary<string, string>> test = TestEnvironment.Connector.QueryList("SELECT * FROM classicmodels.offices", Mapper.String);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void ObjectDictionary()
        {
            IReadOnlyList<IReadOnlyDictionary<string, object>> test = TestEnvironment.Connector.QueryList("SELECT * FROM classicmodels.offices", Mapper.Object);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public async Task ObjectDictionaryAsync() {
            IReadOnlyList<IReadOnlyDictionary<string, object>> test = await TestEnvironment.Connector.QueryListAsync("SELECT * FROM classicmodels.offices", Mapper.ObjectAsync);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void ObjectDynamic()
        {
            IReadOnlyList<dynamic> test = TestEnvironment.Connector.QueryList("SELECT * FROM classicmodels.offices", Mapper.Dynamic);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public async Task ObjectDynamicAsync()
        {
            IReadOnlyList<dynamic> test = await TestEnvironment.Connector.QueryListAsync("SELECT * FROM classicmodels.offices", Mapper.DynamicAsync);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public async Task AsyncQuery()
        {
            IReadOnlyList<Offices> test = await TestEnvironment.Connector.QueryListAsync("SELECT * FROM classicmodels.offices", ObjectMapper<Offices>.MapAllAsync);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void QuerySingleParams()
        {
            Offices office = TestEnvironment.Connector.QuerySingle("SELECT * FROM `classicmodels`.`offices` WHERE `OfficeCode` = @OfficeCode", ObjectMapper<Offices>.Map, new { OfficeCode = "1" });
            Assert.IsNotNull(office);
        }

        [TestMethod]
        public void QuerySingleIEnumerable()
        {
            var parameters = new Dictionary<string, string> { { "OfficeCode", "1" } };
            Offices office = TestEnvironment.Connector.QuerySingle("SELECT * FROM `classicmodels`.`offices` WHERE `OfficeCode` = @OfficeCode", ObjectMapper<Offices>.Map, parameters);
            Assert.IsNotNull(office);
        }

        [TestMethod]
        public void QuerySingleIEnumerable2()
        {
            var parameters = new Dictionary<string, string> { { "OfficeCode", "1" } };
            object office = TestEnvironment.Connector
                .QuerySingle("SELECT * FROM `classicmodels`.`offices` WHERE `OfficeCode` = @OfficeCode", ObjectMapper.Map(typeof(Offices)), parameters);
            Assert.IsNotNull(office as Offices);
        }

        [TestMethod]
        public void QueryComplexList()
        {
            IReadOnlyList<OfficesTricky> test = TestEnvironment.Connector.QueryList("SELECT * FROM classicmodels.offices", ObjectMapper<OfficesTricky>.MapAll);
            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void TestUpdate1()
        {
            TestEnvironment.Connector.UsingConnection(conn => conn.NonQuery(@"UPDATE `classicmodels`.`offices` SET `postalCode`= '94081' WHERE `officeCode`= @officeCode;", new { officeCode = "1" }));
        }

    }
}
