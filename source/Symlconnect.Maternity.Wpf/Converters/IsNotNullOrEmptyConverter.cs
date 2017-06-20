using System;
using System.Globalization;
using System.Windows;

namespace Symlconnect.Maternity.Wpf.Converters
{
    public class IsNotNullOrEmptyToVisibilityConverter : BooleanConverter<Visibility>
    {
        public IsNotNullOrEmptyToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed)
        { }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string && (string)parameter == "invert")
            {
                True = Visibility.Collapsed;
                False = Visibility.Visible;
            }
            else
            {
                True = Visibility.Visible;
                False = Visibility.Collapsed;
            }
            return base.Convert(!string.IsNullOrEmpty(value as string), targetType, parameter, culture);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}