using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.Behaviours
{
    /// <summary>
    /// Returns true if the date aspect of a DateTime value is today's date.
    /// </summary>
    public class IsTodayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.GetType() == typeof(DateTime))
            {
                return ((DateTime)value).Date == DateTime.Today;
            }
            else
            {
                throw new InvalidOperationException($"Value of type {value.GetType()} is not supported");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
