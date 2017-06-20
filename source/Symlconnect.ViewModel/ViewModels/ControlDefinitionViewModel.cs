using System;
using System.Linq;
using Prism.Mvvm;
using Symlconnect.DataModel;

namespace Symlconnect.ViewModel.ViewModels
{
    public class ControlDefinitionViewModelBase<TControlDefinition> : BindableBase, IControlDefinitionViewModel,
        IControlDefinitionViewModel<TControlDefinition>
        where TControlDefinition : IControlDefinition
    {
        private IFormContext _formContext;

        IControlDefinition IControlDefinitionViewModel.ControlDefinition
        {
            get { return ControlDefinition; }
            set { ControlDefinition = (TControlDefinition)value; }
        }

        public TControlDefinition ControlDefinition { get; set; }

        public IFormContext FormContext
        {
            get { return _formContext; }
            set
            {
                var oldFormContext = _formContext;
                _formContext = value;
                OnFormContextChanged(oldFormContext, value);
                OnPropertyChanged(() => FormContext);
            }
        }

        private RuleDefinitionCollection _invalidRuleDefinitions;
        public RuleDefinitionCollection InvalidRuleDefinitions => _invalidRuleDefinitions;

        protected virtual void OnFormContextChanged(IFormContext oldFormContext, IFormContext newFormContext)
        {
            OnPropertyChanged(() => Value);
            OnPropertyChanged(() => IsVisible);
        }

        public virtual object Value
        {
            get
            {
                // ReSharper disable once MergeSequentialChecks - more readable with separate checks
                if (FormContext == null || FormContext.SessionContext == null || FormContext.Entity == null ||
                    ControlDefinition.ValuePropertyName == null)
                {
                    return null;
                }

                return FormContext.Entity.GetValue(ControlDefinition.ValuePropertyName, FormContext.SessionContext);
            }
            set
            {
                // ReSharper disable once MergeSequentialChecks - more readable with separate checks
                if (FormContext == null || FormContext.SessionContext == null || FormContext.Entity == null)
                {
                    throw new InvalidOperationException("Attempt to set value without a valid FormContext");
                }
                if (ControlDefinition.ValuePropertyName == null)
                {
                    throw new InvalidOperationException("Attempt to set value without a ValuePropertyName");
                }

                var result = FormContext.Entity.SetValue(ControlDefinition.ValuePropertyName, value, FormContext.SessionContext);

                if (result.InvalidRuleDefinitions != null && result.InvalidRuleDefinitions.Any())
                {
                    _invalidRuleDefinitions = result.InvalidRuleDefinitions;
                    OnPropertyChanged(() => InvalidRuleDefinitions);
                }
                else if (_invalidRuleDefinitions != null)
                {
                    _invalidRuleDefinitions = null;
                    OnPropertyChanged(() => InvalidRuleDefinitions);
                }

                if (result.IsSuccess)
                {
                    // ReSharper disable once UseNullPropagation - more readable
                    if (FormContext != null)
                    {
                        FormContext.ChangeEvent.Publish(new ControlDefinitionViewModelChange
                        {
                            ControlDefinitionViewModel = this,
                            PropertyName = nameof(Value),
                            NewValue = Value
                        });
                    }
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                // ReSharper disable once MergeSequentialChecks - more readable with separate checks
                if (FormContext == null || FormContext.SessionContext == null || FormContext.Entity == null ||
                    ControlDefinition.IsVisiblePropertyName == null)
                {
                    return true;
                }

                var value = FormContext.Entity.GetValue(ControlDefinition.IsVisiblePropertyName,
                    FormContext.SessionContext);
                if (value is string)
                {
                    value = Convert.ToBoolean(value);
                }
                return value != null && (bool)value;
            }
        }

        public object Width => !string.IsNullOrWhiteSpace(ControlDefinition.Width) ? Convert.ToDouble(ControlDefinition.Width) : 200;

        public object MinWidth => !string.IsNullOrWhiteSpace(ControlDefinition.Width) ? Convert.ToDouble(ControlDefinition.Width) : 200;

        public object Margin
            => !string.IsNullOrWhiteSpace(ControlDefinition.Margin) ? ControlDefinition.Margin : "0,0,0,0";

        public virtual bool IsPropertyNameReferenced(string entityName, string propertyName)
        {
            return PropertyDefinition.IsPropertyMatch(entityName, propertyName, ControlDefinition.ValuePropertyName)
                   || PropertyDefinition.IsPropertyMatch(entityName, propertyName, ControlDefinition.IsVisiblePropertyName);
        }

        public virtual void NotifyPropertyChanged(string entityName, string propertyName)
        {
            if (PropertyDefinition.IsPropertyMatch(entityName, propertyName, ControlDefinition.ValuePropertyName))
            {
                OnPropertyChanged(() => Value);
                // ReSharper disable once UseNullPropagation - more readable
                if (FormContext != null)
                {
                    FormContext.ChangeEvent.Publish(new ControlDefinitionViewModelChange
                    {
                        ControlDefinitionViewModel = this,
                        PropertyName = nameof(Value),
                        NewValue = Value
                    });
                }
            }
            if (PropertyDefinition.IsPropertyMatch(entityName, propertyName, ControlDefinition.IsVisiblePropertyName))
            {
                OnPropertyChanged(() => IsVisible);
            }
        }
    }
}