using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Matcha.BackgroundService.Droid;
using Plugin.CurrentActivity;
using Xamarin.Essentials;
using Firebase.Analytics;

namespace MyWay.Passport.Mobile.Droid
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Enable background fetch service
            BackgroundAggregator.Init(this);

            // Setup CurrentActivity
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            base.OnCreate(savedInstanceState);

            // Init Xamarin Essentials
            Platform.Init(this, savedInstanceState);

            // Apply correct theme
            ApplyTheme();

            // Disable Analytics in Debug mode
            FirebaseAnalytics.GetInstance(this).SetAnalyticsCollectionEnabled(Constants.EnableAnalytics);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void ApplyTheme()
        {
            switch (AppInfo.RequestedTheme)
            {
                case AppTheme.Dark:
                    SetTheme(Resource.Style.MainThemeDark);
                    break;
                default:
                    SetTheme(Resource.Style.MainTheme);
                    break;
            }
        }
    }
}