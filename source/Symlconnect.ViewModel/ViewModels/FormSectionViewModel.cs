using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using Symlconnect.Contracts.Factories;

namespace Symlconnect.ViewModel.ViewModels
{
    public class FormSectionViewModel : BindableBase, IControlDefinitionViewModelContainer
    {
        private readonly IFactory<FormSectionViewModel> _formSectionViewModelFactory;
        private readonly IControlDefinitionViewModelFactoryLocator _controlDefinitionViewModelFactoryLocator;

        public FormSectionViewModel(IFactory<FormSectionViewModel> formSectionViewModelFactory,
            IControlDefinitionViewModelFactoryLocator controlDefinitionViewModelFactoryLocator)
        {
            _formSectionViewModelFactory = formSectionViewModelFactory;
            _controlDefinitionViewModelFactoryLocator = controlDefinitionViewModelFactoryLocator;
        }

        public IFormContext FormContext
        {
            get { return _formContext; }
            set
            {
                _formContext = value;
                if (_childFormSectionDefinitionViewModels != null)
                {
                    foreach (var formSectionViewModel in _childFormSectionDefinitionViewModels)
                    {
                        formSectionViewModel.FormContext = FormContext;
                    }
                }
                if (_controlDefinitionViewModels != null)
                {
                    foreach (var controlDefinitionViewModel in _controlDefinitionViewModels)
                    {
                        controlDefinitionViewModel.FormContext = FormContext;
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

        public IFormSectionDefinition FormSectionDefinition { get; internal set; }

        private IList<FormSectionViewModel> _childFormSectionDefinitionViewModels;

        public IList<FormSectionViewModel> ChildFormSectionViewModels => _childFormSectionDefinitionViewModels ??
                                                                         (_childFormSectionDefinitionViewModels =
                                                                             CreateChildFormSectionViewModels());

        private IList<IControlDefinitionViewModel> _controlDefinitionViewModels;
        private IFormContext _formContext;

        public IList<IControlDefinitionViewModel> ControlDefinitionViewModels
            => _controlDefinitionViewModels ?? (_controlDefinitionViewModels = CreateControlDefinitionViewModels());

        private IList<FormSectionViewModel> CreateChildFormSectionViewModels()
        {
            var viewModels = new List<FormSectionViewModel>();

            foreach (var sectionDefinition in FormSectionDefinition.ChildSectionDefinitions)
            {
                var viewModel = _formSectionViewModelFactory.CreateInstance();
                viewModel.FormSectionDefinition = sectionDefinition;
                viewModel.FormContext = FormContext;
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        private IList<IControlDefinitionViewModel> CreateControlDefinitionViewModels()
        {
            var viewModels = new List<IControlDefinitionViewModel>();

            foreach (var controlDefinition in FormSectionDefinition.ControlDefinitions)
            {
                var factory = _controlDefinitionViewModelFactoryLocator.LocateFactory(controlDefinition.GetType());
                if (factory != null)
                {
                    var viewModel = factory.CreateViewModel();
                    if (viewModel is IControlDefinitionViewModelContainer)
                    {
                        ((IControlDefinitionViewModelContainer) viewModel).ChildFactoryLocator =
                            _controlDefinitionViewModelFactoryLocator;
                    }
                    viewModel.ControlDefinition = controlDefinition;
                    viewModel.FormContext = FormContext;
                    viewModels.Add(viewModel);
                }
            }

            return viewModels;
        }

        public virtual IEnumerable<IControlDefinitionViewModel> FindControlDefinitionViewModels(string entityName,
            string propertyName)
        {
            return ChildFormSectionViewModels.SelectMany(
                    vm => vm.FindControlDefinitionViewModels(entityName, propertyName))
                .Concat(ControlDefinitionViewModels.Where(vm => vm.IsPropertyNameReferenced(entityName, propertyName)))
                .Concat(ControlDefinitionViewModels.OfType<IControlDefinitionViewModelContainer>()
                    .SelectMany(vm => vm.FindControlDefinitionViewModels(entityName, propertyName)));
        }

        public bool IsContained(IControlDefinitionViewModel changeControlDefinitionViewModel)
        {
            return ChildFormSectionViewModels
                       .SelectMany(vm => vm.ControlDefinitionViewModels)
                       .Concat(ControlDefinitionViewModels)
                       .Concat(ControlDefinitionViewModels.OfType<IControlDefinitionViewModelContainer>()
                           .SelectMany(vm => vm.ControlDefinitionViewModels))
                       .FirstOrDefault(vm => vm == changeControlDefinitionViewModel) != null;
        }

        public IEnumerable<IControlDefinitionViewModel> GetAllControlDefinitionViewModels()
        {
            return ChildFormSectionViewModels
                .SelectMany(vm => vm.GetAllControlDefinitionViewModels())
                .Concat(ControlDefinitionViewModels)
                .Concat(ControlDefinitionViewModels.OfType<IControlDefinitionViewModelContainer>()
                    .SelectMany(vm => vm.GetAllControlDefinitionViewModels()));
        }

        public IControlDefinitionViewModelFactoryLocator ChildFactoryLocator
        {
            get { return _controlDefinitionViewModelFactoryLocator; }
            set { throw new NotImplementedException(); }
        }

        public string Title => FormSectionDefinition.Title.Replace("%entitycreateddatetime%", FormContext.Entity.CreatedDateTime.ToString());
    }
}