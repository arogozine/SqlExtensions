﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using UnitTests.Properties;

namespace UnitTests
{
    [TestClass]
    [TestCategory(nameof(MySqlQuerySingle))]
    public class MySqlQuerySingle
    {
        [TestMethod]
        public void MapperStringSingle()
        {
            IReadOnlyDictionary<string, string> test = TestEnvironment.Connector
                .QuerySingle("SELECT * FROM classicmodels.offices LIMIT 1", Mapper.StringSingle);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void MapperStringSingle_AnonymousParameters()
        {
            IReadOnlyDictionary<string, string> test = TestEnvironment.Connector
                .QuerySingle("SELECT * FROM classicmodels.offices WHERE territory = @territory LIMIT 1", Mapper.StringSingle, new { territory = "NA" });

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void MapperStringSingle_TupleParameters()
        {
            IReadOnlyDictionary<string, string> test = TestEnvironment.Connector
                .QuerySingle("SELECT * FROM classicmodels.offices WHERE territory = @territory", Mapper.StringSingle, ("territory", "NA"));

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void MapperStringSingle_DictionaryParameters()
        {
            var dictParam = new Dictionary<string, string> {
                {
                    "territory", "NA"
                }
            };

            IReadOnlyDictionary<string, string> test = TestEnvironment.Connector
                .QuerySingle("SELECT * FROM classicmodels.offices WHERE territory = @territory", Mapper.StringSingle, dictParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void MapperObjectSingle()
        {
            IReadOnlyDictionary<string, object> test = TestEnvironment.Connector
                .QuerySingle("SELECT * FROM classicmodels.offices", Mapper.ObjectSingle);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void MapperDynamicSingle()
        {
            dynamic test = TestEnvironment.Connector
                .QuerySingle("SELECT * FROM classicmodels.offices", Mapper.DynamicSingle);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void OfficeMapperSingle()
        {
            Offices office = TestEnvironment.Connector.QuerySingle("SELECT * FROM classicmodels.offices", ObjectMapper<Offices>.Map);

            Assert.IsNotNull(office);
        }
    }
}
