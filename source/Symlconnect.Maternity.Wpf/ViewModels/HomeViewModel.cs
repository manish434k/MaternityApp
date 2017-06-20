using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Symlconnect.Contracts.Factories;
using Symlconnect.Maternity.Common;
using Symlconnect.Maternity.Common.ViewModels;

namespace Symlconnect.Maternity.Wpf.ViewModels
{
    /// <summary>
    /// Application Home Page ViewModel, including a filterable list of Patients.
    /// </summary>
    public class HomeViewModel : BindableBase
    {
        private readonly IPatientLoader _patientLoader;
        private readonly IFactory<PatientViewModel> _patientViewModelFactory;
        private readonly IRegionManager _regionManager;

        public HomeViewModel(IPatientLoader patientLoader, IFactory<PatientViewModel> patientViewModelFactory,
            IRegionManager regionManager)
        {
            _patientLoader = patientLoader;
            _patientViewModelFactory = patientViewModelFactory;
            _regionManager = regionManager;

            NavigateToPatientHomeCommand = new DelegateCommand<IPatient>(OnNavigateToPatientHomeCommand);
        }

        private IList<PatientViewModel> _patientViewModels;
        private string _filterText;

        public DelegateCommand<IPatient> NavigateToPatientHomeCommand { get; }

        public IList<PatientViewModel> PatientViewModels
            => _patientViewModels ?? (_patientViewModels = CreatePatientViewModels());

        private IList<PatientViewModel> CreatePatientViewModels()
        {
            var patientViewModels = new List<PatientViewModel>();
            foreach (var patient in _patientLoader.LoadPatients())
            {
                var viewModel = _patientViewModelFactory.CreateInstance();
                viewModel.Patient = patient;
                patientViewModels.Add(viewModel);
            }
            return patientViewModels;
        }

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    OnPropertyChanged(() => FilterText);
                    OnPropertyChanged(() => HasFilteredPatientViewModels);
                    OnPropertyChanged(() => FilteredPatientViewModels);
                }
            }
        }

        public bool HasFilteredPatientViewModels => FilteredPatientViewModels.Any();

        public IEnumerable<PatientViewModel> FilteredPatientViewModels
            =>
                PatientViewModels.OrderBy(vm => vm.Patient.Name)
                    .Where(
                        vm =>
                            string.IsNullOrWhiteSpace(FilterText) ||
                            vm.Patient.Name.IndexOf(FilterText, StringComparison.InvariantCultureIgnoreCase) != -1);

        private void OnNavigateToPatientHomeCommand(IPatient patient)
        {
            _regionManager.RequestNavigate("MainRegion", new Uri("PatientHome", UriKind.Relative), OnNavigationComplete,
                new NavigationParameters {{"Patient", patient}});
        }

        private void OnNavigationComplete(NavigationResult obj)
        {
        }
    }
}