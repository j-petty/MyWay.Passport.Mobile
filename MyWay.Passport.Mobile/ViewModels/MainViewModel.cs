using System;
using System.Threading.Tasks;
using MyWay.Passport.Mobile.Models;
using MyWay.Passport.Mobile.Pages;
using MyWay.Passport.Mobile.Services;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Variables
        private CardDetails cardDetails;
        public CardDetails CardDetails
        {
            get { return cardDetails; }
            set { SetProperty(ref cardDetails, value); }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Open Settings listener.
        /// </summary>
        public Command OpenSettingsSelected
        {
            get
            {
                return new Command(async () =>
                {
                    await Navigation.PushAsync(new CardDetailsPage());
                });
            }
        }

        /// <summary>
        /// Refresh balance listener.
        /// </summary>
        public Command RefreshBalanceSelected
        {
            get
            {
                return new Command(async () =>
                {
                    // Don't load balance if card details aren't filled
                    if (CardDetails == null || !CardDetails.CheckFilled())
                    {
                        return;
                    }

                    // Refresh balance
                    await GetBalanceAsync();
                });
            }
        }
        #endregion

        // Default constructor
        public MainViewModel(INavigation navigation) : base(navigation)
        {
        }

        public override void OnViewAppearing()
        {
            base.OnViewAppearing();

            TryRefreshBalance();
        }

        public override void OnViewResuming()
        {
            base.OnViewAppearing();

            TryRefreshBalance();
        }

        /// <summary>
        /// Checks if balance need to be refreshed and invokes Command to refresh.
        /// </summary>
        private void TryRefreshBalance()
        {
            CardDetails = SettingsService.CardDetails;

            if (CardDetails == null || !CardDetails.CheckFilled())
            {
                CardDetails = new CardDetails();

                // Display error if card details haven't been provided
                ErrorMessage = "Enter MyWay card details to view balance.";
            }
            else if (CardDetails.LastUpdated == null || CardDetails.LastUpdated < DateTime.Now.AddHours(-1) || CardDetails.LastBalance == 0.0)
            {
                // Retrieve latest balance if haven't in the last hour or the balance is 0
                RefreshBalanceSelected.Execute(null);
            }
        }

        /// <summary>
        /// Get's the latest balance data from vendor API.
        /// </summary>
        private async Task GetBalanceAsync()
        {
            IsBusy = true;

            try
            {
                CardDetails = await App.VendorService.GetBalanceAsync(CardDetails);

                // Reset error on successful balance update
                ErrorMessage = null;
            }
            catch
            {
                // Display error if refresh failed
                ErrorMessage = "Failed to retrieve balance. Check your card details.";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
