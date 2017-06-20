using System.Xml.Linq;

namespace Symlconnect.Contracts.Serialization
{
    /// <summary>
    /// Implemented by a type that can deserialize a type <typeparamref name="TRoot"/> from an XDocument instance.
    /// </summary>
    /// <typeparam name="TRoot">The root type which can be deserialized</typeparam>
    public interface IDocumentDeserializer<out TRoot>
    {
        TRoot DeserializeFromXDocument(XDocument document);
    }
}