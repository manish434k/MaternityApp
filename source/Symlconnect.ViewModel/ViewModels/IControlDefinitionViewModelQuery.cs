using System.Collections.Generic;

namespace Symlconnect.ViewModel.ViewModels
{
    public interface IControlDefinitionViewModelQuery
    {
        IEnumerable<IControlDefinitionViewModel> GetAllControlDefinitionViewModels();
    }
}