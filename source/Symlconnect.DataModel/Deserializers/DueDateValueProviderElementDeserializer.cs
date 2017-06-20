using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel.ValueProviders;

namespace Symlconnect.DataModel.Deserializers
{
    public class DueDateValueProviderElementDeserializer : ValueProviderElementDeserializerBase<DueDateProvider>
    {
        public DueDateValueProviderElementDeserializer(IFactory<DueDateProvider> valueProviderFactory)
            : base(valueProviderFactory)
        {
        }

        public override object DeserializeFromXElement(XElement element, object parent, IDataDictionary root)
        {
            var newInstance = (DueDateProvider)base.DeserializeFromXElement(element, parent, root);

            element.ValidateRequiredAttributes("mydueDateProperty");

            newInstance.DueDatePropertyName = element.Attribute("mydueDateProperty").Value;

            return newInstance;
        }

        public override string ElementName => "mydueDate";
    }
}
