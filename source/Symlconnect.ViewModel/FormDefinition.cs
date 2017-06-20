using Symlconnect.Contracts.ObjectModel;
using System;
using System.Collections;
using System.Linq;

namespace Symlconnect.ViewModel
{
    public class FormDefinition : IFormDefinition, IChildItemContainer
    {
        public string Id { get;  set; }
        public string DataDictionaryName { get;  set; }

        private readonly Lazy<ControlDefinitionCollection> _sharedControlDefinitions =
            new Lazy<ControlDefinitionCollection>(() => new ControlDefinitionCollection());

        private readonly Lazy<LookupDefinitionCollection> _lookupDefinitions
            = new Lazy<LookupDefinitionCollection>(() => new LookupDefinitionCollection());

        private readonly Lazy<FormSectionDefinitionCollection> _sectionDefinitions =
            new Lazy<FormSectionDefinitionCollection>(() => new FormSectionDefinitionCollection());

        public ControlDefinitionCollection SharedControlDefinitions => _sharedControlDefinitions.Value;
        public LookupDefinitionCollection LookupDefinitions => _lookupDefinitions.Value;
        public FormSectionDefinitionCollection SectionDefinitions => _sectionDefinitions.Value;

        #region IChildItemContainer

        public bool IsSupportedChildItem(object item)
        {
            return item is IControlDefinition || item is IFormSectionDefinition || item is LookupDefinition;
        }

        public void AddChildItem(object item)
        {
            if (item is IControlDefinition)
            {
                SharedControlDefinitions.Add((IControlDefinition) item);
            }
            else if (item is IFormSectionDefinition)
            {
                SectionDefinitions.Add((IFormSectionDefinition) item);
            } else if (item is LookupDefinition)
            {
                LookupDefinitions.Add((LookupDefinition) item);
            }
        }

        public IEnumerable GetChildItems()
        {
            return SharedControlDefinitions.Concat<object>(SectionDefinitions).Concat(LookupDefinitions);
        }

        #endregion
    }
}