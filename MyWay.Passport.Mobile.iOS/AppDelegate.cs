using System;
using Foundation;
using Matcha.BackgroundService.iOS;
using MyWay.Passport.Mobile.iOS.Services;
using Syncfusion.ListView.XForms.iOS;
using UIKit;
using UserNotifications;

namespace MyWay.Passport.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary options)
        {
            // Request required permissions
            RequestPermissions();

            // Init background fetch service
            BackgroundAggregator.Init(this);

            Xamarin.Forms.Forms.Init();

            // Init SFList View
            SfListViewRenderer.Init();

            LoadApplication(new App());

#pragma warning disable CS0162 // Unreachable code detected
            if (Constants.EnableAnalytics)
            {
                // Init Firebase Analytics
                Firebase.Core.App.Configure();
                // Required for Firebase. Remove this line after issue is resolved: https://github.com/xamarin/GoogleApisForiOSComponents/issues/443
                var instance = Firebase.Installations.Installations.DefaultInstance;
            }
#pragma warning restore CS0162 // Unreachable code detected

            return base.FinishedLaunching(application, options);
        }

        private void RequestPermissions()
        {
            // Request notification permissions based on iOS version
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Ask the user for permission to get notifications on iOS 10.0+
                UNUserNotificationCenter.Current.RequestAuthorization(
                        UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                        (approved, error) =>
                        {
                            if (!approved || error != null)
                            {
                                Console.WriteLine("Notifications are disabled");
                            }
                        });

                // Watch for notifications while app is active
                UNUserNotificationCenter.Current.Delegate = new NotificationReceiver();
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // Ask the user for permission to get notifications on iOS 8.0+
                var settings = UIUserNotificationSettings.GetSettingsForTypes(
                        UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                        new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
        }
    }
}
