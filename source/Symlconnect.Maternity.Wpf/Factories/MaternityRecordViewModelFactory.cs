using Prism.Regions;
using Symlconnect.Contracts.Factories;
using Symlconnect.Maternity.Common.ViewModels;
using Symlconnect.ViewModel;
using Symlconnect.ViewModel.ViewModels;
using MaternityRecordViewModel = Symlconnect.Maternity.Wpf.ViewModels.MaternityRecordViewModel;

namespace Symlconnect.Maternity.Wpf.Factories
{
    public class MaternityRecordViewModelFactory : IFactory<Common.ViewModels.MaternityRecordViewModel>
    {
        private readonly IFactory<IFormContext> _formContextFactory;
        private readonly IFormDefinitionLocator _formDefinitionLocator;
        private readonly IFactory<PatientViewModel> _patientViewModelFactory;
        private readonly IRegionManager _regionManager;
        private readonly IFactory<FormViewModel> _formViewModelFactory;

        public MaternityRecordViewModelFactory(IFactory<IFormContext> formContextFactory,
            IFormDefinitionLocator formDefinitionLocator, IFactory<PatientViewModel> patientViewModelFactory,
            IRegionManager regionManager, IFactory<FormViewModel> formViewModelFactory)
        {
            _formContextFactory = formContextFactory;
            _formDefinitionLocator = formDefinitionLocator;
            _patientViewModelFactory = patientViewModelFactory;
            _regionManager = regionManager;
            _formViewModelFactory = formViewModelFactory;
        }

        public Common.ViewModels.MaternityRecordViewModel CreateInstance()
        {
            return new MaternityRecordViewModel(_formDefinitionLocator, _formContextFactory, _patientViewModelFactory,
                _regionManager, _formViewModelFactory);
        }
    }
}