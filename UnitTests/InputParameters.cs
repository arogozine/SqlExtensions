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
    [TestCategory(nameof(InputParameters))]
    public class InputParameters
    {
        MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder
        {
            Database = "classicmodels",
            UserID = "John",
            Password = Resources.Password,
            Server = "localhost",
            Port = 3306,
        };

        [TestMethod]
        public void SavesParameterTupleParams()
        {
            dynamic test = TestEnvironment.Connector.QuerySingle("SELECT @PassedInParam AS 'PassedInParam'", Mapper.DynamicSingle, ("PassedInParam", "Foo"));

            Assert.IsNotNull(test);
            Assert.AreEqual<string>(test.PassedInParam, "Foo");
        }

        [TestMethod]
        public void SavesParameterTupleIEnumerable()
        {
            dynamic test = TestEnvironment.Connector.QuerySingle("SELECT @PassedInParam AS 'PassedInParam'", Mapper.DynamicSingle,
                new List<(string, string)> { ("PassedInParam", "Foo") });

            Assert.IsNotNull(test);
            Assert.AreEqual<string>(test.PassedInParam, "Foo");
        }

        [TestMethod]
        public void SavesParameterDictionary()
        {
            dynamic test = TestEnvironment.Connector.QuerySingle("SELECT @PassedInParam AS 'PassedInParam'", Mapper.DynamicSingle,
                new Dictionary<string, string> {
                    {  "PassedInParam", "Foo" }
                });

            Assert.IsNotNull(test);
            Assert.AreEqual<string>(test.PassedInParam, "Foo");
        }

        [TestMethod]
        public void SavesParameterAnonymousObject()
        {
            dynamic test = TestEnvironment.Connector.QuerySingle("SELECT @PassedInParam AS 'PassedInParam'", Mapper.DynamicSingle,
                new { PassedInParam = "Foo" });

            Assert.IsNotNull(test);
            Assert.AreEqual<string>(test.PassedInParam, "Foo");
        }

        public class PassedInParamObj
        {
            public string PassedInParam { get; set; }
        }

        [TestMethod]
        public void FooBar() {
            var test = TestEnvironment.Connector.QuerySingle("SELECT @PassedInParam AS 'PassedInParam'", ObjectMapper<PassedInParamObj>.Map, ("PassedInParam", "Foo"));

            Assert.IsNotNull(test);
            Assert.AreEqual(test.PassedInParam, "Foo");
        }
    }
}
