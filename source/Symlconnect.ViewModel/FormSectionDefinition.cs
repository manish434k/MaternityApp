using System;
using System.Collections;
using System.Linq;
using Symlconnect.Contracts.ObjectModel;
using Symlconnect.ViewModel.Media;

namespace Symlconnect.ViewModel
{
    public class FormSectionDefinition : IFormSectionDefinition, IChildItemContainer
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Color? BackgroundColor { get; set; }
        public Color? ForegroundColor { get; set; }

        private readonly Lazy<FormSectionDefinitionCollection> _childSectionDefinitions
            = new Lazy<FormSectionDefinitionCollection>(() => new FormSectionDefinitionCollection());

        public FormSectionDefinitionCollection ChildSectionDefinitions => _childSectionDefinitions.Value;

        private readonly Lazy<ControlDefinitionCollection> _controlDefinitions
            = new Lazy<ControlDefinitionCollection>(() => new ControlDefinitionCollection());

        public ControlDefinitionCollection ControlDefinitions => _controlDefinitions.Value;

        #region IChildItemContainer

        public bool IsSupportedChildItem(object item)
        {
            return item is IControlDefinition || item is IFormSectionDefinition;
        }

        public void AddChildItem(object item)
        {
            if (item is IControlDefinition)
            {
                ControlDefinitions.Add((IControlDefinition) item);
            }
            else if (item is IFormSectionDefinition)
            {
                ChildSectionDefinitions.Add((IFormSectionDefinition) item);
            }
        }

        public IEnumerable GetChildItems()
        {
            return ControlDefinitions.Concat<object>(ChildSectionDefinitions);
        }

        #endregion
    }
}