using System;
using System.Globalization;
using System.Xml;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.Common.Serialization
{
    /// <summary>
    ///     Common Deserializers for deserializing to Boolean, DateTime, Double and Int64 values.
    /// </summary>
    public class CommonValueDeserializers : IValueDeserializer<bool>, IValueDeserializer<DateTime>,
        IValueDeserializer<double>, IValueDeserializer<long>
    {
        bool IValueDeserializer<bool>.DeserializeValue(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }
            return XmlConvert.ToBoolean(text?.ToLowerInvariant());
        }

        DateTime IValueDeserializer<DateTime>.DeserializeValue(string text)
        {
            return DateTime.Parse(text, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
        }

        double IValueDeserializer<double>.DeserializeValue(string text)
        {
            return XmlConvert.ToDouble(text);
        }

        long IValueDeserializer<long>.DeserializeValue(string text)
        {
            return XmlConvert.ToInt64(text);
        }
    }
}