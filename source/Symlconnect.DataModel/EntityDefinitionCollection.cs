using System.Collections.ObjectModel;

namespace Symlconnect.DataModel
{
    public class EntityDefinitionCollection : KeyedCollection<string, IEntityDefinition>
    {
        protected override string GetKeyForItem(IEntityDefinition item)
        {
            return item.EntityName;
        }
    }
}