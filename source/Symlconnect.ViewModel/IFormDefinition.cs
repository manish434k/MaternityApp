namespace Symlconnect.ViewModel
{
    public interface IFormDefinition
    {
        string Id { get; set; }
        string DataDictionaryName { get; set; }
        ControlDefinitionCollection SharedControlDefinitions { get; }
        FormSectionDefinitionCollection SectionDefinitions { get; }
        LookupDefinitionCollection LookupDefinitions { get; }
    }
}