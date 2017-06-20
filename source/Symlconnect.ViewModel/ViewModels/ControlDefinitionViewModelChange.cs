namespace Symlconnect.ViewModel.ViewModels
{
    public class ControlDefinitionViewModelChange
    {
        public IControlDefinitionViewModel ControlDefinitionViewModel { get; set; }
        public string PropertyName { get; set; }
        public object NewValue { get; set; }
    }
}