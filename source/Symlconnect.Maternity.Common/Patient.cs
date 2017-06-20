using System;
using System.Diagnostics.CodeAnalysis;
using Prism.Mvvm;

namespace Symlconnect.Maternity.Common
{
    public class Patient : BindableBase, IPatient
    {
        private string _name;
        [ExcludeFromCodeCoverage] // Simple Property
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(() => Name);
                }
            }
        }

        private string _id;
        [ExcludeFromCodeCoverage] // Simple Property
        public string Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(() => Id);
                }
            }
        }

        private string _patientNumber;
        [ExcludeFromCodeCoverage] // Simple Property
        public string PatientNumber
        {
            get { return _patientNumber; }
            set
            {
                if (_patientNumber != value)
                {
                    _patientNumber = value;
                    OnPropertyChanged(() => PatientNumber);
                }
            }
        }
        
        private DateTime? _dateOfBirth;
        [ExcludeFromCodeCoverage] // Simple Property
        public DateTime? DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                if ((_dateOfBirth.HasValue != value.HasValue) || _dateOfBirth != value)
                {
                    _dateOfBirth = value;
                    OnPropertyChanged(() => DateOfBirth);
                }
            }
        }
    }
}