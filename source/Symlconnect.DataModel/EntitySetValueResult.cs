namespace Symlconnect.DataModel
{
    public class EntitySetValueResult
    {
        /// <summary>
        /// Returns true if the SetValue operation succeeded. If false, check InvalidRuleDefinitions for rule failures.
        /// </summary>
        public bool IsSuccess { get; internal set; }
        /// <summary>
        /// A collection of RuleDefinitions that failed to validate for the new value.
        /// </summary>
        public RuleDefinitionCollection InvalidRuleDefinitions { get; internal set; }
        public EntityPropertyValueChangeset ChangeSet { get; set; }
    }
}