namespace Symlconnect.ViewModel.ViewModels
{
    public class InvalidRuleDefinitionsControlDefinitionViewModel :
        ControlDefinitionMonitorViewModel<IControlDefinition, IControlDefinitionViewModel>
    {
        protected override bool IsControlDefinitionIncluded(IControlDefinitionViewModel controlDefinitionViewModel)
        {
            // Will come back to this once the rules framework is more rounded out
            return controlDefinitionViewModel.InvalidRuleDefinitions != null; 
            // && controlDefinitionViewModel.InvalidRuleDefinitions.Any(rule => rule.Severity == RuleDefinitionSeverity.Error);
        }
    }
}