using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Symlconnect.ViewModel.ViewModels
{
    public abstract class ControlDefinitionMonitorViewModel<T, TViewModel> 
        where T : IControlDefinition
        where TViewModel : IControlDefinitionViewModel
    {
        private readonly Lazy<ObservableCollection<TViewModel>> _activeControlDefinitionViewModels
            = new Lazy<ObservableCollection<TViewModel>>(() => new ObservableCollection<TViewModel>());

        public ObservableCollection<TViewModel> ActiveControlDefinitionViewModels
            => _activeControlDefinitionViewModels.Value;

        public IControlDefinitionViewModelQuery Container;
        public IFormContext FormContext { get; set; }

        public void Initialize(IControlDefinitionViewModelQuery container, IFormContext formContext)
        {
            Container = container;
            FormContext = formContext;
            FormContext.ChangeEvent.Subscribe(OnControlDefinitionChange);

            // Get an initial active set
            var allControlDefinitionViewModels = container
                .GetAllControlDefinitionViewModels();
            var possibleViewModels = allControlDefinitionViewModels
                .OfType<TViewModel>()
                .Where(IsControlDefinitionIncluded);

            foreach (var viewModel in possibleViewModels)
            {
                ActiveControlDefinitionViewModels.Add(viewModel);
            }

            // ControlDefinitionViewModels publish events when values change
            FormContext.ChangeEvent.Subscribe(OnControlDefinitionChange);
        }

        private void OnControlDefinitionChange(ControlDefinitionViewModelChange change)
        {
            if (change.ControlDefinitionViewModel is TViewModel)
            {
                var viewModel = (TViewModel) change.ControlDefinitionViewModel;
                if (IsControlDefinitionIncluded(viewModel))
                {
                    if (Container != null)
                    {
                        if (Container.GetAllControlDefinitionViewModels()
                                .FirstOrDefault(vm => vm == change.ControlDefinitionViewModel) == null)
                        {
                            // ControlDefinitionViewModel not included in this section
                            return;
                        }
                    }

                    if (!ActiveControlDefinitionViewModels.Contains(viewModel))
                    {
                        ActiveControlDefinitionViewModels.Add(viewModel);
                    }
                }
                else if (ActiveControlDefinitionViewModels.Contains(viewModel))
                {
                    ActiveControlDefinitionViewModels.Remove(viewModel);
                }
            }
        }

        protected abstract bool IsControlDefinitionIncluded(TViewModel controlDefinitionViewModel);
    }
}