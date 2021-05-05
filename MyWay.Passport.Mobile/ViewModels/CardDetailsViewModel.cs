using System;
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
        /// Clear button listener.
        /// </summary>
        public Command OnClearSelected
        {
            get
            {
                return new Command(() =>
                {
                    CardDetails = new CardDetails();
                    SettingsService.ClearLocalData();
                });
            }
        }

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

                    // Save CardDetails
                    SettingsService.CardDetails = CardDetails;

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

        // Default constructor
        public CardDetailsViewModel(INavigation navigation) : base(navigation)
        {
            CardDetails = SettingsService.CardDetails ?? new CardDetails();
        }
    }
}
