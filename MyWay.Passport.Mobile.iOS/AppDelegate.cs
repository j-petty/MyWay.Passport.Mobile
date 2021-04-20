using System;
using Foundation;
using MyWay.Passport.Mobile.iOS.Services;
using MyWay.Passport.Mobile.Services;
using Plugin.LocalNotifications;
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
            // TODO: enable request for permission and background refresh once Xamarin issue is fixed
            // https://github.com/xamarin/xamarin-macios/issues/6849

            // Request required permissions
            //RequestPermissions();

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            // Set backgrond App refresh frequency (seconds)
            //UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            return base.FinishedLaunching(application, options);
        }

        /// <summary>
        /// Perform background fetch
        /// </summary>
        public override async void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            try
            {
                // Try to fetch lastest balance details
                var cardDetails = await App.VendorService.GetBalanceAsync(SettingsService.CardDetails);

                Console.WriteLine("Successful background refresh");

                // TODO: show notification if balance is low
                CrossLocalNotifications.Current.Show(
                    "Fetch Complete",
                    "Testing refresh"
                    /*$"Successfully refreshed balance: ${cardDetails?.LastBalance.ToString("F2")}"*/);

                // Trigger success callback
                completionHandler(UIBackgroundFetchResult.NewData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed background refresh", ex);

                // TODO: remove this
                CrossLocalNotifications.Current.Show(
                    "Failed to Fetch",
                    "Testing refresh failed");

                // Trigger failure callback
                completionHandler(UIBackgroundFetchResult.Failed);
            }
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
