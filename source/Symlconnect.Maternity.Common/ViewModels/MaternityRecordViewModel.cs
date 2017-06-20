using System;
using System.Diagnostics.CodeAnalysis;
using Prism.Mvvm;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel;
using Symlconnect.ViewModel;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.Maternity.Common.ViewModels
{
    public class MaternityRecordViewModel : BindableBase
    {
        private readonly IFormDefinitionLocator _formDefinitionLocator;
        private readonly IFactory<IFormContext> _formContextFactory;
        private readonly IFactory<PatientViewModel> _patientViewModelFactory;
        private readonly IFactory<FormViewModel> _formViewModelFactory;

        public MaternityRecordViewModel(IFormDefinitionLocator formDefinitionLocator,
            IFactory<IFormContext> formContextFactory,
            IFactory<PatientViewModel> patientViewModelFactory,
            IFactory<FormViewModel> formViewModelFactory)
        {
            _formDefinitionLocator = formDefinitionLocator;
            _formContextFactory = formContextFactory;
            _patientViewModelFactory = patientViewModelFactory;
            _formViewModelFactory = formViewModelFactory;
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
                    OnPropertyChanged(() => Patient);
                    _patientViewModel = null;
                    OnPropertyChanged(() => PatientViewModel);
                }
            }
        }

        private IEntity _entity;

        /// <summary>
        ///     The Maternity Record - an Entity instance.
        /// </summary>
        [ExcludeFromCodeCoverage] // Simple Property
        public IEntity Entity
        {
            get { return _entity; }
            set
            {
                if (_entity != value)
                {
                    _entity = value;
                    OnPropertyChanged(() => Entity);
                    _formDefinition = null;
                    _formViewModel = null;
                    OnPropertyChanged(() => FormDefinition);
                    OnPropertyChanged(() => FormViewModel);
                    OnPropertyChanged(() => Caption);
                }
            }
        }

        private IFormDefinition _formDefinition;

        public IFormDefinition FormDefinition
            => _formDefinition ?? (_formDefinition = _formDefinitionLocator.GetFormDefinition("maternity"));

        private FormViewModel _formViewModel;
        private PatientViewModel _patientViewModel;

        public FormViewModel FormViewModel
            => _formViewModel ?? (_formViewModel = CreateFormViewModel());

        private FormViewModel CreateFormViewModel()
        {
            var viewModel = _formViewModelFactory.CreateInstance();
            viewModel.FormContext = CreateFormContext();
            viewModel.FormDefinition = FormDefinition;
            return viewModel;
        }

        public PatientViewModel PatientViewModel => _patientViewModel ?? (_patientViewModel = CreatePatientViewModel());

        private PatientViewModel CreatePatientViewModel()
        {
            var viewModel = _patientViewModelFactory.CreateInstance();
            viewModel.Patient = Patient;
            return viewModel;
        }

        public string Caption => $"{Entity?.CreatedDateTime}, {Entity?.CreatedByUserDisplayName}";

        private IFormContext CreateFormContext()
        {
            if (Entity == null)
            {
                return null;
            }

            var formContext = _formContextFactory.CreateInstance();
            formContext.FormDefinition = FormDefinition;
            formContext.Entity = Entity;
            formContext.ChildFormViewModelFactory = _formViewModelFactory;
            formContext.SessionContext = new SessionContext
            {
                SessionDateTime = Entity.CreatedDateTime,
                SessionId = Guid.NewGuid().ToString(),
                SessionUser = new User {DisplayName = Entity.CreatedByUserDisplayName, UserId = Entity.CreatedByUserId}
            };
            return formContext;
        }
    }
}