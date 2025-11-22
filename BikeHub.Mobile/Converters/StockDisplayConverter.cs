using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.Converters
{
    class StockDisplayConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int stock)
            {
                // If the target property is text
                if (targetType == typeof(string))
                {
                    return stock < 10 ? $"Low stock ({stock})" : $"In stock ({stock})";
                }

                // If the target property is color
                if (targetType == typeof(Color))
                {
                    return stock < 10 ? Colors.Red : Colors.Green;
                }
            }

            // Default values
            if (targetType == typeof(string))
                return "N/A";
            if (targetType == typeof(Color))
                return Colors.Gray;

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
