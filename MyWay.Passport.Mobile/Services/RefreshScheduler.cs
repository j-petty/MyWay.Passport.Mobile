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

                // Show notification if balance is low
                if (cardDetails != null && cardDetails.LastBalance <= Constants.BalanceWarningLimit)
                {
                    CrossLocalNotifications.Current.Show(
                        "MyWay Balance Low",
                        $"Your MyWay balance is running low. ${cardDetails?.LastBalance?.ToString("F2")}");
                }

                // Trigger success callback
                return await Task.FromResult(shouldTriggerAgain);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed background refresh", ex);

                // Trigger failure callback
                return await Task.FromResult(shouldTriggerAgain);
            }
        }
    }
}
