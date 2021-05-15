using System;
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
            CardDetails = new CardDetails();
        }

        /// <summary>
        /// Update CardDetails.
        /// </summary>
        /// <param name="existingCard">Existing card to update.</param>
        public CardDetailsViewModel(INavigation navigation, CardDetails existingCard) : base(navigation)
        {
            CardDetails = existingCard ?? new CardDetails();
        }
    }
}
