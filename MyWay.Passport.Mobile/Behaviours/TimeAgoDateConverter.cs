using System;
using System.Globalization;
using Humanizer;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.Behaviours
{
    public class TimeAgoDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            // Cast value to DateTime
            var dateTime = (DateTime)value;

            // Return human friendly date string
            return dateTime.Humanize(false);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
