using System;
using System.Globalization;
using System.Windows.Data;

namespace SampleApp.Converters
{
    public class PhoneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            string phoneNo = value.ToString().Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Replace("-", string.Empty);

            return Convert(phoneNo);
        }

        public static string Convert(string number)
        {
            switch (number.Length)
            {
                case 8:
                    return $"{number.Substring(0, 4)}-{number.Substring(4, 4)}";
                case 9:
                    return $" {number.Substring(0, 1)} {number.Substring(1, 4)}-{number.Substring(5, 4)}";
                case 10:
                    return $"({number.Substring(0, 2)}) {number.Substring(2, 4)}-{number.Substring(6, 4)}";
                case 11:
                    return $"({number.Substring(0, 2)}) {number.Substring(2, 1)} {number.Substring(3, 4)}-{number.Substring(7, 4)}";
                case 13:
                    return $"{number.Substring(0, 2)} ({number.Substring(3, 2)}) {number.Substring(4, 1)} {number.Substring(5, 4)}-{number.Substring(9, 4)}";
                default:
                    return number;
            }
        }

        public static object ConvertBack(object value)
        {
            return value.ToString().Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Replace("-", string.Empty); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack(value);
        }
    }
}
