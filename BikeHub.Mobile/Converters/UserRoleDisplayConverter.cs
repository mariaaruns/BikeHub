using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.Converters
{
    public class UserRoleDisplayConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string roles && targetType == typeof(Color))
            {
                return roles switch
                {
                    "Admin" => Color.FromArgb("#047dff"),
                    "Manager" => Color.FromArgb("#4caf50"),
                    "Employee" => Color.FromArgb("#ff9800"),
                    _ => Colors.Black
                };
            }
            else
            {
                return Colors.Gray; // Default
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
