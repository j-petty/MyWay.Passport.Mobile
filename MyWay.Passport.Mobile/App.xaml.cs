using Matcha.BackgroundService;
using MyWay.Passport.Mobile.Pages;
using MyWay.Passport.Mobile.Services;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile
{
#pragma warning disable CS0162 // Unreachable code detected
    public partial class App : Application
    {
#if DEBUG
        public const bool UseMockData = false;
#else
        public const bool UseMockData = false;
#endif

        public static RequestService RequestService { get; private set; }
        public static IVendorService VendorService { get; private set; }

        private Color _barBackgroundColor;
        private Color _barTextColor;

        public App()
        {
            // Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Constants.SysfusionLicenceKey);

            InitializeComponent();

            // Register services
            RequestService = new RequestService();

            if (UseMockData)
            {
                VendorService = new MockVendorService();
            }
            else
            {
                VendorService = new VendorService();
            }

            UpdateStatusBarColors();
            InitializeMainPage();

            // Update BarColor if theme changes
            App.Current.RequestedThemeChanged += (s, a) =>
            {
                UpdateStatusBarColors();
                InitializeMainPage();
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

        /// <summary>
        /// Update StatusBar colours based on device theme.
        /// </summary>
        private void UpdateStatusBarColors()
        {
            _barBackgroundColor = App.Current.RequestedTheme == OSAppTheme.Dark ? (Color)App.Current.Resources["DBackgroundColor"] : (Color)App.Current.Resources["BackgroundColor"];
            _barTextColor = App.Current.RequestedTheme == OSAppTheme.Dark ? (Color)App.Current.Resources["DTextColor"] : (Color)App.Current.Resources["TextColor"];
        }

        /// <summary>
        /// Initialize the MainPage. Equivelant to restarting application.
        /// </summary>
        private void InitializeMainPage()
        {
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = _barBackgroundColor,
                BarTextColor = _barTextColor
            };
        }
    }
#pragma warning restore CS0162 // Unreachable code detected
}
