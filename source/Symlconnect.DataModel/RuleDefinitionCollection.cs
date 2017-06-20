using System.Collections.ObjectModel;

namespace Symlconnect.DataModel
{
    public class RuleDefinitionCollection : KeyedCollection<string, IRuleDefinition>
    {
        protected override string GetKeyForItem(IRuleDefinition item)
        {
            return item.Id;
        }
    }
}