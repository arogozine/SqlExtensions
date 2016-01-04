﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class ConverterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ConversionNotSupportedException))]
        public void EnumToClassError()
        {
            ConverterTests t = TypeConverter.Convert<Animal, ConverterTests>(new Animal());
        }

        class ClassWithParse
        {
            public static ClassWithParse Parse(string value)
            {
                //
                return new ClassWithParse();
            } 
        }

        [TestMethod]
        public void TestStaticParseMethod()
        {
            ClassWithParse foo = TypeConverter.Convert<string, ClassWithParse>("someValidValue");
            Assert.IsNotNull(foo);
        }

        interface IAnimal { }

        class Animal : IAnimal { }

        class Doge : Animal { }

        [TestMethod]
        public void ConvertClassToInterface()
        {
            Animal animalObject = new Animal();
            IAnimal animalInterface = TypeConverter.Convert<Animal, IAnimal>(animalObject);
            Assert.IsNotNull(animalInterface);
            Assert.AreEqual(animalObject, animalInterface);
        }

        [TestMethod]
        public void ConvertInterfaceToClass()
        {
            IAnimal animalInterface = new Animal();
            Animal animalObject = TypeConverter.Convert<IAnimal, Animal>(animalInterface);
            Assert.IsNotNull(animalObject);
            Assert.AreEqual(animalObject, animalInterface);
        }

        [TestMethod]
        public void ConvertDateTimeToTimeSpan()
        {
            DateTime currentDateTime = DateTime.Now;
            TimeSpan currentTimeSpan = TypeConverter.Convert<DateTime, TimeSpan>(currentDateTime);
            Assert.IsTrue(currentDateTime.TimeOfDay == currentTimeSpan);
        }

        [TestMethod]
        public void ConvertTimeSpanToDateTime()
        {
            TimeSpan currentTimeSpan = DateTime.Now.TimeOfDay;
            DateTime currentDateTime = TypeConverter.Convert<TimeSpan, DateTime>(currentTimeSpan);
            Assert.IsTrue(currentDateTime.TimeOfDay == currentTimeSpan);
        }

        [TestMethod]
        public void ConvertBoolToBool()
        {
            bool falseBool = TypeConverter.Convert<bool, bool>(false);
            Assert.IsFalse(falseBool);
        }

        [TestMethod]
        public void ConvertBoolToBool_NonGeneric()
        {
            object falseBool = TypeConverter.Convert(typeof(bool), typeof(bool), false);
            Assert.IsTrue(falseBool is bool);
            Assert.IsFalse((bool)falseBool);
        }

        [TestMethod]
        public void ConvertEnumStructToEnumClass()
        {
            Enum e = TypeConverter.Convert<TypeCode, Enum>(TypeCode.Byte);
            Assert.IsTrue(e is TypeCode);
            Assert.IsTrue((TypeCode)e == TypeCode.Byte);
        }

        [TestMethod]
        public void ConvertEnumStructToEnumClass_NonGeneric()
        {
            object e = TypeConverter.Convert(typeof(TypeCode), typeof(Enum), TypeCode.Byte);
            Assert.IsTrue(e is TypeCode);
            Assert.IsTrue((TypeCode)e == TypeCode.Byte);
        }
        
        [TestMethod]
        public void ConvertEnumByte() => TestEnumConvert<byte>();

        [TestMethod]
        public void ConvertEnumSByte() => TestEnumConvert<sbyte>();

        [TestMethod]
        public void ConvertEnumShort() => TestEnumConvert<short>();

        [TestMethod]
        public void ConvertEnumUShort() => TestEnumConvert<ushort>();

        [TestMethod]
        public void ConvertEnumInt() => TestEnumConvert<int>();

        [TestMethod]
        public void ConvertEnumUInt() => TestEnumConvert<uint>();

        [TestMethod]
        public void ConvertEnumLong() => TestEnumConvert<long>();

        [TestMethod]
        public void ConvertEnumULong() => TestEnumConvert<ulong>();

        [TestMethod]
        public void ConvertEnumFloat() => TestEnumConvert<float>();

        [TestMethod]
        public void ConvertEnumDouble() => TestEnumConvert<double>();

        [TestMethod]
        public void ConvertEnumDecimal() => TestEnumConvert<decimal>();

        private void TestEnumConvert<TTo>()
            where TTo : struct, IComparable, IConvertible
        {
            { 
                // NOT NULL -> NOT NULL
                TTo toStruct = TypeConverter.Convert<TypeCode, TTo>(TypeCode.UInt64);
                TypeCode afterConversion = TypeConverter.Convert<TTo, TypeCode>(toStruct);
                Assert.IsTrue(afterConversion == TypeCode.UInt64);
            }

            {
                // NULLABLE -> NOT NULL
                TTo toStruct = TypeConverter.Convert<TypeCode?, TTo>(TypeCode.UInt64);
                TypeCode? afterConversion = TypeConverter.Convert<TTo, TypeCode?>(toStruct);
                Assert.IsTrue(afterConversion == TypeCode.UInt64);
            }

            {
                // NOT NULL -> NULLABLE
                TTo? toStruct = TypeConverter.Convert<TypeCode, TTo?>(TypeCode.UInt64);
                TypeCode afterConversion = TypeConverter.Convert<TTo?, TypeCode>(toStruct);
                Assert.IsTrue(afterConversion == TypeCode.UInt64);
            }
        }

        [TestMethod]
        public void ConvertEnumByteNonGen() => TestEnumConvertNonGen<byte>();

        [TestMethod]
        public void ConvertEnumSByteNonGen() => TestEnumConvertNonGen<sbyte>();

        [TestMethod]
        public void ConvertEnumShortNonGen() => TestEnumConvertNonGen<short>();

        [TestMethod]
        public void ConvertEnumUShortNonGen() => TestEnumConvertNonGen<ushort>();

        [TestMethod]
        public void ConvertEnumIntNonGen() => TestEnumConvertNonGen<int>();

        [TestMethod]
        public void ConvertEnumUIntNonGen() => TestEnumConvertNonGen<uint>();

        [TestMethod]
        public void ConvertEnumLongNonGen() => TestEnumConvertNonGen<long>();

        [TestMethod]
        public void ConvertEnumULongNonGen() => TestEnumConvertNonGen<ulong>();

        [TestMethod]
        public void ConvertEnumFloatNonGen() => TestEnumConvertNonGen<float>();

        [TestMethod]
        public void ConvertEnumDoubleNonGen() => TestEnumConvertNonGen<double>();

        [TestMethod]
        public void ConvertEnumDecimalNonGen() => TestEnumConvertNonGen<decimal>();

        private static void TestEnumConvertNonGen<TTo>()
            where TTo : struct, IComparable, IConvertible
        {
            object toObj = TypeConverter.Convert(typeof(TypeCode), typeof(TTo), TypeCode.UInt64);
            Assert.IsNotNull(toObj);
            Assert.IsTrue(toObj is TTo);

            object afterConversion = TypeConverter.Convert(typeof(TTo), typeof(TypeCode), toObj);
            Assert.IsNotNull(afterConversion);
            Assert.IsTrue(afterConversion is TypeCode);
            Assert.IsTrue((TypeCode)afterConversion == TypeCode.UInt64);
        }
        
        [TestMethod]
        public void TestParseTimeSpan() => TestParse(DateTime.Now.TimeOfDay);

        [TestMethod]
        public void TestParseDateTime() => TestParse(DateTime.Now);

        [TestMethod]
        public void TestParseByte() => TestParse(byte.MaxValue);

        [TestMethod]
        public void TestParseSByte() => TestParse(sbyte.MaxValue);

        [TestMethod]
        public void TestParseShort() => TestParse(short.MaxValue);

        [TestMethod]
        public void TestParseUShort() => TestParse(ushort.MaxValue);

        [TestMethod]
        public void TestParseInt() => TestParse(int.MaxValue);

        [TestMethod]
        public void TestParseUInt() => TestParse(uint.MaxValue);

        [TestMethod]
        public void TestParseLong() => TestParse(long.MaxValue);

        [TestMethod]
        public void TestParseULong() => TestParse(ulong.MaxValue);

        [TestMethod]
        public void TestParseFloat() => TestParse(float.MaxValue);

        [TestMethod]
        public void TestParseDouble() => TestParse(double.MaxValue);

        [TestMethod]
        public void TestDecimal() => TestParse(decimal.MaxValue);

        [TestMethod]
        public void TestParseBool() => TestParse(false);

        private static void TestParse<TInOut>(TInOut valueToParse)
            where TInOut : struct
        {
            string stringValue = TypeConverter.Convert<TInOut, string>(valueToParse);
            Assert.IsNotNull(stringValue);
            TInOut parsedValue = TypeConverter.Convert<string, TInOut>(stringValue);
            Assert.AreEqual(stringValue, valueToParse.ToString());
        }

        [TestMethod]
        public void FromShortToByte() => TestValueType<short, byte>(7);

        [TestMethod]
        public void FromUShortToByte() => TestValueType<ushort, byte>(7);

        [TestMethod]
        public void FromIntToByte() => TestValueType<int, byte>(7);

        [TestMethod]
        public void FromLongToByte() => TestValueType<long, byte>(7);

        [TestMethod]
        public void FromULongToByte() => TestValueType<ulong, byte>(7);

        public void TestValueType<TFrom, TTo>(TFrom val)
            where TFrom : struct, IComparable, IFormattable, IConvertible, IComparable<TFrom>, IEquatable<TFrom>
            where TTo : struct, IComparable, IFormattable, IConvertible, IComparable<TTo>, IEquatable<TTo>
        {
            TTo ssv = TypeConverter.Convert<TFrom, TTo>(val);
            object ssv2 = TypeConverter.Convert(typeof(TFrom), typeof(TTo), val);
            Assert.AreEqual(ssv, ssv2);

            TTo? snv = TypeConverter.Convert<TFrom, TTo?>(val);
            object snv2 = TypeConverter.Convert(typeof(TFrom), typeof(TTo), val);
            Assert.AreEqual(snv, snv2);

            TTo nsv = TypeConverter.Convert<TFrom?, TTo>(val);
            object nsv2 = TypeConverter.Convert(typeof(TFrom?), typeof(TTo), val);
            Assert.AreEqual(nsv, nsv2);

            TTo? nnv = TypeConverter.Convert<TFrom?, TTo?>(val);
            object nnv2 = TypeConverter.Convert(typeof(TFrom?), typeof(TTo?), val);
            Assert.AreEqual(nnv, nnv2);

            AllEqual(ssv, ssv2, snv, snv2, nsv, nsv2, nnv, nnv2);

            TTo? nnn = TypeConverter.Convert<TFrom?, TTo?>(null);
            object nnn2 = TypeConverter.Convert(typeof(TFrom?), typeof(TTo?), null);
            Assert.IsTrue(nnn == null);
            Assert.IsTrue(nnn2 == null);

            TTo nsn = TypeConverter.Convert<TFrom?, TTo>(null);
            object nsn2 = TypeConverter.Convert(typeof(TFrom?), typeof(TTo), null);
            Assert.AreEqual(default(TTo), nsn);
            Assert.AreEqual(default(TTo), nsn2);
        }

        private static void AllEqual(params object[] values)
        {
            Assert.AreEqual(values.Select(x => x == null ? null : x.ToString()).Distinct().Count(), 1);
        }
    }
}
