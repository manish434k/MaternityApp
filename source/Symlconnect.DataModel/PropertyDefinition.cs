using System;
using System.Collections;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.DataModel
{
    /// <summary>
    ///     The base Property Definition type. These properties are read directly from and saved directly to the store.
    /// </summary>
    public class PropertyDefinition : IPropertyDefinition, IChildItemContainer
    {
        public string Name { get; set; }
        public virtual ValueKind PropertyDefinitionKind { get; set; }

        private readonly Lazy<RuleDefinitionCollection> _ruleDefinitions
            = new Lazy<RuleDefinitionCollection>(() => new RuleDefinitionCollection());

        public RuleDefinitionCollection RuleDefinitions => _ruleDefinitions.Value;

        public virtual void TargetToSource(IEntity entity, EntityPropertyValueChangeset changeset)
        {
            if (_ruleDefinitions.IsValueCreated)
            {
                foreach (var ruleDefinition in RuleDefinitions)
                {
                    if (!ruleDefinition.IsValidValue(changeset.Changes[0].NewValue))
                    {
                        if (!changeset.InvalidRuleDefinitions.Contains(ruleDefinition.Id))
                        {
                            changeset.InvalidRuleDefinitions.Add(ruleDefinition);
                        }
                    }
                }
            }
        }

        public virtual object SourceToTarget(IEntity entity, object value, ISessionContext sessionContext)
        {
            // No processing for basic properties
            return value;
        }

        #region IChildItemContainer

        bool IChildItemContainer.IsSupportedChildItem(object item)
        {
            return item is IRuleDefinition;
        }

        void IChildItemContainer.AddChildItem(object item)
        {
            if (item is IRuleDefinition)
            {
                RuleDefinitions.Add((IRuleDefinition)item);
            }
        }

        IEnumerable IChildItemContainer.GetChildItems()
        {
            return RuleDefinitions;
        }

        #endregion

        public static bool IsPropertyMatch(string entityName, string propertyName,
            string possiblePropertyNameReference)
        {
            if (!String.IsNullOrWhiteSpace(possiblePropertyNameReference))
            {
                string fullPropertyName = $"{entityName}.{propertyName}";
                return possiblePropertyNameReference == propertyName ||
                       possiblePropertyNameReference == fullPropertyName;
            }
            return false;
        }
    }
}