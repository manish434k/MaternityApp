using System;
using Prism.Commands;
using Symlconnect.DataModel;

namespace Symlconnect.ViewModel.ViewModels
{
    public class AuditControlDefinitionViewModel : ControlDefinitionViewModelBase<AuditControlDefinition>
    {
        public DelegateCommand ConfirmCommand { get; }

        public AuditControlDefinitionViewModel()
        {
            ConfirmCommand = new DelegateCommand(OnConfirmCommand);
        }

        private AuditPropertyDefinition PropertyDefinition
        {
            get
            {
                if (FormContext != null
                    && ControlDefinition?.ValuePropertyName != null
                    &&
                    FormContext.Entity.EntityDefinition.PropertyDefinitions.Contains(ControlDefinition.ValuePropertyName))
                {
                    return
                        FormContext.Entity.EntityDefinition.PropertyDefinitions[ControlDefinition.ValuePropertyName] as
                            AuditPropertyDefinition;
                }

                return null;
            }
        }

        public string AuditUserDisplayName
        {
            get
            {
                var value = FormContext.Entity.GetValue(PropertyDefinition?.UserIdPropertyDefinition.Name,
                    FormContext.SessionContext);
                return value as string;
            }
        }

        public string AuditDateTime
        {
            get
            {
                var value = FormContext.Entity.GetValue(PropertyDefinition?.ChangeDateTimePropertyDefinition.Name,
                    FormContext.SessionContext);
                // ReSharper disable once UseNullPropagation - readability
                if (value is DateTime)
                {
                    return ((DateTime) value).ToString();
                }

                return null;
            }
        }

        private void OnConfirmCommand()
        {
            Value = true;
            OnPropertyChanged(() => Value);
            OnPropertyChanged(() => AuditDateTime);
            OnPropertyChanged(() => AuditUserDisplayName);
        }
    }
}