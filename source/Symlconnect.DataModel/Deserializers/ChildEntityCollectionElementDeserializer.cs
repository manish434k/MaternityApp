using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;
using Symlconnect.Contracts.Serialization;

namespace Symlconnect.DataModel.Deserializers
{
    public class ChildEntityCollectionElementDeserializer : IElementDeserializer<IEntity>
    {
        private readonly IFactory<ChildEntityCollection> _childEntityCollectionFactory;

        public ChildEntityCollectionElementDeserializer(IFactory<ChildEntityCollection> childEntityCollectionFactory)
        {
            _childEntityCollectionFactory = childEntityCollectionFactory;
        }

        public string ElementName => "childentities";

        public object DeserializeFromXElement(XElement element, object parent, IEntity root)
        {
            element.ValidateRequiredAttributes("propertyname");

            var collection = _childEntityCollectionFactory.CreateInstance();
            collection.PropertyName = element.Attribute("propertyname").Value;
            return collection;
        }
    }
}