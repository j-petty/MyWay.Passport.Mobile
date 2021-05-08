using MyWay.Passport.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyWay.Passport.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecentTripsPage : ContentPage
    {
        private RecentTripsViewModel viewModel;

        public RecentTripsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new RecentTripsViewModel(Navigation);
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
