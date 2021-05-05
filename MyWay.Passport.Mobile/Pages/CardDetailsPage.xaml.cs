using System;
using MyWay.Passport.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyWay.Passport.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardDetailsPage : ContentPage
    {
        private CardDetailsViewModel viewModel;

        public CardDetailsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CardDetailsViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnViewAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.OnViewDisappearing();
        }

        private async void OnClearClicked(object sender, EventArgs e)
        {
            // Display confirmation dialog
            var result = await DisplayAlert("Are you sure?", "This will delete your saved card details.", "Delete", "Cancel");

            if (result)
            {
                // Invoke OnClearSelected if confirmed
                viewModel.OnClearSelected.Execute(null);
            }
        }

        private async void OnPasswordHelpClicked(object sender, EventArgs e)
        {
            // Display information dialog
            await DisplayAlert(string.Empty, $"Your password is the secret question you answered when you registered your MyWay card.\n\nVisit {Constants.TransportCanberraDomain} for more information.", "Done");
        }
    }
}
