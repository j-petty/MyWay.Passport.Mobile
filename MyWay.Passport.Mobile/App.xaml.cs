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

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
