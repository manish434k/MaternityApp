using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel.ValueProviders;

namespace Symlconnect.DataModel.Factories
{
    public class AgeValueProviderFactory : IFactory<AgeValueProvider>
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public AgeValueProviderFactory(ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        public AgeValueProvider CreateInstance()
        {
            return new AgeValueProvider(_currentDateTimeProvider);
        }
    }
}