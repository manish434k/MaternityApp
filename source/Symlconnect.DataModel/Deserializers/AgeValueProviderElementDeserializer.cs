using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel.ValueProviders;

namespace Symlconnect.DataModel.Deserializers
{
    public class AgeValueProviderElementDeserializer : ValueProviderElementDeserializerBase<AgeValueProvider>
    {
        public AgeValueProviderElementDeserializer(IFactory<AgeValueProvider> valueProviderFactory)
            : base(valueProviderFactory)
        {
        }

        public override object DeserializeFromXElement(XElement element, object parent, IDataDictionary root)
        {
            var newInstance = (AgeValueProvider) base.DeserializeFromXElement(element, parent, root);

            element.ValidateRequiredAttributes("dateofbirthpropertyname");

            newInstance.DateOfBirthPropertyName = element.Attribute("dateofbirthpropertyname").Value;
            newInstance.ReferenceDatePropertyName = element.Attribute("referencedatepropertyname")?.Value;

            return newInstance;
        }

        public override string ElementName => "age";
    }
}