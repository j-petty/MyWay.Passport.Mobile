using System;
using System.Collections.ObjectModel;
using MyWay.Passport.Mobile.Models;
using MyWay.Passport.Mobile.Pages;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.ViewModels
{
    public class CardListViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<CardDetails> cards;
        public ObservableCollection<CardDetails> Cards
        {
            get { return cards; }
            set { SetProperty(ref cards, value); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// AddCard listener.
        /// </summary>
        public Command AddCardSelected
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
        /// List item selected listener.
        /// </summary>
        public Command ListItemSelected
        {
            get
            {
                return new Command<SfListView>(async (listView) =>
                {
                    // TODO: pass details of selected card
                    //(listView.SelectedItem as CardDetails)
                    await Navigation.PushAsync(new CardDetailsPage());
                });
            }
        }

        /// <summary>
        /// Delete Card listener.
        /// </summary>
        public Command DeleteCardSelected
        {
            get
            {
                return new Command<CardDetails>(async (card) =>
                {
                    // Present delete Card confirmation dialog
                    var result = await Application.Current.MainPage.DisplayAlert(
                        "Are you sure?",
                        "Your card details will be removed.",
                        "Delete",
                        "Cancel");

                    if (result)
                    {
                        // Remove selected Card from the list
                        Cards.Remove(card);
                    }
                });
            }
        }
        #endregion

        // Default constructor
        public CardListViewModel(INavigation navigation) : base(navigation)
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
        }
    }
}
