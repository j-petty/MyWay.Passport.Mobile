using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.Behaviours
{
    /// <summary>
    /// Returns true if the value is less than the double provided in the paramater.
    /// </summary>
    public class IsLessThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            var cutoff = (double)parameter;

            if (value.GetType() == typeof(double))
            {
                return (double)value < cutoff;
            }
            else if (value.GetType() == typeof(int))
            {
                return (int)value < cutoff;
            }
            else
            {
                throw new NotImplementedException($"Value of type {value.GetType()} is not supported");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
