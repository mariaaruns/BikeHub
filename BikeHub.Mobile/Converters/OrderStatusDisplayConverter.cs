using System.Globalization;

namespace BikeHub.Mobile.Converters
{
    class OrderStatusDisplayConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string status && targetType == typeof(Color))
            {
                return status switch
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
