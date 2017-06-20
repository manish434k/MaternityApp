using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Factories
{
    public class EntityFactory : IFactory<Entity>
    {
        private readonly IFactory<EntityPropertyValueCollection> _entityPropertyValueCollectionFactory;
        private readonly IFactory<ChildEntityCollection> _childEntityCollectionFactory;
        private readonly IFactory<ChildEntity> _childEntityFactory;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public EntityFactory(IFactory<EntityPropertyValueCollection> entityPropertyValueCollectionFactory,
            IFactory<ChildEntityCollection> childEntityCollectionFactory,
            IFactory<ChildEntity> childEntityFactory,
            ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _entityPropertyValueCollectionFactory = entityPropertyValueCollectionFactory;
            _childEntityCollectionFactory = childEntityCollectionFactory;
            _childEntityFactory = childEntityFactory;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        public Entity CreateInstance()
        {
            return new Entity(_entityPropertyValueCollectionFactory, _childEntityCollectionFactory,
                _childEntityFactory, _currentDateTimeProvider);
        }
    }
}