using System;
namespace MyWay.Passport.Mobile.Models
{
    public class RecentTrip : BaseModel
    {
        private string from;
        public string From
        {
            get { return from; }
            set { from = value; OnPropertyChanged(); }
        }

        private string to;
        public string To
        {
            get { return to; }
            set { to = value; OnPropertyChanged(); }
        }

        private string route;
        public string Route
        {
            get { return route; }
            set { route = value; OnPropertyChanged(); }
        }

        private DateTime? date;
        public DateTime? Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged(); }
        }

        private double? price;
        public double? Price
        {
            get { return price; }
            set { price = value; OnPropertyChanged(); }
        }
    }
}
