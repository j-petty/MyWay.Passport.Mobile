using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyWay.Passport.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            // TODO: check if details have been captured or not
            Navigation.PushAsync(new CardDetailsPage());

            base.OnAppearing();
        }

        private async void OpenSettingsListener(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CardDetailsPage());
        }
    }
}
