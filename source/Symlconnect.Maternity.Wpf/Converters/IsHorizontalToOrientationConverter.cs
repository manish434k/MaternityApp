using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Symlconnect.Maternity.Wpf.Converters
{
    public class IsHorizontalToOrientationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool) value)
            {
                return Orientation.Horizontal;
            }
            else
            {
                return Orientation.Vertical;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}