using BikeHub.Shared.Dto.Response;
using System.Globalization;

namespace BikeHub.Mobile.Converters
{
    class OrderStatusDisplayConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DropdownDto status && targetType == typeof(Color))
            {
                return status.Text switch
                {
                    "Processing" => Colors.Orange,
                    "Shipped" => Colors.YellowGreen,
                    "Delivered" => Colors.Green,
                    "Cancelled" => Colors.Red,
                    _ => Colors.Gray
                };
            }

            return Colors.Gray; // Default
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();

        
    }
}
