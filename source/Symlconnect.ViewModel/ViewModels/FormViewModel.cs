using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel;

namespace Symlconnect.ViewModel.ViewModels
{
    public class FormViewModel : BindableBase, IControlDefinitionViewModelQuery
    {
        private readonly IFactory<FormSectionViewModel> _formSectionViewModelFactory;

        public FormViewModel(IFactory<FormSectionViewModel> formSectionViewModelFactory)
        {
            _formSectionViewModelFactory = formSectionViewModelFactory;
        }

        private IFormDefinition _formDefinition;

        public IFormDefinition FormDefinition
        {
            get { return _formDefinition; }
            set
            {
                if (_formDefinition != value)
                {
                    _formDefinition = value;
                    OnPropertyChanged(() => FormDefinition);
                    _formSectionViewModels = null;
                    OnPropertyChanged(() => FormSectionViewModels);
                }
            }
        }

        public IFormContext FormContext
        {
            get { return _formContext; }
            set
            {
                if (_formContext != null)
                {
                    _formContext.Entity.EntityPropertyChanged -= OnEntityPropertyChanged;
                }
                _formContext = value;
                if (_formContext != null)
                {
                    _formContext.Entity.EntityPropertyChanged += OnEntityPropertyChanged;
                }
                if (_formSectionViewModels != null)
                {
                    foreach (var formSectionViewModel in _formSectionViewModels)
                    {
                        formSectionViewModel.FormContext = FormContext;
                    }
                }
                OnPropertyChanged(() => FormContext);
                _alertMonitorViewModel = null;
                OnPropertyChanged(() => AlertMonitorViewModel);
                _invalidRuleDefinitionsMonitorViewModel = null;
                OnPropertyChanged(() => AlertMonitorViewModel);
            }
        }

        private AlertControlDefinitionMonitorViewModel _alertMonitorViewModel;

        public AlertControlDefinitionMonitorViewModel AlertMonitorViewModel
        {
            get
            {
                if (_alertMonitorViewModel == null && FormContext != null)
                {
                    _alertMonitorViewModel = new AlertControlDefinitionMonitorViewModel();
                    _alertMonitorViewModel.Initialize(this, FormContext);
                }
                return _alertMonitorViewModel;
            }
        }

        private InvalidRuleDefinitionsControlDefinitionViewModel _invalidRuleDefinitionsMonitorViewModel;

        public InvalidRuleDefinitionsControlDefinitionViewModel InvalidRuleDefinitionsMonitorViewModel
        {
            get
            {
                if (_invalidRuleDefinitionsMonitorViewModel == null && FormContext != null)
                {
                    _invalidRuleDefinitionsMonitorViewModel = new InvalidRuleDefinitionsControlDefinitionViewModel();
                    _invalidRuleDefinitionsMonitorViewModel.Initialize(this, FormContext);
                }
                return _invalidRuleDefinitionsMonitorViewModel;
            }
        }

        private void OnEntityPropertyChanged(object sender, EntityPropertyChangedEventArgs e)
        {
            // Whenever an entity property changes, we nudge any ControlDefinitionViewModels that reference the property
            foreach (
                var controlDefinitionViewModel in
                FindControlDefinitionViewModels(e.Entity.EntityDefinition.EntityName, e.PropertyName))
            {
                controlDefinitionViewModel.NotifyPropertyChanged(e.Entity.EntityDefinition.EntityName, e.PropertyName);
            }
        }

        private IEnumerable<IControlDefinitionViewModel> FindControlDefinitionViewModels(string entityName,
            string propertyName)
        {
            return FormSectionViewModels.SelectMany(vm => vm.FindControlDefinitionViewModels(entityName, propertyName));
        }

        private IList<FormSectionViewModel> _formSectionViewModels;
        private IFormContext _formContext;

        public IList<FormSectionViewModel> FormSectionViewModels
            => _formSectionViewModels ?? (_formSectionViewModels = CreateFormSectionViewModels());

        private IList<FormSectionViewModel> CreateFormSectionViewModels()
        {
            if (FormDefinition == null)
            {
                return null;
            }

            var viewModels = new List<FormSectionViewModel>();

            foreach (var sectionDefinition in FormDefinition.SectionDefinitions)
            {
                var viewModel = _formSectionViewModelFactory.CreateInstance();
                viewModel.FormSectionDefinition = sectionDefinition;
                viewModel.FormContext = FormContext;
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public IEnumerable<IControlDefinitionViewModel> GetAllControlDefinitionViewModels()
        {
            return FormSectionViewModels.SelectMany(vm => vm.GetAllControlDefinitionViewModels());
        }
    }
}