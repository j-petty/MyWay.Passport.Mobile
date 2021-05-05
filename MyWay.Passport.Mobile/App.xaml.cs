using Matcha.BackgroundService;
using MyWay.Passport.Mobile.Pages;
using MyWay.Passport.Mobile.Services;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile
{
    public partial class App : Application
    {
        public static RequestService RequestService { get; private set; }
        public static VendorService VendorService { get; private set; }

        public App()
        {
            InitializeComponent();

            // Register services
            RequestService = new RequestService();
            VendorService = new VendorService();

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = (Color)App.Current.Resources["BackgroundColor"],
                BarTextColor = (Color)App.Current.Resources["LinkTextColor"]
            };
        }

        protected override void OnStart()
        {
            // Register background fetch service
            BackgroundAggregatorService.Add(() => new RefreshScheduler());

            // Start background service
            BackgroundAggregatorService.StartBackgroundService();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            // Publish event for ViewModels to handle returning from background
            MessagingCenter.Send(this, Constants.EventNames.OnResume);
        }
    }
}
