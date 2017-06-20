using Symlconnect.DataModel;

namespace Symlconnect.ViewModel.ViewModels
{
    public class AlertControlDefinitionViewModel : ControlDefinitionViewModelBase<AlertControlDefinition>
    {
        public string Content
        {
            get
            {
                string contentValue = null;
                if (!string.IsNullOrWhiteSpace(ControlDefinition.ContentPropertyName))
                {
                    contentValue = (string) FormContext.Entity.GetValue(ControlDefinition.ContentPropertyName,
                        FormContext.SessionContext);
                }
                if (!string.IsNullOrWhiteSpace(ControlDefinition.Content))
                {
                    if (contentValue != null)
                    {
                        contentValue += $"\n\n{ControlDefinition.Content}";
                    }
                    else
                    {
                        contentValue = ControlDefinition.Content;
                    }
                }
                return contentValue;
            }
        }

        public override bool IsPropertyNameReferenced(string entityName, string propertyName)
        {
            return base.IsPropertyNameReferenced(entityName, propertyName)
                   || PropertyDefinition.IsPropertyMatch(entityName, propertyName, ControlDefinition.ContentPropertyName);
        }

        public override void NotifyPropertyChanged(string entityName, string propertyName)
        {
            base.NotifyPropertyChanged(entityName, propertyName);

            if (PropertyDefinition.IsPropertyMatch(entityName, propertyName, ControlDefinition.ContentPropertyName))
            {
                OnPropertyChanged(() => Content);
            }
        }
    }
}