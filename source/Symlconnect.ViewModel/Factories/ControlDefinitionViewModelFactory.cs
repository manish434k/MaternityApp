using System;
using Symlconnect.ViewModel.ViewModels;

namespace Symlconnect.ViewModel.Factories
{
    public class ControlDefinitionViewModelFactory<TControlDefinitionViewModel, TControlDefinition>
        : IControlDefinitionViewModelFactory
        where TControlDefinitionViewModel : IControlDefinitionViewModel, IControlDefinitionViewModel<TControlDefinition>,
        new()
        where TControlDefinition : IControlDefinition
    {
        public bool IsControlDefinitionTypeSupported(Type controlDefinitionType)
        {
            return controlDefinitionType == typeof(TControlDefinition);
        }

        public IControlDefinitionViewModel CreateViewModel()
        {
            return new TControlDefinitionViewModel();
        }
    }
}