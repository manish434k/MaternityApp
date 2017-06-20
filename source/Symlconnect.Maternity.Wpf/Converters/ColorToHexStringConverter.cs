using System;
using System.Globalization;
using System.Windows.Data;
using Symlconnect.ViewModel.Media;

namespace Symlconnect.Maternity.Wpf.Converters
{
    public class ColorToHexStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ReSharper disable once MergeConditionalExpression - prefer for readability
            return value is Color ? ((Color) value).ToHexString() : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}