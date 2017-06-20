using System;
using System.Globalization;
using System.Windows;

namespace Symlconnect.Maternity.Wpf.Converters
{
    public class GreaterThanZeroToVisibilityConverter : BooleanConverter<Visibility>
    {
        public GreaterThanZeroToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed)
        {
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return base.Convert(value is int && (int) value > 0, targetType, parameter, culture);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}