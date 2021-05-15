﻿using System.Collections.ObjectModel;
using System.Linq;
using MyWay.Passport.Mobile.Models;
using MyWay.Passport.Mobile.Pages;
using MyWay.Passport.Mobile.Services;
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
                    // Retrieive selected Card
                    var selectedCard = (listView.SelectedItem as CardDetails);

                    // Send selected card to CardDetailsPage to be updated
                    await Navigation.PushAsync(new CardDetailsPage(selectedCard));
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

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CardListViewModel(INavigation navigation) : base(navigation)
        {
        }

        public override void OnViewAppearing()
        {
            // Retrieve CardsList from storage
            Cards = new ObservableCollection<CardDetails>(SettingsService.CardList);

            base.OnViewAppearing();
        }

        public override void OnViewDisappearing()
        {
            if (Cards != null)
            {
                // Save updated CardList
                SettingsService.CardList = Cards.ToList();
            }

            base.OnViewDisappearing();
        }
    }
}
