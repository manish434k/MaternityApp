using System.Collections.ObjectModel;

namespace Symlconnect.DataModel
{
    public class PropertyDefinitionCollection : KeyedCollection<string, IPropertyDefinition>
    {
        protected override string GetKeyForItem(IPropertyDefinition item)
        {
            return item.Name;
        }
    }
}