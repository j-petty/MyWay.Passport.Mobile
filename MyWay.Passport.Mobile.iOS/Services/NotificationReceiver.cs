using System;
using UserNotifications;

namespace MyWay.Passport.Mobile.iOS.Services
{
    /// <summary>
    /// Required to allow local notifications while in foreground.
    /// </summary>
    public class NotificationReceiver : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Tell system to display the notification anyway
            completionHandler(UNNotificationPresentationOptions.Banner);
        }
    }
}
