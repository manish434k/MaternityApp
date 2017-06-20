using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Factories
{
    public class EntityDefinitionFactory : IFactory<EntityDefinition>
    {
        public EntityDefinition CreateInstance()
        {
            return new EntityDefinition();
        }
    }
}