using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SqlExtensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [Serializable]
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

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private ConversionNotSupportedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            To = (Type)info.GetValue(nameof(To), typeof(Type));
            From = (Type)info.GetValue(nameof(From), typeof(Type));
            Value = info.GetValue(nameof(Value), typeof(object));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(To), To);
            info.AddValue(nameof(From), From);
            info.AddValue(nameof(Value), Value);

            base.GetObjectData(info, context);
        }
    }
}
