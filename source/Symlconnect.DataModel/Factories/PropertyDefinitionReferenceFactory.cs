using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Factories
{
    public class PropertyDefinitionReferenceFactory : IFactory<PropertyDefinitionReference>
    {
        public PropertyDefinitionReference CreateInstance()
        {
            return new PropertyDefinitionReference();
        }
    }
}