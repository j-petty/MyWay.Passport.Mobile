using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.Behaviours
{
    /// <summary>
    /// Returns true if the value is an empty list
    /// </summary>
    public class IsEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null || ((IList)value)?.Count == 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
