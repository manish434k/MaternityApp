using System.Collections.ObjectModel;

namespace Symlconnect.ViewModel
{
    public class ControlDefinitionCollection : KeyedCollection<string, IControlDefinition>
    {
        protected override string GetKeyForItem(IControlDefinition item)
        {
            return item.Id;
        }
    }
}