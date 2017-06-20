using Prism.Mvvm;
using Symlconnect.Common;
using Symlconnect.Contracts.Environment;

namespace Symlconnect.Maternity.Common.ViewModels
{
    /// <summary>
    /// Base Patient ViewModel. This is the ViewModel that sits behind the Patient List and doesn't contain the child Maternity Record ViewModels etc.
    /// </summary>
    public class PatientViewModel : BindableBase
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public PatientViewModel(ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        private IPatient _patient;

        public IPatient Patient
        {
            get { return _patient; }
            set
            {
                if (_patient != value)
                {
                    _patient = value;
                    OnPatientChanged(value);
                }
            }
        }

        protected virtual void OnPatientChanged(IPatient patient)
        {
            OnPropertyChanged(() => Patient);
            OnPropertyChanged(() => Age);
            OnPropertyChanged(() => Caption);
        }

        public string Age
        {
            get
            {
                if (!Patient.DateOfBirth.HasValue)
                {
                    return "(uknown)";
                }
                return DateTimeHelpers.CalculateAge(Patient.DateOfBirth.Value,_currentDateTimeProvider.GetCurrentDateTime()).ToString();
            }
        }

        public string Caption => Patient != null ? $"{Patient.Name}, No. {Patient.PatientNumber}, {Age} years" : null;
    }
}