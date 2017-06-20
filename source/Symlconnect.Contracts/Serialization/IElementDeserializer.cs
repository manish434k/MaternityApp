using System.Xml.Linq;

namespace Symlconnect.Contracts.Serialization
{
    public interface IElementDeserializer<in TRoot>
    {
        string ElementName { get; }
        object DeserializeFromXElement(XElement element, object parent, TRoot root);
    }
}