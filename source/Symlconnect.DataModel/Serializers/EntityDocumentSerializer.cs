using System.Collections.Generic;
using Symlconnect.Common.Serialization;
using Symlconnect.Contracts.Diagnostics;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Serializers
{
    public class EntityDocumentSerializer : DocumentSerializer<IEntity>
    {
        public EntityDocumentSerializer(IEnumerable<IElementSerializer<IEntity>> elementSerializers, ILogger logger) 
            : base(elementSerializers, logger)
        {

        }
    }
}