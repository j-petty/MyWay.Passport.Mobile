using System.Collections.Generic;
using System.Threading.Tasks;
using MyWay.Passport.Mobile.Models;

namespace MyWay.Passport.Mobile.Services
{
    public interface IVendorService
    {
        /// <summary>
        /// Retrieves a cards balance.
        /// </summary>
        /// <param name="cardDetails">Card details to retrieve balance for.</param>
        /// <returns>Returns card details with updated balance.</returns>
        Task<CardDetails> GetBalanceAsync(CardDetails cardDetails);

        /// <summary>
        /// Retrieves recent trips for a card.
        /// </summary>
        /// <param name="cardDetails">Card details to retrieve recent trips for</param>
        /// <returns>Returns list of recent trips.</returns>
        Task<IEnumerable<RecentTrip>> GetRecentTripsAsync(CardDetails cardDetails);
    }
}
