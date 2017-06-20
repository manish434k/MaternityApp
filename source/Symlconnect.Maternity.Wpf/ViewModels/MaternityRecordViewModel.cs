using Prism.Commands;
using Prism.Regions;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel;
using Symlconnect.Maternity.Common;
using Symlconnect.Maternity.Common.ViewModels;
using Symlconnect.ViewModel;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.Maternity.Wpf.ViewModels
{
    public class MaternityRecordViewModel : Common.ViewModels.MaternityRecordViewModel, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        public MaternityRecordViewModel(IFormDefinitionLocator formDefinitionLocator,
            IFactory<IFormContext> formContextFactory, IFactory<PatientViewModel> patientViewModelFactory,
            IRegionManager regionManager, IFactory<FormViewModel> formViewModelFactory)
            : base(formDefinitionLocator, formContextFactory, patientViewModelFactory, formViewModelFactory)
        {
            _regionManager = regionManager;
            NavigateBackCommand = new DelegateCommand(OnNavigateBack);
        }

        public DelegateCommand NavigateBackCommand { get; }

        private void OnNavigateBack()
        {
            _regionManager.RequestNavigate("MainRegion", "PatientHome", new NavigationParameters {{"Patient", Patient}});
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters["Patient"] is IPatient &&
                navigationContext.Parameters["MaternityRecord"] is IEntity)
            {
                Patient = (IPatient) navigationContext.Parameters["Patient"];
                Entity = (IEntity) navigationContext.Parameters["MaternityRecord"];
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}