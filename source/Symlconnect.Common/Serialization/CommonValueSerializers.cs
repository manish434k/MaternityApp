using System;
using System.Xml;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.Common.Serialization
{
    /// <summary>
    ///     Common serializers for serializing to a string from Boolean, DateTime, Double and Int64 values.
    /// </summary>
    public class CommonValueSerializers : IValueSerializer<bool>, IValueSerializer<double>, IValueSerializer<DateTime>,
        IValueSerializer<long>
    {
        public string SerializeValue(bool value)
        {
            return XmlConvert.ToString(value);
        }

        public string SerializeValue(double value)
        {
            return XmlConvert.ToString(value);
        }

        public string SerializeValue(DateTime value)
        {
            return value.SafeUniversal().ToString("O");
        }

        public string SerializeValue(long value)
        {
            return XmlConvert.ToString(value);
        }
    }
}