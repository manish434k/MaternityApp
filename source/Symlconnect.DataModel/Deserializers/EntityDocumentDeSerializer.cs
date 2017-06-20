using System.Collections.Generic;
using Symlconnect.Common.Serialization;
using Symlconnect.Contracts.Diagnostics;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    /// <summary>
    ///     Deserializes an Entity from an XDocument.
    /// </summary>
    public class EntityDocumentDeSerializer : DocumentDeserializer<Entity>
    {
        public EntityDocumentDeSerializer(IEnumerable<IElementDeserializer<IEntity>> elementDeserializers,
            IEnumerable<IElementGroupDeserializer> elementGroupDeserializers, ILogger logger)
            : base(elementDeserializers, elementGroupDeserializers, logger)
        {
        }
    }
}