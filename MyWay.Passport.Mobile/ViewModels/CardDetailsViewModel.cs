using MyWay.Passport.Mobile.Models;
using MyWay.Passport.Mobile.Services;
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
        #endregion

        // Default constructor
        public CardDetailsViewModel(INavigation navigation) : base(navigation)
        {
            CardDetails = SettingsService.CardDetails ?? new CardDetails();
        }

        public override void OnViewDisappearing()
        {
            SettingsService.CardDetails = CardDetails;
        }
    }
}
