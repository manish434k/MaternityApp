using System.Collections.Generic;
using Symlconnect.Common.Serialization;
using Symlconnect.Contracts.Diagnostics;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.ViewModel.Deserializers
{
    /// <summary>
    ///     Deserializes a Form Definition from an XDocument.
    /// </summary>
    public class FormDefinitionDocumentDeserializer : DocumentDeserializer<IFormDefinition>
    {
        public FormDefinitionDocumentDeserializer(
            IEnumerable<IElementDeserializer<IFormDefinition>> elementDeserializers,
            IEnumerable<IElementGroupDeserializer> elementGroupDeserializers, ILogger logger)
            : base(elementDeserializers, elementGroupDeserializers, logger)
        {
        }
    }
}