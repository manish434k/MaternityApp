using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Factories
{
    public class EntityPropertyValueCollectionFactory : IFactory<EntityPropertyValueCollection>
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        private readonly EntityPropertyValueFactory _entityPropertyValueFactory;

        public EntityPropertyValueCollectionFactory(ICurrentDateTimeProvider currentDateTimeProvider,
            EntityPropertyValueFactory entityPropertyValueFactory)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
            _entityPropertyValueFactory = entityPropertyValueFactory;
        }

        public EntityPropertyValueCollection CreateInstance()
        {
            return new EntityPropertyValueCollection(_currentDateTimeProvider, _entityPropertyValueFactory);
        }
    }
}