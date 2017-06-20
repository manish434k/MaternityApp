using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel.ValueProviders;

namespace Symlconnect.DataModel.Factories
{
    public class DueDateValueProviderFactory : IFactory<DueDateProvider>
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public DueDateValueProviderFactory(ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        public DueDateProvider CreateInstance()
        {
            return new DueDateProvider(_currentDateTimeProvider);
        }
    }
}
