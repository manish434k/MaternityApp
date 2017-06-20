using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Factories
{
    public class EntityPropertyValueFactory : IFactory<EntityPropertyValue>
    {
        public EntityPropertyValue CreateInstance()
        {
            return new EntityPropertyValue();
        }
    }
}