using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    [TestCategory(nameof(QueryListTests))]
    public class QueryListTests
    {
        const string SimpleQuery = "SELECT * FROM classicmodels.offices";
        const string ParamQuery = "SELECT * FROM `classicmodels`.`offices` WHERE `city` = @City";
        readonly object AnonParam = new
        {
            City = "Boston"
        };
        readonly Dictionary<string, string> DictionaryParam = new Dictionary<string, string>
        {
            { "City", "Boston" }
        };
        readonly ValueTuple<string, string> TupleParam = ("City", "Boston");

        [TestMethod]
        public void ObjectMapper()
        {
            IReadOnlyList<Offices> test = TestEnvironment.Connector
                .QueryList(SimpleQuery, ObjectMapper<Offices>.MapAll);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void ObjectMapperParamObj()
        {
            IReadOnlyList<Offices> test = TestEnvironment.Connector
                .QueryList(ParamQuery, ObjectMapper<Offices>.MapAll, AnonParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public void ObjectMapperParamDictionary()
        {
            IReadOnlyList<Offices> test = TestEnvironment.Connector
                .QueryList(ParamQuery, ObjectMapper<Offices>.MapAll, DictionaryParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public void ObjectMapperParamTuple()
        {
            IReadOnlyList<Offices> test = TestEnvironment.Connector
                .QueryList(ParamQuery, ObjectMapper<Offices>.MapAll, TupleParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public async Task ObjectMapperAsync()
        {
            IReadOnlyList<Offices> test = await TestEnvironment.Connector
                .QueryListAsync(SimpleQuery, ObjectMapper<Offices>.MapAllAsync);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public async Task ObjectMapperAsyncParamObj()
        {
            IReadOnlyList<Offices> test = await TestEnvironment.Connector
                .QueryListAsync(ParamQuery, ObjectMapper<Offices>.MapAllAsync, AnonParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public async Task ObjectMapperAsyncParamDictionary()
        {
            IReadOnlyList<Offices> test = await TestEnvironment.Connector
                .QueryListAsync(ParamQuery, ObjectMapper<Offices>.MapAllAsync, DictionaryParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public async Task ObjectMapperAsyncParamTuple()
        {
            IReadOnlyList<Offices> test = await TestEnvironment.Connector
                .QueryListAsync(ParamQuery, ObjectMapper<Offices>.MapAllAsync, TupleParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public void ObjectDictionary()
        {
            IReadOnlyList<IReadOnlyDictionary<string, object>> test = TestEnvironment.Connector
                .QueryList(SimpleQuery, Mapper.Object);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void ObjectDictionaryParamObj()
        {
            IReadOnlyList<IReadOnlyDictionary<string, object>> test = TestEnvironment.Connector
                .QueryList(ParamQuery, Mapper.Object, AnonParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public void ObjectDictionaryParamDictionary()
        {
            IReadOnlyList<IReadOnlyDictionary<string, object>> test = TestEnvironment.Connector
                .QueryList(ParamQuery, Mapper.Object, DictionaryParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public void ObjectDictionaryParamTuple()
        {
            IReadOnlyList<IReadOnlyDictionary<string, object>> test = TestEnvironment.Connector
                .QueryList(ParamQuery, Mapper.Object, TupleParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public async Task ObjectDictionaryAsync()
        {
            IReadOnlyList<IReadOnlyDictionary<string, object>> test =await TestEnvironment.Connector
                .QueryListAsync(SimpleQuery, Mapper.ObjectAsync);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public async Task ObjectDictionaryAsyncParamObj()
        {
            IReadOnlyList<IReadOnlyDictionary<string, object>> test = await TestEnvironment.Connector
                .QueryListAsync(ParamQuery, Mapper.ObjectAsync, AnonParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public async Task ObjectDictionaryAsyncParamDictionary()
        {
            IReadOnlyList<IReadOnlyDictionary<string, object>> test = await TestEnvironment.Connector
                .QueryListAsync(ParamQuery, Mapper.ObjectAsync, DictionaryParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public async Task ObjectDictionaryAsyncParamTuple()
        {
            IReadOnlyList<IReadOnlyDictionary<string, object>> test = await TestEnvironment.Connector
                .QueryListAsync(ParamQuery, Mapper.ObjectAsync, TupleParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public void ObjectDynamic()
        {
            IReadOnlyList<dynamic> test = TestEnvironment.Connector
                .QueryList(SimpleQuery, Mapper.Dynamic);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void ObjectDynamicParamObj()
        {
            IReadOnlyList<dynamic> test = TestEnvironment.Connector
                .QueryList(ParamQuery, Mapper.Dynamic, AnonParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public void ObjectDynamicParamDictionary()
        {
            IReadOnlyList<dynamic> test = TestEnvironment.Connector
                .QueryList(ParamQuery, Mapper.Dynamic, DictionaryParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public void ObjectDynamicParamTuple()
        {
            IReadOnlyList<dynamic> test = TestEnvironment.Connector
                .QueryList(ParamQuery, Mapper.Dynamic, TupleParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public async Task ObjectDynamicAsync()
        {
            IReadOnlyList<dynamic> test = await TestEnvironment.Connector
                .QueryListAsync(SimpleQuery, Mapper.DynamicAsync);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public async Task ObjectDynamicAsyncParamObj()
        {
            IReadOnlyList<dynamic> test = await TestEnvironment.Connector
                .QueryListAsync(ParamQuery, Mapper.DynamicAsync, AnonParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public async Task ObjectDynamicAsyncParamDictionary()
        {
            IReadOnlyList<dynamic> test = await TestEnvironment.Connector
                .QueryListAsync(ParamQuery, Mapper.DynamicAsync, DictionaryParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public async Task ObjectDynamicAsyncParamTuple()
        {
            IReadOnlyList<dynamic> test = await TestEnvironment.Connector
                .QueryListAsync(ParamQuery, Mapper.DynamicAsync, TupleParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public void StringDictionary()
        {
            IReadOnlyList<IReadOnlyDictionary<string, string>> test = TestEnvironment.Connector
                .QueryList(SimpleQuery, Mapper.String);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public void StringDictionaryParamObj()
        {
            IReadOnlyList<IReadOnlyDictionary<string, string>> test = TestEnvironment.Connector
                .QueryList(ParamQuery, Mapper.String, AnonParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public void StringDictionaryParamDictionary()
        {
            IReadOnlyList<IReadOnlyDictionary<string, string>> test = TestEnvironment.Connector
                .QueryList(ParamQuery, Mapper.String, DictionaryParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public void StringDictionaryParamTuple()
        {
            IReadOnlyList<IReadOnlyDictionary<string, string>> test = TestEnvironment.Connector
                .QueryList(ParamQuery, Mapper.String, TupleParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public async Task StringDictionaryAsync()
        {
            IReadOnlyList<IReadOnlyDictionary<string, string>> test = await TestEnvironment.Connector
                .QueryListAsync(SimpleQuery, Mapper.StringAsync);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count > 1);
        }

        [TestMethod]
        public async Task StringDictionaryAsyncParamObj()
        {
            IReadOnlyList<IReadOnlyDictionary<string, string>> test = await TestEnvironment.Connector
                .QueryListAsync(ParamQuery, Mapper.StringAsync, AnonParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public async Task StringDictionaryAsyncParamDictionary()
        {
            IReadOnlyList<IReadOnlyDictionary<string, string>> test = await TestEnvironment.Connector
                .QueryListAsync(ParamQuery, Mapper.StringAsync, DictionaryParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }

        [TestMethod]
        public async Task StringDictionaryAsyncParamTuple()
        {
            IReadOnlyList<IReadOnlyDictionary<string, string>> test = await TestEnvironment.Connector
                .QueryListAsync(ParamQuery, Mapper.StringAsync, TupleParam);

            Assert.IsNotNull(test);
            Assert.IsTrue(test.Count == 1);
        }
    }
}
