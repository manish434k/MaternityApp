using System;
using System.Collections;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.DataModel
{
    public class EntityDefinition : IEntityDefinition, IChildItemContainer
    {
        private readonly Lazy<PropertyDefinitionCollection> _properties =
            new Lazy<PropertyDefinitionCollection>(() => new PropertyDefinitionCollection());

        public PropertyDefinitionCollection PropertyDefinitions => _properties.Value;
        public string EntityName { get; internal set; }
        public IDataDictionary DataDictionary { get; internal set; }

        #region IChildItemContainer

        public bool IsSupportedChildItem(object item)
        {
            return item is IPropertyDefinition;
        }

        public void AddChildItem(object item)
        {
            if (item is IPropertyDefinition)
            {
                PropertyDefinitions.Add((IPropertyDefinition) item);
            }
        }

        public IEnumerable GetChildItems()
        {
            return PropertyDefinitions;
        }

        #endregion
    }
}