using System.Collections.Generic;
using System.Linq;

namespace Symlconnect.ViewModel.ViewModels
{
    public class StackControlDefinitionViewModel : ControlDefinitionViewModelBase<StackControlDefinition>,
        IControlDefinitionViewModelContainer
    {
        private IList<IControlDefinitionViewModel> _controlDefinitionViewModels;

        public IList<IControlDefinitionViewModel> ControlDefinitionViewModels
            => _controlDefinitionViewModels ?? (_controlDefinitionViewModels = CreateControlDefinitionViewModels());

        protected override void OnFormContextChanged(IFormContext oldFormContext, IFormContext newFormContext)
        {
            base.OnFormContextChanged(oldFormContext, newFormContext);

            if (_controlDefinitionViewModels != null)
            {
                foreach (var controlDefinitionViewModel in _controlDefinitionViewModels)
                {
                    controlDefinitionViewModel.FormContext = FormContext;
                }
            }
        }

        private IList<IControlDefinitionViewModel> CreateControlDefinitionViewModels()
        {
            var viewModels = new List<IControlDefinitionViewModel>();

            foreach (var controlDefinition in ControlDefinition.ChildControlDefinitions)
            {
                var factory = ChildFactoryLocator.LocateFactory(controlDefinition.GetType());
                if (factory != null)
                {
                    var viewModel = factory.CreateViewModel();
                    if (viewModel is IControlDefinitionViewModelContainer)
                    {
                        ((IControlDefinitionViewModelContainer)viewModel).ChildFactoryLocator =
                            ChildFactoryLocator;
                    }
                    viewModel.ControlDefinition = controlDefinition;
                    viewModel.FormContext = FormContext;
                    viewModels.Add(viewModel);
                }
            }

            return viewModels;
        }

        IEnumerable<IControlDefinitionViewModel> IControlDefinitionViewModelContainer.FindControlDefinitionViewModels(
            string entityName, string propertyName)
        {
            return ControlDefinitionViewModels.Where(vm => vm.IsPropertyNameReferenced(entityName, propertyName))
                .Concat(ControlDefinitionViewModels.OfType<IControlDefinitionViewModelContainer>()
                    .SelectMany(vm => vm.FindControlDefinitionViewModels(entityName, propertyName)));
        }

        public IControlDefinitionViewModelFactoryLocator ChildFactoryLocator { get; set; }

        public IEnumerable<IControlDefinitionViewModel> GetAllControlDefinitionViewModels()
        {
            return ControlDefinitionViewModels
                .Concat(ControlDefinitionViewModels.OfType<IControlDefinitionViewModelQuery>()
                    .SelectMany(vm => vm.GetAllControlDefinitionViewModels()));
        }
    }
}