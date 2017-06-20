using Symlconnect.ViewModel.Media;

namespace Symlconnect.ViewModel
{
    public interface IFormSectionDefinition
    {
        string Id { get; set; }
        string Title { get; set; }
        Color? BackgroundColor { get; set; }
        Color? ForegroundColor { get; set; }
        FormSectionDefinitionCollection ChildSectionDefinitions { get; }
        ControlDefinitionCollection ControlDefinitions { get; }
    }
}