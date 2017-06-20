using System;

namespace Symlconnect.ViewModel.ViewModels
{
    public class YesNoControlDefinitionViewModel : ControlDefinitionViewModelBase<YesNoControlDefinition>
    {
        public string YesGroupName => ControlDefinition.Id + "_yes";
        public string NoGroupName => ControlDefinition.Id + "_no";

        protected override void OnPropertyChanged(string propertyName = null)
        {
            // ReSharper disable once ExplicitCallerInfoArgument - need to override this overload to catch any OnPropertyChanged calls
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(Value))
            {
                OnPropertyChanged(() => IsYes);
                OnPropertyChanged(() => IsNo);
            }
        }

        public bool IsYes
        {
            get
            {
                var value = Value;
                if (value is string)
                {
                    value = Convert.ToBoolean(value);
                }
                return value != null && ((bool) value);
            }
            set
            {
                if (value)
                {
                    Value = true;
                }
            }
        }

        public bool IsNo
        {
            get
            {
                var value = Value;
                if (value is string)
                {
                    value = Convert.ToBoolean(value);
                }
                return value != null && !((bool)value);
            }
            set
            {
                if (value)
                {
                    Value = false;
                }
            }
        }
    }
}