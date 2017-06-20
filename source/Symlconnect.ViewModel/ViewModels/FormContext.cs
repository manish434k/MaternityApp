using System;
using Prism.Events;
using Symlconnect.Contracts.Factories;
using Symlconnect.DataModel;

namespace Symlconnect.ViewModel.ViewModels
{
    public class FormContext : IFormContext
    {
        public ISessionContext SessionContext { get; set; }
        public IEntity Entity { get; set; }
        public IFormDefinition FormDefinition { get; set; }
        public IFactory<FormViewModel> ChildFormViewModelFactory { get; set; }

        private readonly Lazy<PubSubEvent<ControlDefinitionViewModelChange>> _changeEvent
            =
            new Lazy<PubSubEvent<ControlDefinitionViewModelChange>>(
                () => new PubSubEvent<ControlDefinitionViewModelChange>());

        public PubSubEvent<ControlDefinitionViewModelChange> ChangeEvent => _changeEvent.Value;
    }
}