namespace Symlconnect.DataModel.Rules
{
    public class RegExRuleDefinition : IRuleDefinition
    {
        public string Id { get; set; }
        public RuleDefinitionSeverity Severity { get; set; }
        public string Pattern { get; set; }

        public bool IsValidValue(object value)
        {
            return !(value is string) || System.Text.RegularExpressions.Regex.IsMatch((string) value, Pattern);
        }

        public string Message { get; set; }
    }
}