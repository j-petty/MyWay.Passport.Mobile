using System;
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

        private double balance = 0.0;
        public double Balance
        {
            get { return balance; }
            set { SetProperty(ref balance, value); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Open Settings button listener.
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
        #endregion

        // Default constructor
        public MainViewModel(INavigation navigation) : base(navigation)
        {
            CardDetails = SettingsService.CardDetails;
        }

        public override void OnViewAppearing()
        {
            base.OnViewAppearing();

            if (CardDetails == null || !CardDetails.CheckFilled())
            {
                Console.WriteLine("Couldn't find card details");

                OpenSettingsSelected.Execute(null);
            }
        }
    }
}
