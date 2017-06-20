using System;
using System.Collections;
using System.Linq;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.DataModel
{
    public class DataDictionary : IDataDictionary, IChildItemContainer
    {
        public string Name { get; internal set; }

        private readonly Lazy<EntityDefinitionCollection> _entityDefinitions =
            new Lazy<EntityDefinitionCollection>(() => new EntityDefinitionCollection());

        private readonly Lazy<PropertyDefinitionCollection> _propertyDefinitions =
            new Lazy<PropertyDefinitionCollection>(() => new PropertyDefinitionCollection());

        private readonly Lazy<RuleDefinitionCollection> _ruleDefinitions = 
            new Lazy<RuleDefinitionCollection>(() => new RuleDefinitionCollection());

        public EntityDefinitionCollection EntityDefinitions => _entityDefinitions.Value;
        public PropertyDefinitionCollection PropertyDefinitions => _propertyDefinitions.Value;
        public RuleDefinitionCollection RuleDefinitions => _ruleDefinitions.Value;

        #region IChildItemContainer

        public bool IsSupportedChildItem(object item)
        {
            return item is IEntityDefinition
                   || item is IPropertyDefinition
                   || item is IRuleDefinition;
        }

        public void AddChildItem(object item)
        {
            if (item is IEntityDefinition)
            {
                EntityDefinitions.Add((IEntityDefinition) item);
            }
            else if (item is IPropertyDefinition)
            {
                PropertyDefinitions.Add((IPropertyDefinition) item);
            } else if (item is IRuleDefinition)
            {
                RuleDefinitions.Add((IRuleDefinition) item);
            }
        }

        public IEnumerable GetChildItems()
        {
            return EntityDefinitions.Concat<object>(PropertyDefinitions).Concat(RuleDefinitions);
        }

        #endregion
    }
}