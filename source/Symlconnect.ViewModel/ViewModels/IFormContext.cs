using Prism.Events;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel;

namespace Symlconnect.ViewModel.ViewModels
{
    public interface IFormContext
    {
        ISessionContext SessionContext { get; set; }
        IEntity Entity { get; set; }
        IFormDefinition FormDefinition { get; set; }
        IFactory<FormViewModel> ChildFormViewModelFactory { get; set; }
        PubSubEvent<ControlDefinitionViewModelChange> ChangeEvent { get; }
    }
}