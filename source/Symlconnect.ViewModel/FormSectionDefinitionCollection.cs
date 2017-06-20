using System.Collections.ObjectModel;

namespace Symlconnect.ViewModel
{
    public class FormSectionDefinitionCollection : KeyedCollection<string, IFormSectionDefinition>
    {
        protected override string GetKeyForItem(IFormSectionDefinition item)
        {
            return item.Id;
        }
    }
}