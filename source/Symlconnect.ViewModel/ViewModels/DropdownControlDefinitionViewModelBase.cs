namespace Symlconnect.ViewModel.ViewModels
{
    public class DropdownControlDefinitionViewModelBase<TControlDefinition> :
        ControlDefinitionViewModelBase<TControlDefinition>
        where TControlDefinition : DropdownControlDefinition
    {
        public ILookupDefinition LookupDefinition
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ControlDefinition.LookupName)
                    && FormContext?.FormDefinition?.LookupDefinitions != null
                    && FormContext.FormDefinition.LookupDefinitions.Contains(ControlDefinition.LookupName))
                {
                    return FormContext.FormDefinition.LookupDefinitions[ControlDefinition.LookupName];
                }

                return null;
            }
        }
    }
}