using System.Collections.Generic;

namespace Symlconnect.ViewModel.ViewModels
{
    public interface IControlDefinitionViewModelContainer : IControlDefinitionViewModelQuery
    {
        IControlDefinitionViewModelFactoryLocator ChildFactoryLocator { get; set; } 
        IList<IControlDefinitionViewModel> ControlDefinitionViewModels { get; }
        IEnumerable<IControlDefinitionViewModel> FindControlDefinitionViewModels(string entityName, string propertyName);
    }
}