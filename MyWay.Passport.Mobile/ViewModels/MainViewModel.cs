using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MyWay.Passport.Mobile.Models;
using MyWay.Passport.Mobile.Pages;
using MyWay.Passport.Mobile.Services;
using Xamarin.Essentials;
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

        private ObservableCollection<CardDetails> cards;
        public ObservableCollection<CardDetails> Cards
        {
            get { return cards; }
            set { SetProperty(ref cards, value); }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        private double cardWidth;
        public double CardWidth
        {
            get { return cardWidth; }
            set { SetProperty(ref cardWidth, value); }
        }

        private double cardHeight;
        public double CardHeight
        {
            get { return cardHeight; }
            set { SetProperty(ref cardHeight, value); }
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
                    //await Navigation.PushAsync(new CardDetailsPage());
                    await Navigation.PushAsync(new CardListPage());
                });
            }
        }

        /// <summary>
        /// Open Recent Trips listener.
        /// </summary>
        public Command OpenRecentTripsSelected
        {
            get
            {
                return new Command(async () =>
                {
                    await Navigation.PushAsync(new RecentTripsPage());
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
                        IsBusy = false;
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
            Cards = new ObservableCollection<CardDetails>
            {
                new CardDetails
                {
                    CardNumber = "123456"
                },
                new CardDetails
                {
                    CardNumber = "789123"
                },
                new CardDetails
                {
                    CardNumber = "456789"
                },
            };

            UpdateCardSize();
        }

        public override void OnViewAppearing()
        {
            UpdateCardSize();

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
                ErrorMessage = Constants.ErrorMessages.BalanceCheckMissingCardDetails;
                IsBusy = false;
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
            if (IsBusy)
            {
                // Exit early if request is already processing
                return;
            }

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
                ErrorMessage = Constants.ErrorMessages.BalanceCheckFailure;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void UpdateCardSize()
        {
            CardWidth = Math.Min(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 500);
            CardHeight = cardWidth * 0.63;
        }
    }
}
