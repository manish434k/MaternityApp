using System;

namespace Symlconnect.ViewModel.ViewModels
{
    public interface IControlDefinitionViewModelFactoryLocator
    {
        IControlDefinitionViewModelFactory LocateFactory(Type controlDefinitionType);
    }
}