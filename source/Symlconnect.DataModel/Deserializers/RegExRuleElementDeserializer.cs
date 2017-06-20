using System.Xml.Linq;
using Symlconnect.Common.ExtensionMethods;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel.Rules;

namespace Symlconnect.DataModel.Deserializers
{
    public class RegExRuleElementDeserializer : RuleDeserializerBase<RegExRuleDefinition>
    {
        public RegExRuleElementDeserializer(IFactory<RegExRuleDefinition> instanceFactory) : base(instanceFactory)
        {
        }

        public override string ElementName => "regexrule";

        protected override void DeserializeAdditionalAttributes(RegExRuleDefinition instance, XElement element,
            object parent,
            IDataDictionary root)
        {
            element.ValidateRequiredAttributes("pattern");

            instance.Pattern = element.Attribute("pattern").Value;
        }
    }
}