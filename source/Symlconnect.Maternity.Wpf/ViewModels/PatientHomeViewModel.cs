using System;
using Prism.Commands;
using Prism.Regions;
using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel;
using Symlconnect.Maternity.Common;

namespace Symlconnect.Maternity.Wpf.ViewModels
{
    public class PatientHomeViewModel : Common.ViewModels.PatientHomeViewModel, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        public PatientHomeViewModel(IPatientLoader patientLoader,
            ICurrentDateTimeProvider currentDateTimeProvider,
            IRegionManager regionManager,
            IFactory<Common.ViewModels.MaternityRecordViewModel> maternityRecordViewModelFactory)
            : base(patientLoader, currentDateTimeProvider, maternityRecordViewModelFactory)
        {
            _regionManager = regionManager;
            NavigateBackCommand = new DelegateCommand(OnNavigateBack);
        }

        private void OnNavigateBack()
        {
            _regionManager.RequestNavigate("MainRegion", "Home", OnNavigateComplete);
        }

        private void OnNavigateComplete(NavigationResult obj)
        {
            if (obj.Error != null)
            {
                // TODO: Log navigation failures
            }
        }

        public DelegateCommand NavigateBackCommand { get; }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters["Patient"] is IPatient)
            {
                Patient = (IPatient) navigationContext.Parameters["Patient"];
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        protected override void OnEditMaternityRecord(IEntity maternityRecord)
        {
            if (maternityRecord == null)
            {
                return;
            }

            _regionManager.RequestNavigate("MainRegion", new Uri("MaternityRecord", UriKind.Relative),
                new NavigationParameters
                {
                    {"Patient", Patient},
                    {"MaternityRecord", maternityRecord}
                });
        }
    }
}