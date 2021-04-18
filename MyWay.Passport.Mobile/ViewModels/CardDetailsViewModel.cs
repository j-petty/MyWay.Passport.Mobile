using System;
using MyWay.Passport.Mobile.Models;
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
        /// Login button listener.
        /// </summary>
        /*public Command OnLoginSelected
        {
            get
            {
                return new Command(async () =>
                {
                    bool result = await LoginAsync();

                    if (result)
                    {
                        await Navigation.PushModalAsync(new NavigationPage(new MainPage()));
                    }
                });
            }
        }*/
        #endregion

        // Default constructor
        public CardDetailsViewModel(INavigation navigation) : base(navigation)
        {
            CardDetails = new CardDetails
            {
                CardNumber = Guid.NewGuid().ToString()
            };
        }
    }
}
