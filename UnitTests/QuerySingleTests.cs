using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    [TestCategory(nameof(QuerySingleTests))]
    public class QuerySingleTests
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
            Offices test = TestEnvironment.Connector
                .QuerySingle(SimpleQuery, ObjectMapper<Offices>.Map);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void ObjectMapperParamObj()
        {
            Offices test = TestEnvironment.Connector
                .QuerySingle(ParamQuery, ObjectMapper<Offices>.Map, AnonParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void ObjectMapperParamDictionary()
        {
            Offices test = TestEnvironment.Connector
                .QuerySingle(ParamQuery, ObjectMapper<Offices>.Map, DictionaryParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void ObjectMapperParamTuple()
        {
            Offices test = TestEnvironment.Connector
                .QuerySingle(ParamQuery, ObjectMapper<Offices>.Map, TupleParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task ObjectMapperAsync()
        {
            Offices test = await TestEnvironment.Connector
                .QuerySingleAsync(SimpleQuery, async x => ObjectMapper<Offices>.Map(x));

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task ObjectMapperAsyncParamObj()
        {
            Offices test = await TestEnvironment.Connector
                .QuerySingleAsync(ParamQuery, async x => ObjectMapper<Offices>.Map(x), AnonParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task ObjectMapperAsyncParamDictionary()
        {
            Offices test = await TestEnvironment.Connector
                .QuerySingleAsync(ParamQuery, async x => ObjectMapper<Offices>.Map(x), DictionaryParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task ObjectMapperAsyncParamTuple()
        {
            Offices test = await TestEnvironment.Connector
                .QuerySingleAsync(ParamQuery, async x => ObjectMapper<Offices>.Map(x), TupleParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void ObjectDictionary()
        {
            IReadOnlyDictionary<string, object> test = TestEnvironment.Connector
                .QuerySingle(SimpleQuery, Mapper.ObjectSingle);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void ObjectDictionaryParamObj()
        {
            IReadOnlyDictionary<string, object> test = TestEnvironment.Connector
                .QuerySingle(ParamQuery, Mapper.ObjectSingle, AnonParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void ObjectDictionaryParamDictionary()
        {
            IReadOnlyDictionary<string, object> test = TestEnvironment.Connector
                .QuerySingle(ParamQuery, Mapper.ObjectSingle, DictionaryParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void ObjectDictionaryParamTuple()
        {
            IReadOnlyDictionary<string, object> test = TestEnvironment.Connector
                .QuerySingle(ParamQuery, Mapper.ObjectSingle, TupleParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task ObjectDictionaryAsync()
        {
            IReadOnlyDictionary<string, object> test = await TestEnvironment.Connector
                .QuerySingleAsync(SimpleQuery, async x => Mapper.ObjectSingle(x));

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task ObjectDictionaryAsyncParamObj()
        {
            IReadOnlyDictionary<string, object> test = await TestEnvironment.Connector
                .QuerySingleAsync(ParamQuery, async x => Mapper.ObjectSingle(x), AnonParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task ObjectDictionaryAsyncParamDictionary()
        {
            IReadOnlyDictionary<string, object> test = await TestEnvironment.Connector
                .QuerySingleAsync(ParamQuery, async x => Mapper.ObjectSingle(x), DictionaryParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task ObjectDictionaryAsyncParamTuple()
        {
            IReadOnlyDictionary<string, object> test = await TestEnvironment.Connector
                .QuerySingleAsync(ParamQuery, async x => Mapper.ObjectSingle(x), TupleParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void ObjectDynamic()
        {
            dynamic test = TestEnvironment.Connector
                .QuerySingle(SimpleQuery, Mapper.Dynamic);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void ObjectDynamicParamObj()
        {
            dynamic test = TestEnvironment.Connector
                .QuerySingle(ParamQuery, Mapper.Dynamic, AnonParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void ObjectDynamicParamDictionary()
        {
            dynamic test = TestEnvironment.Connector
                .QuerySingle(ParamQuery, Mapper.Dynamic, DictionaryParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void ObjectDynamicParamTuple()
        {
            dynamic test = TestEnvironment.Connector
                .QuerySingle(ParamQuery, Mapper.Dynamic, TupleParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task ObjectDynamicAsync()
        {
            dynamic test = await TestEnvironment.Connector
                .QuerySingleAsync(SimpleQuery, async x => Mapper.DynamicSingle(x));

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task ObjectDynamicAsyncParamObj()
        {
            dynamic test = await TestEnvironment.Connector
                .QuerySingleAsync(ParamQuery, async x => Mapper.DynamicSingle(x), AnonParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task ObjectDynamicAsyncParamDictionary()
        {
            dynamic test = await TestEnvironment.Connector
                .QuerySingleAsync(ParamQuery, async x => Mapper.DynamicSingle(x), DictionaryParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task ObjectDynamicAsyncParamTuple()
        {
            dynamic test = await TestEnvironment.Connector
                .QuerySingleAsync(ParamQuery, async x => Mapper.DynamicSingle(x), TupleParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void StringDictionary()
        {
            IReadOnlyDictionary<string, string> test = TestEnvironment.Connector
                .QuerySingle(SimpleQuery, Mapper.StringSingle);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void StringDictionaryParamObj()
        {
            IReadOnlyDictionary<string, string> test = TestEnvironment.Connector
                .QuerySingle(ParamQuery, Mapper.StringSingle, AnonParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void StringDictionaryParamDictionary()
        {
            IReadOnlyDictionary<string, string> test = TestEnvironment.Connector
                .QuerySingle(ParamQuery, Mapper.StringSingle, DictionaryParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void StringDictionaryParamTuple()
        {
            IReadOnlyDictionary<string, string> test = TestEnvironment.Connector
                .QuerySingle(ParamQuery, Mapper.StringSingle, TupleParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task StringDictionaryAsync()
        {
            IReadOnlyDictionary<string, string> test = await TestEnvironment.Connector
                .QuerySingleAsync(SimpleQuery, async x => Mapper.StringSingle(x));

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task StringDictionaryAsyncParamObj()
        {
            IReadOnlyDictionary<string, string> test = await TestEnvironment.Connector
                .QuerySingleAsync(ParamQuery, async x => Mapper.StringSingle(x), AnonParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task StringDictionaryAsyncParamDictionary()
        {
            IReadOnlyDictionary<string, string> test = await TestEnvironment.Connector
                .QuerySingleAsync(ParamQuery, async x => Mapper.StringSingle(x), DictionaryParam);

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public async Task StringDictionaryAsyncParamTuple()
        {
            IReadOnlyDictionary<string, string> test = await TestEnvironment.Connector
                .QuerySingleAsync(ParamQuery, async x => Mapper.StringSingle(x), TupleParam);

            Assert.IsNotNull(test);
        }

    }
}
