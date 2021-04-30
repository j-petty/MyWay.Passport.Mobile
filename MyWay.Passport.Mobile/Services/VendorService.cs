using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MyWay.Passport.Mobile.Models;

namespace MyWay.Passport.Mobile.Services
{
    public class VendorService
    {
        #region Public Methods

        public async Task<CardDetails> GetBalanceAsync(CardDetails cardDetails)
        {
            try
            {
                var response = await App.RequestService.SendRequest(
                    Constants.BalanceCheckApiUrl,
                    CreateRequestPayload(cardDetails));

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse html to get card balance
                    cardDetails.LastBalance = GetBalance(content);

                    // Set LastUpdated date to now
                    cardDetails.LastUpdated = DateTime.Now;

                    // Save updated CardDetails
                    SettingsService.CardDetails = cardDetails;

                    return cardDetails;
                }
                else
                {
                    throw new InvalidOperationException("HTTP request failure response");
                }
            }
            catch (Exception e)
            {
                // TODO: handle misc errors
                Console.WriteLine($"[{nameof(VendorService)}]: " + e);

                // Reset card balance if error occured
                cardDetails.LastBalance = 0.0;
                SettingsService.CardDetails = cardDetails;

                throw;
            }
        }

        public async Task<IEnumerable<RecentTrip>> GetRecentTripsAsync(CardDetails cardDetails)
        {
            try
            {
                var response = await App.RequestService.SendRequest(
                    Constants.BalanceCheckApiUrl,
                    CreateRequestPayload(cardDetails));

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse html to get recent trips
                    return GetRecentTrips(content);
                }
                else
                {
                    throw new InvalidOperationException("HTTP request failure response");
                }
            }
            catch (Exception e)
            {
                // TODO: handle misc errors
                Console.WriteLine($"[{nameof(VendorService)}]: " + e);

                throw;
            }
        }

        #endregion

        #region HTML Parsing Methods

        /// <summary>
        /// Parse HTML to get balance.
        /// </summary>
        /// <param name="html">Html to parse.</param>
        /// <returns>Returns balance.</returns>
        private double? GetBalance(string html)
        {
            try
            {
                // Load html document
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                // Navigate to the correct html block
                var mainBlock = htmlDoc.GetElementbyId("main");
                var tableRows = mainBlock.SelectNodes("//table[contains(@class,'smartCardTable type3')]//tr");

                // Loop through table rows
                foreach (var row in tableRows)
                {
                    var head = row.SelectSingleNode("th");

                    // Check if this is the correct table row
                    if (head.InnerText == "Card Balance")
                    {
                        // Retrieve correct data node
                        var value = row.SelectSingleNode("td").InnerText;

                        // Remove $ and space from start of value
                        value = CleanString(value);

                        // Return value as double
                        return double.Parse(value);
                    }
                }

                return 0.0;
            }
            catch
            {
                Console.WriteLine("Failed to parse response HTML");

                throw;
            }
        }

        /// <summary>
        /// Parse HTML to get trip history.
        /// </summary>
        /// <param name="html">Html to parse.</param>
        /// <returns>Returns recent trips.</returns>
        private IEnumerable<RecentTrip> GetRecentTrips(string html)
        {
            try
            {
                // Load html document
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                // Navigate to the correct html block
                var mainBlock = htmlDoc.GetElementbyId("main");
                var tableRows = mainBlock.SelectNodes("//table[contains(@class,'smartCardTable type2')]//tr");

                var recentTrips = new List<RecentTrip>();

                // Loop through table rows
                foreach (var row in tableRows.Skip(1))
                {
                    var rowData = row.SelectNodes("td");

                    // Get trip details from data
                    var date = GetTripDate(rowData);
                    var price = GetTripPrice(rowData);
                    var route = GetTripRoute(rowData);
                    var destination = GetTripLocation(rowData);

                    // Don't return trips which were transfers or card deposits
                    if (price == null || price > 0.0 || date == null || string.IsNullOrEmpty(route))
                    {
                        continue;
                    }

                    // Initiate the trip
                    var thisTrip = new RecentTrip
                    {
                        To = destination,
                        Route = route,
                        Price = -price,
                        Date = date
                    };

                    // Get trip departure (comes as seperate row in data)
                    var indexOfTrip = tableRows.ToList().FindIndex(r => r == row);
                    thisTrip.From = GetTripDepartureLocation(tableRows, thisTrip, indexOfTrip);

                    // Add trip to list
                    recentTrips.Add(thisTrip);
                }

                return recentTrips;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to parse response HTML");

                throw;
            }
        }

        #endregion

        #region Common Helper Methods

        /// <summary>
        /// Create request payload.
        /// </summary>
        /// <param name="cardDetails">Card details to populate in request.</param>
        /// <returns>Returns populated request payload.</returns>
        private Dictionary<string, string> CreateRequestPayload(CardDetails cardDetails)
        {
            return new Dictionary<string, string>
            {
                {"srno", cardDetails?.CardNumber},
                {"day", cardDetails?.DateOfBirth?.Day.ToString("D2")},
                {"month", cardDetails?.DateOfBirth?.Month.ToString("D2")},
                {"year", cardDetails?.DateOfBirth?.Year.ToString()},
                {"pwrd", cardDetails?.Password}
            };
        }

        #endregion

        #region RecentTrips Helper Methods

        /// <summary>
        /// Gets the trips Price.
        /// </summary>
        /// <param name="rowData">Data to retrieve trip Price from.</param>
        /// <returns>Returns Price if it exists.</returns>
        private double? GetTripPrice(HtmlNodeCollection rowData)
        {
            if (rowData == null)
            {
                return null;
            }

            var priceStr = CleanString(rowData.ElementAtOrDefault(5)?.InnerText);

            if (string.IsNullOrEmpty(priceStr))
            {
                return null;
            }
            return double.Parse(priceStr);
        }

        /// <summary>
        /// Gets the trips Date.
        /// </summary>
        /// <param name="rowData">Data to retrieve trip Date from.</param>
        /// <returns>Returns Date if it exists.</returns>
        private DateTime? GetTripDate(HtmlNodeCollection rowData)
        {
            if (rowData == null)
            {
                return null;
            }

            var tripDateStr = CleanString(rowData.ElementAtOrDefault(0)?.InnerText);

            if (string.IsNullOrEmpty(tripDateStr))
            {
                return null;
            }
            return DateTime.Parse(tripDateStr);
        }

        /// <summary>
        /// Gets the trips Route.
        /// </summary>
        /// <param name="rowData">Data to retrieve trip Route from.</param>
        /// <returns>Returns Route if it exists.</returns>
        private string GetTripRoute(HtmlNodeCollection rowData)
        {
            if (rowData == null)
            {
                return null;
            }
            return CleanString(rowData.ElementAtOrDefault(3)?.InnerText);
        }

        /// <summary>
        /// Gets the trips Location (destination).
        /// </summary>
        /// <param name="rowData">Data to retrieve trip Location from.</param>
        /// <returns>Returns Location if it exists.</returns>
        private string GetTripLocation(HtmlNodeCollection rowData)
        {
            if (rowData == null)
            {
                return null;
            }
            return CleanString(rowData.ElementAtOrDefault(4)?.InnerText);
        }

        /// <summary>
        /// Remove unwanted spaces or characters.
        /// </summary>
        /// <param name="inputString">String to clean.</param>
        /// <returns>Clean version of inputString.</returns>
        private string CleanString(string inputString)
        {
            if (inputString == null)
            {
                return inputString;
            }

            // Remove space HTML entity from string
            inputString = inputString.Replace("&nbsp;", string.Empty);

            // Remove $ from start
            if (inputString.StartsWith("$"))
            {
                inputString = inputString.Remove(0, 1);
            }

            // Return trimmed string
            return inputString.Trim();
        }

        /// <summary>
        /// Finds a departure location for a corresponding trip. This is based on route, price and time.
        /// </summary>
        /// <param name="tableRows">Rows to look in.</param>
        /// <param name="trip">Trip to get departure location for.</param>
        /// <returns>Returns departure location name.</returns>
        private string GetTripDepartureLocation(HtmlNodeCollection tableRows, RecentTrip trip, int tripIndex)
        {
            // Find previous table row
            var departureRow = tableRows.Skip(tripIndex + 1).FirstOrDefault(row =>
            {
                var rowData = row?.SelectNodes("td");

                if (rowData == null)
                {
                    return false;
                }

                var route = GetTripRoute(rowData);
                var price = GetTripPrice(rowData);

                // Confirm this is a departure record
                if (string.IsNullOrEmpty(route) || route != trip.Route || price != null)
                {
                    return false;
                }

                return true;
            });

            if (departureRow == null)
            {
                return null;
            }

            var location = GetTripLocation(departureRow.SelectNodes("td"));

            // Return location of departure
            return location;
        }

        #endregion
    }
}
