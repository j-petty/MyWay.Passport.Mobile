using System;

namespace MyWay.Passport.Mobile.Models
{
    public class CardDetails : BaseModel
    {
        private string cardNumber;
        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; OnPropertyChanged(); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }

        private DateTime dateOfBirth;
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; OnPropertyChanged(); }
        }

        public CardDetails()
        {
            DateOfBirth = DateTime.Today;
        }
    }
}
