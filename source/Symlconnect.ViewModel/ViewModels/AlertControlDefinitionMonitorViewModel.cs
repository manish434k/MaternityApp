using System;

namespace Symlconnect.ViewModel.ViewModels
{
    public class AlertControlDefinitionMonitorViewModel :
        ControlDefinitionMonitorViewModel<AlertControlDefinition, AlertControlDefinitionViewModel>
    {
        protected override bool IsControlDefinitionIncluded(AlertControlDefinitionViewModel controlDefinitionViewModel)
        {
            var value = controlDefinitionViewModel.Value;
            if (value is string)
            {
                value = Convert.ToBoolean(value);
            }
            else if (value == null)
            {
                value = false;
            }
            return (bool) value;
        }
    }
}