using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.Behaviours
{
    /// <summary>
    /// Used to return the negation/opposite of a boolean value.
    /// </summary>
    public class NegateBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
