using System;

namespace Symlconnect.ViewModel.ViewModels
{
    public interface IControlDefinitionViewModelFactory
    {
        bool IsControlDefinitionTypeSupported(Type controlDefinitionType);
        IControlDefinitionViewModel CreateViewModel();
    }
}