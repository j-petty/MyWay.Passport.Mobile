using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.Behaviours
{
    /// <summary>
    /// Returns true if the value is less than the double provided in the paramater.
    /// </summary>
    public class LessThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cutoff = (double)parameter;

            if (value.GetType() == typeof(double))
            {
                return (double)value < cutoff;
            }
            else
            {
                throw new NotImplementedException("Value of type {value.GetType()} is not supported");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private bool IsInteger (double number)
        {
            return number == Math.Round(number);
        }
    }
}
