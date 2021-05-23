using System;
using System.Collections.ObjectModel;
using System.Linq;
using MyWay.Passport.Mobile.Models;
using MyWay.Passport.Mobile.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.ViewModels
{
    public class CardDetailsViewModel : BaseViewModel
    {
        #region Variables
        private CardDetails cardDetails;
        public CardDetails CardDetails
        {
            get { return cardDetails; }
            set { SetProperty(ref cardDetails, value); }
        }

        private ObservableCollection<CardColour> cardColours;
        public ObservableCollection<CardColour> CardColours
        {
            get { return cardColours; }
            private set { SetProperty(ref cardColours, value); }
        }

        private CardColour selectedCardColour;
        public CardColour SelectedCardColour
        {
            get { return selectedCardColour; }
            set
            {
                SetProperty(ref selectedCardColour, value);
                CardDetails.CardColourName = selectedCardColour?.Name;
            }
        }

        private bool cardNumberValid = true;
        public bool CardNumberValid
        {
            get { return cardNumberValid; }
            set { SetProperty(ref cardNumberValid, value); }
        }

        private bool cardPasswordValid = true;
        public bool CardPasswordValid
        {
            get { return cardPasswordValid; }
            set { SetProperty(ref cardPasswordValid, value); }
        }

        private bool birthDateValid = true;
        public bool BirthDateValid
        {
            get { return birthDateValid; }
            set { SetProperty(ref birthDateValid, value); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Save listener.
        /// </summary>
        public Command OnSaveSelected
        {
            get
            {
                return new Command(async () =>
                {
                    // Validate form fields
                    if (!ValidateFields())
                    {
                        return;
                    }

                    // Reset last updated to force balance refresh if details changed
                    CardDetails.LastUpdated = null;

                    // Save Card to storage
                    SettingsService.AddOrReplaceCard(CardDetails);

                    // Return to previous page
                    await Navigation.PopAsync();
                });
            }
        }

        /// <summary>
        /// Open register card listener.
        /// </summary>
        public Command OnRegisterSelected
        {
            get
            {
                return new Command(async () =>
                {
                    await Launcher.OpenAsync(new Uri(Constants.RegisterCardUrl));
                });
            }
        }

        /// <summary>
        /// Open Password help listener.
        /// </summary>
        public Command OnPasswordHelpSelected
        {
            get
            {
                return new Command(async () =>
                {
                    // Display information dialog
                    await Application.Current.MainPage.DisplayAlert(
                        string.Empty,
                        $"Your password is the secret question you answered when you registered your MyWay card.\n\nVisit {Constants.TransportCanberraDomain} for more information.",
                        "Done");
                });
            }
        }

        /// <summary>
        /// Open privacy policy listener.
        /// </summary>
        public Command OnPrivacyPolicySelected
        {
            get
            {
                return new Command(async () =>
                {
                    await Launcher.OpenAsync(new Uri(Constants.PrivacyPolicyUrl));
                });
            }
        }
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CardDetailsViewModel(INavigation navigation) : base(navigation)
        {
            CardColours = new ObservableCollection<CardColour>(Constants.CardColours);
            CardDetails = new CardDetails();
        }

        /// <summary>
        /// Update CardDetails.
        /// </summary>
        /// <param name="existingCard">Existing card to update.</param>
        public CardDetailsViewModel(INavigation navigation, CardDetails existingCard) : base(navigation)
        {
            CardColours = new ObservableCollection<CardColour>(Constants.CardColours);
            CardDetails = existingCard ?? new CardDetails();
            SelectedCardColour = CardColours.FirstOrDefault(colour => colour.Name == CardDetails.CardColourName);
        }

        private bool ValidateFields()
        {
            var cardsList = SettingsService.CardList;

            // Validate CardNumber
            CardNumberValid = !string.IsNullOrWhiteSpace(CardDetails.CardNumber) &&
                (!cardsList.Any(card => card.CardNumber == CardDetails.CardNumber && card.CardId != CardDetails.CardId));

            // Validate Password
            CardPasswordValid = !string.IsNullOrWhiteSpace(CardDetails.Password);

            // Validate Date of Birth
            BirthDateValid = CardDetails.DateOfBirth != null && CardDetails.DateOfBirth <= DateTime.Today;

            return
                CardNumberValid &&
                CardPasswordValid &&
                BirthDateValid;
        }
    }
}
