using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Symlconnect.Contracts.ObjectModel;

namespace Symlconnect.ViewModel
{
    public class StackControlDefinition : ControlDefinitionBase, IChildItemContainer
    {
        [ExcludeFromCodeCoverage] // Simple Property
        public bool IsHorizontal { get; internal set; }

        private readonly Lazy<ControlDefinitionCollection> _childControlDefinitions
            = new Lazy<ControlDefinitionCollection>(() => new ControlDefinitionCollection());

        public ControlDefinitionCollection ChildControlDefinitions => _childControlDefinitions.Value;

        #region IChildItemContainer

        public bool IsSupportedChildItem(object item)
        {
            return item is IControlDefinition;
        }

        public void AddChildItem(object item)
        {
            if (item is IControlDefinition)
            {
                ChildControlDefinitions.Add((IControlDefinition) item);
            }
        }

        public IEnumerable GetChildItems()
        {
            return ChildControlDefinitions;
        }

        #endregion
    }
}