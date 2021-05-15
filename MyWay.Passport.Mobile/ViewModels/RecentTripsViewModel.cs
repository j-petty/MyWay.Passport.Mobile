using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MyWay.Passport.Mobile.Models;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.ViewModels
{
    public class RecentTripsViewModel : BaseViewModel
    {
        #region Variables
        private CardDetails selectedCard;
        public CardDetails SelectedCard
        {
            get { return selectedCard; }
            set { SetProperty(ref selectedCard, value); }
        }

        private ObservableCollection<RecentTrip> recentTrips;
        public ObservableCollection<RecentTrip> RecentTrips
        {
            get { return recentTrips; }
            set { SetProperty(ref recentTrips, value); }
        }

        private bool tripsLoaded = false;
        public bool TripsLoaded
        {
            get { return tripsLoaded; }
            set { SetProperty(ref tripsLoaded, value); }
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
        /// Refresh history listener.
        /// </summary>
        public Command RefreshHistorySelected
        {
            get
            {
                return new Command(async () =>
                {
                    // Don't load balance if Card details aren't filled
                    if (SelectedCard == null || !SelectedCard.CheckFilled())
                    {
                        // Display error if card details haven't been provided
                        ErrorMessage = Constants.ErrorMessages.RecentTripsMissingCardDetails;
                        IsBusy = false;
                        return;
                    }

                    // Refresh balance
                    await GetRecentTripsAsync();
                });
            }
        }
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RecentTripsViewModel(INavigation navigation, CardDetails card) : base(navigation)
        {
            SelectedCard = card;
            RecentTrips = new ObservableCollection<RecentTrip>();
        }

        public override void OnViewAppearing()
        {
            base.OnViewAppearing();

            // Retrieve latest trip history
            RefreshHistorySelected.Execute(null);
        }

        /// <summary>
        /// Get's the latest balance data from vendor API.
        /// </summary>
        private async Task GetRecentTripsAsync()
        {
            if (IsBusy)
            {
                // Exit early if request is already processing
                return;
            }

            IsBusy = true;

            try
            {
                // Retrieve recent trips
                RecentTrips = new ObservableCollection<RecentTrip>(
                    await App.VendorService.GetRecentTripsAsync(SelectedCard));

                // Reset error on successful history update
                ErrorMessage = null;
            }
            catch
            {
                // Reset recent trips
                RecentTrips = new ObservableCollection<RecentTrip>();

                // Display error if refresh failed
                ErrorMessage = Constants.ErrorMessages.RecentTripsFailure;
            }
            finally
            {
                IsBusy = false;
                TripsLoaded = true;
            }
        }
    }
}
