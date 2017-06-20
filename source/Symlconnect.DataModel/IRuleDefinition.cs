namespace Symlconnect.DataModel
{
    public interface IRuleDefinition
    {
        string Id { get; set; }
        RuleDefinitionSeverity Severity { get; set; }
        bool IsValidValue(object value);
        string Message { get; set; }
    }
}