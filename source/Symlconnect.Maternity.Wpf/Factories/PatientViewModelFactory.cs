using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;
using Symlconnect.Maternity.Common.ViewModels;

namespace Symlconnect.Maternity.Wpf.Factories
{
    public class PatientViewModelFactory : IFactory<PatientViewModel>
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public PatientViewModelFactory(ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        public PatientViewModel CreateInstance()
        {
            return new PatientViewModel(_currentDateTimeProvider);
        }
    }
}