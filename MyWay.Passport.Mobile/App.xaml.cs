using System;
using Matcha.BackgroundService;
using Microsoft.Extensions.Configuration;
using MyWay.Passport.Mobile.Pages;
using MyWay.Passport.Mobile.Services;
using MyWay.Passport.Mobile.Themes;
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
        public static IConfiguration Configuration { get; private set; }

        private Color _barBackgroundColor;
        private Color _barTextColor;

        public App(Action<ConfigurationBuilder> configuration)
        {
            // Setup configuration (note appsettings is required but not included in source control)
            Configuration = Setup.Configuration
               .ConfigureSharedProject()
               .ConfigurePlatformProject(configuration)
               .Build();

            // Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Configuration[Constants.SysfusionLicenceKeyName]);

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

            UpdateTheme();
            InitializeMainPage();

            // Update BarColor if theme changes
            App.Current.RequestedThemeChanged += (s, a) =>
            {
                UpdateTheme();
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
        /// Update resources based on device theme.
        /// </summary>
        private void UpdateTheme()
        {
            var mergedDictionaries = App.Current.Resources.MergedDictionaries;

            if (mergedDictionaries != null)
            {
                // Clear resources
                mergedDictionaries.Clear();

                // Update resources based on theme
                switch (App.Current.RequestedTheme)
                {
                    case OSAppTheme.Dark:
                        mergedDictionaries.Add(new DarkTheme());
                        break;
                    case OSAppTheme.Light:
                    default:
                        mergedDictionaries.Add(new LightTheme());
                        break;
                }
            }

            // Update status bar colours
            _barBackgroundColor = (Color)App.Current.Resources["BackgroundColor"];
            _barTextColor = (Color)App.Current.Resources["TextColor"];
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
