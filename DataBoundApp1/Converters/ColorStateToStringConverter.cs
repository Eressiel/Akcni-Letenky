using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static DataBoundApp1.ViewModels.Colors;

namespace DataBoundApp1.Converters
{
    class ColorStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ColorState)Enum.Parse(typeof(ColorState), value.ToString(), true);
        }
    }
}
