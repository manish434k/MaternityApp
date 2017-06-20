using System.Collections.ObjectModel;

namespace Symlconnect.ViewModel
{
    public class LookupDefinitionCollection : KeyedCollection<string, ILookupDefinition>
    {
        protected override string GetKeyForItem(ILookupDefinition item)
        {
            return item.Id;
        }
    }
}