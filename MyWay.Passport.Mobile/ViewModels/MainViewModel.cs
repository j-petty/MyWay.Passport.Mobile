using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private CardDetails selectedCard;
        public CardDetails SelectedCard
        {
            get { return selectedCard; }
            set { SetProperty(ref selectedCard, value); }
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
                    // Don't show history if card is null
                    if (SelectedCard == null)
                    {
                        return;
                    }

                    await Navigation.PushAsync(new RecentTripsPage(SelectedCard));
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
                    if (SelectedCard == null || !SelectedCard.CheckFilled() || !Cards.Any())
                    {
                        IsBusy = false;
                        return;
                    }

                    // Refresh balance
                    await GetBalanceAsync();
                });
            }
        }

        /// <summary>
        /// Selected card changed listener.
        /// </summary>
        public Command SelectedCardChanged
        {
            get
            {
                return new Command(() =>
                {
                    if (SelectedCard == null)
                    {
                        return;
                    }
                    TryRefreshBalance();
                });
            }
        }
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainViewModel(INavigation navigation) : base(navigation)
        {
            UpdateCardSize();
        }

        public override void OnViewAppearing()
        {
            // Retrieve CardsList from storage
            Cards = new ObservableCollection<CardDetails>(SettingsService.CardList);

            // Set the initial SelectedCard (happens automatically on iOS but not Android)
            if (SelectedCard == null && Cards.Any())
            {
                SelectedCard = Cards.FirstOrDefault();
            }

            UpdateCardSize();

            base.OnViewAppearing();

            TryRefreshBalance();
        }

        public override void OnViewResuming()
        {
            base.OnViewResuming();

            TryRefreshBalance();
        }

        public override void OnViewDisappearing()
        {
            Cards = new ObservableCollection<CardDetails>();
            ErrorMessage = null;

            base.OnViewDisappearing();
        }

        /// <summary>
        /// Checks if balance need to be refreshed and invokes Command to refresh.
        /// </summary>
        private void TryRefreshBalance()
        {
            ErrorMessage = null;

            if (SelectedCard == null || !SelectedCard.CheckFilled())
            {
                if (SelectedCard == null)
                {
                    // Prepopulate an empty Card
                    Cards.Add(new CardDetails());
                }

                // Display error if card details haven't been provided
                ErrorMessage = Constants.ErrorMessages.BalanceCheckMissingCardDetails;
                IsBusy = false;
            }
            else if (SelectedCard.LastUpdated == null || SelectedCard.LastUpdated < DateTime.Now.AddHours(-1) || SelectedCard.LastBalance == 0.0)
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

            // Retrieve SelectedItem
            var selectedIndex = Cards.IndexOf(SelectedCard);

            try
            {
                // Retrieve updated Card details
                var returnedCard = await App.VendorService.GetBalanceAsync(SelectedCard);

                if (selectedIndex >= 0 && Cards.Count > selectedIndex)
                {
                    // Update selected item
                    Cards[selectedIndex] = returnedCard;
                }

                // Reset error on successful balance update
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                if (selectedIndex >= 0 && Cards.Count > selectedIndex)
                {
                    // Clear LastUpdated
                    Cards[selectedIndex].LastUpdated = null;
                }

                // Display error if refresh failed
                ErrorMessage = Constants.ErrorMessages.BalanceCheckFailure;
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Update rendered CardSize based on screen.
        /// </summary>
        private void UpdateCardSize()
        {
            CardWidth = Math.Min(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 500);
            CardHeight = cardWidth * 0.63;
        }
    }
}
