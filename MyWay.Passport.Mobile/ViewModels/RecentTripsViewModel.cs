using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MyWay.Passport.Mobile.Models;
using MyWay.Passport.Mobile.Services;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.ViewModels
{
    public class RecentTripsViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<RecentTrip> recentTrips;
        public ObservableCollection<RecentTrip> RecentTrips
        {
            get { return recentTrips; }
            set { SetProperty(ref recentTrips, value); }
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
                    // Don't load balance if card details aren't filled
                    if (SettingsService.CardDetails == null || !SettingsService.CardDetails.CheckFilled())
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

        // Default constructor
        public RecentTripsViewModel(INavigation navigation) : base(navigation)
        {
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
                RecentTrips = new ObservableCollection<RecentTrip>(await App.VendorService.GetRecentTripsAsync(SettingsService.CardDetails));

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
            }
        }
    }
}
