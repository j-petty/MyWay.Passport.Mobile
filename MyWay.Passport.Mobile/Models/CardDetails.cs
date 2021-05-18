using System;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.Models
{
    public class CardDetails : BaseModel
    {
        // NOTE: this is used to track card uniqueness only
        private string cardId = Guid.NewGuid().ToString();
        public string CardId
        {
            get { return cardId; }
            set { cardId = value; OnPropertyChanged(); }
        }

        private string cardName;
        public string CardName
        {
            get { return cardName; }
            set { cardName = value; OnPropertyChanged(); }
        }

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

        private DateTime? dateOfBirth = new DateTime(1990, 1, 1);
        public DateTime? DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; OnPropertyChanged(); }
        }

        private DateTime? lastUpdated;
        public DateTime? LastUpdated
        {
            get { return lastUpdated; }
            set { lastUpdated = value; OnPropertyChanged(); }
        }

        private double? lastBalance = 0.0;
        public double? LastBalance
        {
            get { return lastBalance; }
            set { lastBalance = value; OnPropertyChanged(); }
        }

        private ColourNames? cardColourName = null;
        public ColourNames? CardColourName
        {
            get { return cardColourName; }
            set
            {
                cardColourName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CardColour));
            }
        }

        [JsonIgnore]
        public CardColour CardColour
        {
            get
            {
                return GetColour();
            }
        }

        /// <summary>
        /// Checks if required CardDetails have been completed.
        /// </summary>
        /// <returns>Returns whether required card details are provided.</returns>
        public bool CheckFilled()
        {
            return !string.IsNullOrEmpty(CardNumber) && !string.IsNullOrEmpty(Password) && DateOfBirth != null && DateOfBirth <= DateTime.Today;
        }

        /// <summary>
        /// Returns a CardColour object which correlates to the provided CardColourName.
        /// </summary>
        /// <returns>Returns CardColour object if one exists.</returns>
        private CardColour GetColour()
        {
            // Return null if CardColour isn't set (will default to dynamic colour)
            if (CardColourName == null)
            {
                var resourceDictionary = App.Current.Resources.MergedDictionaries.FirstOrDefault();

                // Return default CardColour
                return new CardColour
                {
                    BackgroundColour = ((Color)resourceDictionary?["DefaultCardBackgroundColor"]).ToHex(),
                    TextColour = ((Color)resourceDictionary?["DefaultCardTextColor"]).ToHex()
                };
            }

            // Retrieve matching CardColour if it exists
            var cardColour = Constants.CardColours.FirstOrDefault(card => card.Name == CardColourName);

            // Throw error if CardColour hasn't been configured
            if (cardColour == null)
            {
                throw new NotImplementedException($"Unknown CardColour '{CardColourName.Value}'");
            }

            // Return CardColour value corresponding to the CardColourName
            return cardColour;
        }
    }
}
