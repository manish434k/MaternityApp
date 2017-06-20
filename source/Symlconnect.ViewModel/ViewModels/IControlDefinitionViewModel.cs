using Symlconnect.DataModel;

namespace Symlconnect.ViewModel.ViewModels
{
    public interface IControlDefinitionViewModel
    {
        IControlDefinition ControlDefinition { get; set; }
        object Value { get; set; }
        bool IsVisible { get; }
        bool IsPropertyNameReferenced(string entityName, string propertyName);
        void NotifyPropertyChanged(string entityName, string propertyName);
        IFormContext FormContext { get; set; }
        RuleDefinitionCollection InvalidRuleDefinitions { get; }
    }

    public interface IControlDefinitionViewModel<TControlDefinition>
        where TControlDefinition : IControlDefinition
    {
        TControlDefinition ControlDefinition { get; set; }
    }
}