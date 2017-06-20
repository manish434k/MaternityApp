using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;

namespace Symlconnect.DataModel.Factories
{
    public class AuditPropertyDefinitionFactory : IFactory<AuditPropertyDefinition>
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public AuditPropertyDefinitionFactory(ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        public AuditPropertyDefinition CreateInstance()
        {
            return new AuditPropertyDefinition(_currentDateTimeProvider);
        }
    }
}