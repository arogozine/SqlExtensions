using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SqlExtensions
{
    public delegate object Converter(object input);

    public delegate TTo Converter<TFrom, TTo>(TFrom input);

    public static class TypeConverter
    {
        private static readonly Dictionary<Type, Dictionary<Type, Converter>> Converters
            = new Dictionary<Type, Dictionary<Type, Converter>>();

        private static readonly Converter NoOp = x => x;

        #region Static Initialization

        static TypeConverter() {
            AddSystemTypeCasts();
            AddParseConverters();
            AddManualTypeConversions();
        }

        private static void AddParseConverters()
        {
            // For fast initialization speed
            // there are hard coded
            AddConverter(typeof(string), typeof(byte), x => byte.Parse((string)x, CultureInfo.InvariantCulture));
            AddConverter(typeof(string), typeof(sbyte), x => sbyte.Parse((string)x, CultureInfo.InvariantCulture));
            AddConverter(typeof(string), typeof(short), x => short.Parse((string)x, CultureInfo.InvariantCulture));
            AddConverter(typeof(string), typeof(ushort), x => ushort.Parse((string)x, CultureInfo.InvariantCulture));
            AddConverter(typeof(string), typeof(int), x => int.Parse((string)x, CultureInfo.InvariantCulture));
            AddConverter(typeof(string), typeof(uint), x => uint.Parse((string)x, CultureInfo.InvariantCulture));
            AddConverter(typeof(string), typeof(long), x => long.Parse((string)x, CultureInfo.InvariantCulture));
            AddConverter(typeof(string), typeof(ulong), x => ulong.Parse((string)x, CultureInfo.InvariantCulture));
            AddConverter(typeof(string), typeof(float), x => float.Parse((string)x, CultureInfo.InvariantCulture));
            AddConverter(typeof(string), typeof(double), x => double.Parse((string)x, CultureInfo.InvariantCulture));
            AddConverter(typeof(string), typeof(double), x => double.Parse((string)x, CultureInfo.InvariantCulture));
            AddConverter(typeof(string), typeof(decimal), x => decimal.Parse((string)x, CultureInfo.InvariantCulture));

            AddConverter(typeof(string), typeof(bool), x => bool.Parse((string)x));

            AddConverter(typeof(string), typeof(DateTime), x => DateTime.Parse((string)x, CultureInfo.InvariantCulture));
            AddConverter(typeof(string), typeof(TimeSpan), x => TimeSpan.Parse((string)x, CultureInfo.InvariantCulture));
        }

        private static void AddSystemTypeCasts()
        {
            // System Name Space conversions
            // Pre-Calculated (for fast initialization)
            AddCastConversion<DateTime, DateTimeOffset>();
            AddCastConversion<Boolean, Byte>();
            AddCastConversion<Boolean, Char>();
            AddCastConversion<Boolean, Double>();
            AddCastConversion<Boolean, Int16>();
            AddCastConversion<Boolean, Int32>();
            AddCastConversion<Boolean, Int64>();
            AddCastConversion<Boolean, SByte>();
            AddCastConversion<Boolean, Single>();
            AddCastConversion<Boolean, UInt16>();
            AddCastConversion<Boolean, UInt32>();
            AddCastConversion<Boolean, UInt64>();
            AddCastConversion<Byte, Char>();
            AddCastConversion<Byte, Decimal>();
            AddCastConversion<Byte, Double>();
            AddCastConversion<Byte, Int16>();
            AddCastConversion<Byte, Int32>();
            AddCastConversion<Byte, Int64>();
            AddCastConversion<Byte, SByte>();
            AddCastConversion<Byte, Single>();
            AddCastConversion<Byte, UInt16>();
            AddCastConversion<Byte, UInt32>();
            AddCastConversion<Byte, UInt64>();
            AddCastConversion<Char, Byte>();
            AddCastConversion<Char, Decimal>();
            AddCastConversion<Char, Double>();
            AddCastConversion<Char, Int16>();
            AddCastConversion<Char, Int32>();
            AddCastConversion<Char, Int64>();
            AddCastConversion<Char, SByte>();
            AddCastConversion<Char, Single>();
            AddCastConversion<Char, UInt16>();
            AddCastConversion<Char, UInt32>();
            AddCastConversion<Char, UInt64>();
            AddCastConversion<Decimal, Byte>();
            AddCastConversion<Decimal, Char>();
            AddCastConversion<Decimal, Double>();
            AddCastConversion<Decimal, Int16>();
            AddCastConversion<Decimal, Int32>();
            AddCastConversion<Decimal, Int64>();
            AddCastConversion<Decimal, SByte>();
            AddCastConversion<Decimal, Single>();
            AddCastConversion<Decimal, UInt16>();
            AddCastConversion<Decimal, UInt32>();
            AddCastConversion<Decimal, UInt64>();
            AddCastConversion<Double, Byte>();
            AddCastConversion<Double, Char>();
            AddCastConversion<Double, Decimal>();
            AddCastConversion<Double, Int16>();
            AddCastConversion<Double, Int32>();
            AddCastConversion<Double, Int64>();
            AddCastConversion<Double, SByte>();
            AddCastConversion<Double, Single>();
            AddCastConversion<Double, UInt16>();
            AddCastConversion<Double, UInt32>();
            AddCastConversion<Double, UInt64>();
            AddCastConversion<Int16, Byte>();
            AddCastConversion<Int16, Char>();
            AddCastConversion<Int16, Decimal>();
            AddCastConversion<Int16, Double>();
            AddCastConversion<Int16, Int32>();
            AddCastConversion<Int16, Int64>();
            AddCastConversion<Int16, SByte>();
            AddCastConversion<Int16, Single>();
            AddCastConversion<Int16, UInt16>();
            AddCastConversion<Int16, UInt32>();
            AddCastConversion<Int16, UInt64>();
            AddCastConversion<Int32, Byte>();
            AddCastConversion<Int32, Char>();
            AddCastConversion<Int32, Decimal>();
            AddCastConversion<Int32, Double>();
            AddCastConversion<Int32, Int16>();
            AddCastConversion<Int32, Int64>();
            AddCastConversion<Int32, IntPtr>();
            AddCastConversion<Int32, SByte>();
            AddCastConversion<Int32, Single>();
            AddCastConversion<Int32, UInt16>();
            AddCastConversion<Int32, UInt32>();
            AddCastConversion<Int32, UInt64>();
            AddCastConversion<Int64, Byte>();
            AddCastConversion<Int64, Char>();
            AddCastConversion<Int64, Decimal>();
            AddCastConversion<Int64, Double>();
            AddCastConversion<Int64, Int16>();
            AddCastConversion<Int64, Int32>();
            AddCastConversion<Int64, IntPtr>();
            AddCastConversion<Int64, SByte>();
            AddCastConversion<Int64, Single>();
            AddCastConversion<Int64, UInt16>();
            AddCastConversion<Int64, UInt32>();
            AddCastConversion<Int64, UInt64>();
            AddCastConversion<IntPtr, Int32>();
            AddCastConversion<IntPtr, Int64>();
            AddCastConversion<SByte, Byte>();
            AddCastConversion<SByte, Char>();
            AddCastConversion<SByte, Decimal>();
            AddCastConversion<SByte, Double>();
            AddCastConversion<SByte, Int16>();
            AddCastConversion<SByte, Int32>();
            AddCastConversion<SByte, Int64>();
            AddCastConversion<SByte, Single>();
            AddCastConversion<SByte, UInt16>();
            AddCastConversion<SByte, UInt32>();
            AddCastConversion<SByte, UInt64>();
            AddCastConversion<Single, Byte>();
            AddCastConversion<Single, Char>();
            AddCastConversion<Single, Decimal>();
            AddCastConversion<Single, Double>();
            AddCastConversion<Single, Int16>();
            AddCastConversion<Single, Int32>();
            AddCastConversion<Single, Int64>();
            AddCastConversion<Single, SByte>();
            AddCastConversion<Single, UInt16>();
            AddCastConversion<Single, UInt32>();
            AddCastConversion<Single, UInt64>();
            AddCastConversion<UInt16, Byte>();
            AddCastConversion<UInt16, Char>();
            AddCastConversion<UInt16, Decimal>();
            AddCastConversion<UInt16, Double>();
            AddCastConversion<UInt16, Int16>();
            AddCastConversion<UInt16, Int32>();
            AddCastConversion<UInt16, Int64>();
            AddCastConversion<UInt16, SByte>();
            AddCastConversion<UInt16, Single>();
            AddCastConversion<UInt16, UInt32>();
            AddCastConversion<UInt16, UInt64>();
            AddCastConversion<UInt32, Byte>();
            AddCastConversion<UInt32, Char>();
            AddCastConversion<UInt32, Decimal>();
            AddCastConversion<UInt32, Double>();
            AddCastConversion<UInt32, Int16>();
            AddCastConversion<UInt32, Int32>();
            AddCastConversion<UInt32, Int64>();
            AddCastConversion<UInt32, SByte>();
            AddCastConversion<UInt32, Single>();
            AddCastConversion<UInt32, UInt16>();
            AddCastConversion<UInt32, UInt64>();
            AddCastConversion<UInt32, UIntPtr>();
            AddCastConversion<UInt64, Byte>();
            AddCastConversion<UInt64, Char>();
            AddCastConversion<UInt64, Decimal>();
            AddCastConversion<UInt64, Double>();
            AddCastConversion<UInt64, Int16>();
            AddCastConversion<UInt64, Int32>();
            AddCastConversion<UInt64, Int64>();
            AddCastConversion<UInt64, SByte>();
            AddCastConversion<UInt64, Single>();
            AddCastConversion<UInt64, UInt16>();
            AddCastConversion<UInt64, UInt32>();
            AddCastConversion<UInt64, UIntPtr>();
            AddCastConversion<UIntPtr, UInt32>();
            AddCastConversion<UIntPtr, UInt64>();

            /*
            var results = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.Namespace == nameof(System))
                .Where(t => t.IsPublic)
                .Where(t => t.IsValueType)
                .Where(t => typeof(ArgIterator) != t)
                .Where(t => typeof(RuntimeArgumentHandle) != t)
                .Where(t => typeof(TypedReference) != t)
                .Where(t => typeof(void) != t)
                .Where(t => !t.IsGenericType)
                .Where(t => !t.IsEnum)
                .ToList();
            */
        }

        private static void AddCastConversion<TFrom, TTo>()
            where TFrom : struct
            where TTo : struct
        {
            Converter caster = GenerateCast(typeof(TFrom), typeof(TTo));
            AddCaster<TFrom, TTo>(caster);
        }
        
        private static void AddManualTypeConversions()
        {
            AddCaster<DateTime, TimeSpan>(x => ((DateTime)x).TimeOfDay);
            AddCaster<TimeSpan, DateTime>(x => new DateTime() + (TimeSpan)x);
            
            // Char -> Numeric
            AddConverter(typeof(char), typeof(double), x => char.GetNumericValue((char)x));
            AddConverter(typeof(char), typeof(float), x => (float)char.GetNumericValue((char)x));
            AddConverter(typeof(char), typeof(byte), x => (byte)char.GetNumericValue((char)x));
            AddConverter(typeof(char), typeof(sbyte), x => (sbyte)char.GetNumericValue((char)x));
            AddConverter(typeof(char), typeof(short), x => (short)char.GetNumericValue((char)x));
            AddConverter(typeof(char), typeof(ushort), x => (ushort)char.GetNumericValue((char)x));
            AddConverter(typeof(char), typeof(int), x => (int)char.GetNumericValue((char)x));
            AddConverter(typeof(char), typeof(uint), x => (uint)char.GetNumericValue((char)x));
            AddConverter(typeof(char), typeof(long), x => (long)char.GetNumericValue((char)x));
            AddConverter(typeof(char), typeof(ulong), x => (ulong)char.GetNumericValue((char)x));
        }

        private static void AddCaster<TFrom, TTo>(Converter converter)
            where TFrom : struct
            where TTo : struct
        {
            AddConverter(typeof(TFrom), typeof(TTo), converter);
            AddConverter(typeof(TFrom), typeof(TTo?), converter);
            AddConverter(typeof(TFrom?), typeof(TTo), x => x == null ? default(TTo) : converter(((TFrom?)x).Value));
            AddConverter(typeof(TFrom?), typeof(TTo?), x => x == null ? null : converter(((TFrom?)x).Value));
        }

        #endregion

        private static Converter GenerateCast(Type from, Type to) {
            // Object
            var fromParam = Expression.Parameter(typeof(object), "from");
            // (TFrom)Object
            var fromCast = Expression.Convert(fromParam, from);
            // (TTo)(TFrom)Object
            var fromToCast = Expression.Convert(fromCast, to);
            // (object)(TTo)(TFrom)Object
            var toObjectCast = Expression.Convert(fromToCast, typeof(object));

            return Expression.Lambda<Converter>(toObjectCast, fromParam)
                .Compile();
        }

        private static Converter GetParse(Type outType)
        {
            // Get Static "Parse" Method
            IEnumerable<MethodInfo> methods =
                from m in outType.GetMethods(BindingFlags.Static | BindingFlags.Public)
                where m.Name == "Parse"
                let param = m.GetParameters()
                where param.Length == 1
                where param[0].ParameterType == typeof(string)
                select m;
            MethodInfo parseMethod = methods.First();

            // Compile
            var inParam = Expression.Parameter(typeof(object), "input");
            var inStr = Expression.Convert(inParam, typeof(string));
            var methodCallExp = Expression.Call(parseMethod, inStr);
            var outAsObjectExp = Expression.Convert(methodCallExp, typeof(object));
            return Expression.Lambda<Converter>(outAsObjectExp, inParam)
                .Compile();
        }

        private static bool TryGenerate(Type from, Type to) {

            if (to.IsAssignableFrom(from))
            {
                // No Conversion is Necessary
                AddConverter(from, to, NoOp);
                return true;
            }
            else
            {
                // We can do conversions for Enum types
                // check both from and to
                if (from.GetTypeInfo().IsEnum)
                {
                    AddEnumConverters(from);
                }

                if (to.GetTypeInfo().IsEnum)
                {
                    AddEnumConverters(to);
                }

                if (to.GetTypeInfo().IsEnum || from.GetTypeInfo().IsEnum)
                {
                    // We added some enum converters
                    // try again
                    return true;
                }
                else if (from == typeof(string))
                {
                    // We can call a static "Parse" function
                    try
                    {
                        AddConverter(from, to, GetParse(to));
                        return true;
                    }
                    catch
                    {
                        // no luck
                    }
                }
            }
            
            if (TryAddComponentTypeConverter(from, to))
            {
                return true;
            }

            try
            {
                AddCaster(from, to);
                return true;
            }
            catch (InvalidOperationException) {

            }

            return TryAddIConvertibleConverter(from, to);
        }

        private static bool TryAddIConvertibleConverter(Type from, Type to)
        {
            if (from.GetTypeInfo().GetInterface(nameof(IConvertible)) != null)
            {
                switch (Type.GetTypeCode(to))
                {
                    case TypeCode.Boolean:
                        AddConverter(from, to, x => ((IConvertible)x).ToBoolean(null));
                        return true;
                    case TypeCode.Byte:
                        AddConverter(from, to, x => ((IConvertible)x).ToByte(null));
                        return true;
                    case TypeCode.Char:
                        AddConverter(from, to, x => ((IConvertible)x).ToChar(null));
                        return true;
                    case TypeCode.DateTime:
                        AddConverter(from, to, x => ((IConvertible)x).ToDateTime(null));
                        return true;
                    case TypeCode.Decimal:
                        AddConverter(from, to, x => ((IConvertible)x).ToDecimal(null));
                        return true;
                    case TypeCode.Double:
                        AddConverter(from, to, x => ((IConvertible)x).ToDouble(null));
                        return true;
                    case TypeCode.Int16:
                        AddConverter(from, to, x => ((IConvertible)x).ToInt16(null));
                        return true;
                    case TypeCode.Int32:
                        AddConverter(from, to, x => ((IConvertible)x).ToInt32(null));
                        return true;
                    case TypeCode.Int64:
                        AddConverter(from, to, x => ((IConvertible)x).ToInt64(null));
                        return true;
                    case TypeCode.SByte:
                        AddConverter(from, to, x => ((IConvertible)x).ToSByte(null));
                        return true;
                    case TypeCode.Single:
                        AddConverter(from, to, x => ((IConvertible)x).ToSingle(null));
                        return true;
                    case TypeCode.String:
                        AddConverter(from, to, x => ((IConvertible)x).ToString(null));
                        return true;
                    case TypeCode.UInt16:
                        AddConverter(from, to, x => ((IConvertible)x).ToUInt16(null));
                        return true;
                    case TypeCode.UInt32:
                        AddConverter(from, to, x => ((IConvertible)x).ToUInt32(null));
                        return true;
                    case TypeCode.UInt64:
                        AddConverter(from, to, x => ((IConvertible)x).ToUInt64(null));
                        return true;
                    default:
                        return false;
                }
            }

            return false;
        }
        
        private static bool TryAddComponentTypeConverter(Type from, Type to)
        {
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(to);
            if (converter != null) {
                if (converter.CanConvertFrom(from)) {
                    AddConverter(from, to, value => converter.ConvertFrom(value));
                    return true;
                }
            }

            converter = System.ComponentModel.TypeDescriptor.GetConverter(from);
            if (converter != null) {
                if (converter.CanConvertTo(to)) {
                    AddConverter(from, to, value => converter.ConvertTo(value, to));
                    return true;
                }
            }

            return false;
        }
        
        public static void AddEnumConverters(Type enumType)
        {
            AddEnumCast<byte>(enumType);
            AddEnumCast<sbyte>(enumType);
            AddEnumCast<short>(enumType);
            AddEnumCast<ushort>(enumType);
            AddEnumCast<int>(enumType);
            AddEnumCast<uint>(enumType);
            AddEnumCast<long>(enumType);
            AddEnumCast<ulong>(enumType);
            AddEnumCast<float>(enumType);
            AddEnumCast<double>(enumType);

            AddConverter(enumType, typeof(IComparable), NoOp);
            AddConverter(enumType, typeof(IFormattable), NoOp);
            AddConverter(enumType, typeof(IConvertible), NoOp);

            AddConverter(typeof(string), enumType, x => Enum.Parse(enumType, (string)x));
        }

        private static void AddEnumCast<TCast>(Type enumType)
            where TCast : struct, IComparable, IFormattable, IConvertible, IComparable<TCast>, IEquatable<TCast>
        {
            // Enum -> *
            AddConverter(enumType, typeof(TCast), GenerateCast(enumType, typeof(TCast)));
            AddConverter(enumType, typeof(TCast?), GenerateCast(enumType, typeof(TCast?)));

            Type nullableEnumType = typeof(Nullable<>).MakeGenericType(enumType);            
            var defaultEnumVal = Activator.CreateInstance(enumType);
            Converter conv = GenerateCast(nullableEnumType, typeof(TCast));

            // Nullable Enum -> *
            AddConverter(nullableEnumType, typeof(TCast?), GenerateCast(nullableEnumType, typeof(TCast?)));
            AddConverter(nullableEnumType, typeof(TCast), x => x == null ? defaultEnumVal : conv(x));

            var fromValueType = GenerateCast(typeof(TCast), enumType);

            // Nullable ValueType to Not Null Enum
            AddConverter(typeof(TCast?), enumType,
                x => x == null ? defaultEnumVal : fromValueType(x));
            // Nullable ValueType to Nullable Enum
            AddConverter(typeof(TCast?), nullableEnumType, GenerateCast(typeof(TCast?), nullableEnumType));
            // Not Null to Enum
            AddConverter(typeof(TCast), nullableEnumType, fromValueType);
            AddConverter(typeof(TCast), enumType, GenerateCast(typeof(TCast), enumType));
        }

        private static void AddCaster(Type from, Type to)
        {
            Type fromNullable = from.GetTypeInfo().IsValueType ? Nullable.GetUnderlyingType(from) : null;
            Type toNullable = to.GetTypeInfo().IsValueType ? Nullable.GetUnderlyingType(to) : null;

            if (fromNullable == null)
            {
                Converter caster = GenerateCast(from, to);
                AddConverter(from, to, caster);

                if (toNullable != null)
                {
                    // T to Not Nullable Y
                    AddConverter(from, toNullable, caster);
                }
            }
            else // from is a Nullable<Y>
            {
                object defaultOfTo = Activator.CreateInstance(to);

                if (toNullable != null) // to is a Nullable<Y>
                {
                    // Nullable<Y> to Nullable<T>
                    AddConverter(from, to, GenerateCast(from, to));
                    // Y to Nullable<T>
                    AddConverter(fromNullable, to, GenerateCast(fromNullable, to));
                    // Nullable<Y> to T
                    var cast = GenerateCast(from, toNullable);
                    AddConverter(from, to, x => x == null ? defaultOfTo : cast(x));
                    // Y to T
                    AddConverter(fromNullable, toNullable, GenerateCast(fromNullable, toNullable));
                }
                else if (to.GetTypeInfo().IsValueType)
                {
                    Converter caster = GenerateCast(fromNullable, to);
                    // Nullable<Y> to T
                    AddConverter(from, to, x => x == null ? defaultOfTo : caster(x));
                    // Y to T
                    AddConverter(fromNullable, to, GenerateCast(fromNullable, to));
                }
                else
                {
                    // Nullable<Y> to Object
                    Converter objCaster = GenerateCast(fromNullable, to);
                    AddConverter(from, to, x => x == null ? null : objCaster(x));
                    // Y to Object
                    AddConverter(fromNullable, to, GenerateCast(from, to));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvert(Type from, Type to, object value, out object result)
        {
            if (Converters.TryGetValue(from, out Dictionary<Type, Converter> toDict)
                && toDict.TryGetValue(to, out Converter converter))
            {
                result = converter(value);
                return true;
            }

            result = null;
            return false;
        }

        #region Public Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void AddConverter<TFrom, TTo>()
            where TFrom : TTo
        {
            AddConverter(typeof(TFrom), typeof(TTo), x => x);
        }

        public static void AddConverter<TFrom, TTo>(Converter<TFrom, TTo> converter)
        {
            AddConverter(typeof(TFrom), typeof(TTo), x => converter((TFrom)x));
        }

        public static void AddConverter(Type from, Type to, Converter converter)
        {
            if (!Converters.TryGetValue(from, out Dictionary<Type, Converter> innerDictionary))
            {
                innerDictionary = new Dictionary<Type, Converter>();
                Converters[from] = innerDictionary;
            }

            innerDictionary[to] = converter;
        }

        public static TOut Convert<TIn, TOut>(TIn value)
            => (TOut)Convert(typeof(TIn), typeof(TOut), value);

        public static object Convert(Type from, Type to, object value)
        {
            if (to == from)
            {
                return value;
            }
            else if (to == typeof(string))
            {
                return value?.ToString();
            }

            if (TryConvert(from, to, value, out object result) ||
                (TryGenerate(from, to) && TryConvert(from, to, value, out result)))
            {
                return result;
            }

            throw new ConversionNotSupportedException(from, to, value, "Can't convert");
        }

#endregion
    }
}
