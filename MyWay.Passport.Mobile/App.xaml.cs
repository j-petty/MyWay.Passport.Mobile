using MyWay.Passport.Mobile.Pages;
using MyWay.Passport.Mobile.Services;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile
{
    public partial class App : Application
    {
        public static RequestService RequestService { get; private set; }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

            RequestService = new RequestService();
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
