using System;

namespace SqlExtensions
{
    public sealed class ConversionNotSupportedException : Exception
    {
        public Type To { get; private set; }

        public Type From { get; private set; }

        public object Value { get; private set; }

        internal ConversionNotSupportedException(Type from, Type to, object value) 
            : base()
        {
            To = to;
            From = from;
            Value = value;
        }

        internal ConversionNotSupportedException(Type from, Type to, object value, string message) 
            : base(message)
        {
            To = to;
            From = from;
            Value = value;
        }

        internal ConversionNotSupportedException(Type from, Type to, object value, string message, Exception innerException) : 
            base(message, innerException)
        {
            To = to;
            From = from;
            Value = value;
        }
    }
}
