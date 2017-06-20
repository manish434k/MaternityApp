namespace Symlconnect.ViewModel
{
    public abstract class ControlDefinitionBase : IControlDefinition
    {
        public string Id { get; set; }
        public string Caption { get; set; }
        public string ValuePropertyName { get;  set; }
        public string IsVisiblePropertyName { get;  set; }
        public string Margin { get; set; }
        public string Width { get; set; }
    }
}