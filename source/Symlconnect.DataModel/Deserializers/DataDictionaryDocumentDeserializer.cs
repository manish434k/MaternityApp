using System.Collections.Generic;
using Symlconnect.Common.Serialization;
using Symlconnect.Contracts.Diagnostics;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    /// <summary>
    ///     Xml Document deserializer for a Data Dictionary.
    /// </summary>
    public class DataDictionaryDocumentDeserializer : DocumentDeserializer<IDataDictionary>
    {
        public DataDictionaryDocumentDeserializer(
            IEnumerable<IElementDeserializer<IDataDictionary>> elementDeserializers,
            IEnumerable<IElementGroupDeserializer> elementGroupDeserializers, ILogger logger)
            : base(elementDeserializers, elementGroupDeserializers, logger)
        {
        }
    }
}