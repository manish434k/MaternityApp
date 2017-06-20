using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using Symlconnect.Contracts.Environment;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel;

namespace Symlconnect.Maternity.Common.ViewModels
{
    /// <summary>
    /// ViewModel for the Patient Home View, including Maternity Record ViewModels.
    /// </summary>
    public class PatientHomeViewModel : PatientViewModel
    {
        private readonly IPatientLoader _patientLoader;
        private readonly IFactory<MaternityRecordViewModel> _maternityRecordViewModelFactory;
        private IList<IEntity> _maternityRecords;
        private IList<MaternityRecordViewModel> _maternityRecordViewModels;

        public PatientHomeViewModel(IPatientLoader patientLoader,
            ICurrentDateTimeProvider currentDateTimeProvider,
            IFactory<MaternityRecordViewModel> maternityRecordViewModelFactory) : base(currentDateTimeProvider)
        {
            _patientLoader = patientLoader;
            _maternityRecordViewModelFactory = maternityRecordViewModelFactory;
            EditMaternityRecordCommand = new DelegateCommand<IEntity>(OnEditMaternityRecord);
            NewMaternityRecordCommand = new DelegateCommand(OnNewMaternityRecord);
        }

        public DelegateCommand<IEntity> EditMaternityRecordCommand { get; }
        public DelegateCommand NewMaternityRecordCommand { get; }

        protected virtual void OnEditMaternityRecord(IEntity maternityRecord)
        {
            
        }

        private void OnNewMaternityRecord()
        {
            // TODO: Create new entity
        }

        protected override void OnPatientChanged(IPatient patient)
        {
            base.OnPatientChanged(patient);
            OnPropertyChanged(() => MaternityRecords);
            OnPropertyChanged(() => MaternityRecordViewModels);
        }

        public IList<IEntity> MaternityRecords => _maternityRecords ?? (_maternityRecords = LoadMaternityRecords());

        private IList<IEntity> LoadMaternityRecords()
        {
            if (Patient == null)
            {
                return null;
            }
            return _patientLoader.LoadPatientEntities(Patient, "record").ToList();
        }

        public IList<MaternityRecordViewModel> MaternityRecordViewModels
            => _maternityRecordViewModels ?? (_maternityRecordViewModels = CreateMaternityRecordViewModels());

        private IList<MaternityRecordViewModel> CreateMaternityRecordViewModels()
        {
            if (MaternityRecords == null)
            {
                return null;
            }

            var viewModels = new List<MaternityRecordViewModel>();
            foreach (IEntity maternityRecord in MaternityRecords)
            {
                var viewModel = _maternityRecordViewModelFactory.CreateInstance();
                viewModel.Entity = maternityRecord;
                viewModels.Add(viewModel);
            }
            return viewModels;
        }
    }
}