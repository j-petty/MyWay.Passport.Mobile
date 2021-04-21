using System;
using System.Threading.Tasks;
using Matcha.BackgroundService;
using Plugin.LocalNotifications;

namespace MyWay.Passport.Mobile.Services
{
    public class RefreshScheduler : IPeriodicTask
    {
        // Desired interval between executions
        public TimeSpan Interval { get; set; }

        // Should the task retrigger after running
        private const bool shouldTriggerAgain = true;

        public RefreshScheduler()
        {
            // Set desired refresh interval (ultimately the device controls when refresh happens)
            Interval = TimeSpan.FromSeconds(Constants.BackgroundRefreshFrequency);
        }

        public async Task<bool> StartJob()
        {
            try
            {
                // Try to fetch lastest balance details
                var cardDetails = await App.VendorService.GetBalanceAsync(SettingsService.CardDetails);

                Console.WriteLine("Successful background refresh");

                // TODO: show notification only if balance is low
                CrossLocalNotifications.Current.Show(
                    "TODO: change this title",
                    $"Successfully refreshed balance: ${cardDetails?.LastBalance.ToString("F2")}");

                // Trigger success callback
                return await Task.FromResult(shouldTriggerAgain);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed background refresh", ex);

                // TODO: remove this notification
                CrossLocalNotifications.Current.Show(
                    "Failed to Fetch",
                    "Testing refresh failed. TODO: remove this notification");

                // Trigger failure callback
                return await Task.FromResult(shouldTriggerAgain);
            }
        }
    }
}
