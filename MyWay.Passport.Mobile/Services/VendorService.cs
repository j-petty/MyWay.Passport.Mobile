using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MyWay.Passport.Mobile.Models;

namespace MyWay.Passport.Mobile.Services
{
    public class VendorService
    {
        public async Task<CardDetails> GetBalanceAsync(CardDetails cardDetails)
        {
            try
            {
                var response = await App.RequestService.SendRequest(
                    Constants.BalanceCheckApiUrl,
                    new Dictionary<string, string>
                    {
                        {"srno", cardDetails?.CardNumber},
                        {"day", cardDetails?.DateOfBirth?.Day.ToString("D2")},
                        {"month", cardDetails?.DateOfBirth?.Month.ToString("D2")},
                        {"year", cardDetails?.DateOfBirth?.Year.ToString()},
                        {"pwrd", cardDetails?.Password}
                    });

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse html to get card balance
                    cardDetails.LastBalance = GetBalanceHtml(content);

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

        private double GetBalanceHtml(string html)
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
                        value = value.Substring(2, value.Length - 2);

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
    }
}
