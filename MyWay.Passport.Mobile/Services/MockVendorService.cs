using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWay.Passport.Mobile.Models;

namespace MyWay.Passport.Mobile.Services
{
    public class MockVendorService : IVendorService
    {
        public Task<CardDetails> GetBalanceAsync(CardDetails cardDetails)
        {
            var returnedCard = new CardDetails
            {
                CardId = cardDetails.CardId,
                CardNumber = cardDetails.CardNumber,
                Password = "Password",
                DateOfBirth = new DateTime(1990, 1, 1),
                LastBalance = 13.44,
                LastUpdated = DateTime.Now
            };

            // Save updated CardDetails
            SettingsService.AddOrReplaceCard(returnedCard);

            return Task.FromResult(returnedCard);
        }

        public Task<IEnumerable<RecentTrip>> GetRecentTripsAsync(CardDetails cardDetails)
        {
            var recentTrips = new List<RecentTrip>();
            var random = new Random();

            var tripCount = 30;

            // Generate random trips
            for (var i = 0; i < tripCount; i++)
            {
                // Some trips are transfers but not the first or last ones
                var isTransfer = i != 0 && i != tripCount - 1 && random.Next(3) == 1;

                recentTrips.Add(new RecentTrip
                {
                    To = "Arrival Location",
                    From = "Departure Location",
                    Date = isTransfer ? recentTrips[i - 1].Date : DateTime.Now.AddHours(-random.Next(6)).AddDays(-random.Next(i, i + 2)),
                    Price = isTransfer ? 0 : 3.22,
                    Route = isTransfer ? recentTrips[i - 1].Route : Guid.NewGuid().ToString()
                });
            }

            // Sort trips by date then price to ensure transfers are at the top
            var orderedTrips = recentTrips
                .OrderByDescending(trip => trip.Date)
                .ThenBy(trip => trip.Price);

            return Task.FromResult(orderedTrips.AsEnumerable());
        }
    }
}
