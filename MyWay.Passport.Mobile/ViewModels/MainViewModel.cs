using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MyWay.Passport.Mobile.Models;
using MyWay.Passport.Mobile.Pages;
using MyWay.Passport.Mobile.Services;
using Xamarin.Forms;

namespace MyWay.Passport.Mobile.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Variables
        private CardDetails cardDetails;
        public CardDetails CardDetails
        {
            get { return cardDetails; }
            set { SetProperty(ref cardDetails, value); }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Open Settings listener.
        /// </summary>
        public Command OpenSettingsSelected
        {
            get
            {
                return new Command(async () =>
                {
                    await Navigation.PushAsync(new CardDetailsPage());
                });
            }
        }

        /// <summary>
        /// Refresh balance listener.
        /// </summary>
        public Command RefreshBalanceSelected
        {
            get
            {
                return new Command(async () =>
                {
                    await GetBalanceAsync();
                });
            }
        }
        #endregion

        // Default constructor
        public MainViewModel(INavigation navigation) : base(navigation)
        {
        }

        public override void OnViewAppearing()
        {
            CardDetails = SettingsService.CardDetails;

            if (CardDetails == null || !CardDetails.CheckFilled())
            {
                Console.WriteLine("Couldn't find card details");

                // Open settings if card details haven't been provided
                OpenSettingsSelected.Execute(null);
            }
            else if (CardDetails.LastUpdated < DateTime.Now.AddHours(-1))
            {
                // Retrieve latest balance if haven't in the last hour
                RefreshBalanceSelected.Execute(null);
            }
        }

        /// <summary>
        /// Get's the latest balance data from TransportAct.
        /// </summary>
        public async Task GetBalanceAsync()
        {
            IsBusy = true;

            try
            {
                var response = await App.RequestService.SendRequest(
                    Constants.BalanceCheckApiUrl,
                    new Dictionary<string, string>
                    {
                        {"srno", CardDetails?.CardNumber},
                        {"day", CardDetails?.DateOfBirth?.Day.ToString("D2")},
                        {"month", CardDetails?.DateOfBirth?.Month.ToString("D2")},
                        {"year", CardDetails?.DateOfBirth?.Year.ToString()},
                        {"pwrd", CardDetails?.Password}
                    });

                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse html to get card balance
                    CardDetails.LastBalance = GetBalanceHtml(content);

                    // Set LastUpdated date to now
                    CardDetails.LastUpdated = DateTime.Now;

                    // Save updated CardDetails
                    SettingsService.CardDetails = CardDetails;

                    // Reset error on successful balance update
                    ErrorMessage = null;
                }
                else
                {
                    throw new InvalidOperationException("HTTP request failure response");
                }
            }
            catch (Exception e)
            {
                // TODO: handle misc errors
                Console.WriteLine($"[{nameof(MainViewModel)}]: " + e);

                ErrorMessage = "Failed to retrieve balance. Check your card details.";

                // Reset card balance if error occured
                CardDetails.LastBalance = 0.0;
                SettingsService.CardDetails = CardDetails;
            }
            finally
            {
                IsBusy = false;
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
