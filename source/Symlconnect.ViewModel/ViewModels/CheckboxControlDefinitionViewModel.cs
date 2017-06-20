namespace Symlconnect.ViewModel.ViewModels
{
    public class CheckboxControlDefinitionViewModel : ControlDefinitionViewModelBase<CheckboxControlDefinition>
    {
        public override object Value
        {
            get
            {
                return base.Value ?? false;
            }
            set { base.Value = value; }
        }
    }
}