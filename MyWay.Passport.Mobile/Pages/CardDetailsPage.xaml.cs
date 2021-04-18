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
    }
}
