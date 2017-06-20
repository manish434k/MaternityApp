using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Factories
{
    public class PropertyDefinitionFactory: IFactory<PropertyDefinition>
    {
        public PropertyDefinition CreateInstance()
        {
            return new PropertyDefinition();
        }
    }
}