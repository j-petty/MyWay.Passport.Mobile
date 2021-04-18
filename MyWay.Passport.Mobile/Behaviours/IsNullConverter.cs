using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.Behaviours
{
    /// <summary>
    /// Returns true if the value is null;
    /// </summary>
    public class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
