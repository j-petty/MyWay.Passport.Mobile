using System;
using System.Linq;
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
                var cardTasks = SettingsService.CardList.Select(card => App.VendorService.GetBalanceAsync(card));

                // Loop through each Card after all requests return
                foreach (var card in await Task.WhenAll(cardTasks))
                {
                    // Show notification if balance is low
                    if (card != null && card.LastBalance <= Constants.BalanceWarningLimit)
                    {
                        CrossLocalNotifications.Current.Show(
                            "MyWay Balance Warning",
                            $"Your MyWay card {card.CardNumber} is running low. ${card.LastBalance?.ToString("F2")}");
                    }
                }

                Console.WriteLine("Successful background refresh");

                // Trigger success callback
                return shouldTriggerAgain;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed background refresh", ex);

                // Trigger failure callback
                return shouldTriggerAgain;
            }
        }
    }
}
