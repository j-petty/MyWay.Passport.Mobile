using MyWay.Passport.Mobile.Models;
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

        public CardDetailsPage(CardDetails existingCard)
        {
            InitializeComponent();
            BindingContext = viewModel = new CardDetailsViewModel(Navigation, existingCard);
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
    }
}
