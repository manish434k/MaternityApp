namespace Symlconnect.ViewModel
{
    public interface IControlDefinition
    {
        string Id { get; set; }
        string Caption { get; set; }
        string ValuePropertyName { get; set; }
        string IsVisiblePropertyName { get; set; }
        string Margin { get; set; }
        string Width { get; set; }
    }
}