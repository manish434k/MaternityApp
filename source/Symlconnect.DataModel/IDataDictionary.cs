namespace Symlconnect.DataModel
{
    public interface IDataDictionary
    {
        string Name { get; }
        EntityDefinitionCollection EntityDefinitions { get; }
        PropertyDefinitionCollection PropertyDefinitions { get; }
        RuleDefinitionCollection RuleDefinitions { get; }
    }
}