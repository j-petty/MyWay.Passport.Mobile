using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MyWay.Passport.Mobile.Models;

namespace MyWay.Passport.Mobile
{
    public static class Constants
    {
        // Disable API on developer devices
#if DEBUG
        public const bool EnableAnalytics = false;
#else
        public const bool EnableAnalytics = true;
#endif

        // TransportCanberra URLs
        public const string TransportCanberraDomain = "www.transport.act.gov.au";
        public const string BalanceCheckApiUrl = "https://" + TransportCanberraDomain + "/tickets-and-myway/check-myway-balance/check-your-balance?sq_content_src=%2BdXJsPWh0dHAlM0ElMkYlMkZmaWxlcy50cmFuc3BvcnQuYWN0Lmdvdi5hdSUyRkFSVFMlMkZ1c2VfRnVuY3MuYXNwJmFsbD0x";
        public const string RegisterCardUrl = "https://" + TransportCanberraDomain + "/tickets-and-myway/register-myway";
        public const string PasswordHelpUrl = "https://" + TransportCanberraDomain + "/tickets-and-myway/check-myway-balance#Protect";

        // Susfusion Licence
        public const string SysfusionLicenceKey = "NDQzNzIxQDMxMzkyZTMxMmUzMG0wRTFBcTBKVkkxbXlwbmJUSmZYbFMxRWpJa0REbHVjTDJzdzVDU0ZRdkU9";

        // Internal URLs
        public const string PrivacyPolicyUrl = "https://www.jamespetty.com.au/privacy.html";

        // Background app refresh frequency (seconds). Minimum 15 minutes.
        public const double BackgroundRefreshFrequency = 1800;

        // Balance to send out warning at
        public const double BalanceWarningLimit = 5.0;

        // Maximum number of RecentTrips to show
        public const int MaxRecentTrips = 25;

        // Card Colours
        public static readonly IEnumerable<CardColour> CardColours = new ReadOnlyCollection<CardColour>(
            new List<CardColour>
            {
                new CardColour
                {
                    Name = ColourNames.Red,
                    BackgroundColour = "#E71D36",
                    TextColour = "#FFFFFF"
                },
                new CardColour
                {
                    Name = ColourNames.Green,
                    BackgroundColour = "#06D6A0",
                    TextColour = "#FFFFFF"
                },
                new CardColour
                {
                    Name = ColourNames.Blue,
                    BackgroundColour = "#3A86FF",
                    TextColour = "#FFFFFF"
                },
                new CardColour
                {
                    Name = ColourNames.Yellow,
                    BackgroundColour = "#FEE440",
                    TextColour = "#000000"
                },
                new CardColour
                {
                    Name = ColourNames.Purple,
                    BackgroundColour = "#8338EC",
                    TextColour = "#FFFFFF"
                }
            });

        public static class SettingNames
        {
            [Obsolete("CardDetails should not be used. Reger to CardList instead.")]
            public const string CardDetails = nameof(CardDetails);
            public const string CardList = nameof(CardList);
        }

        public static class EventNames
        {
            public const string OnResume = "OnResume";
        }

        public static class ErrorMessages
        {
            public const string GenericFailure = "Check your card details.\n\n\nIt can take up to a few hours to activate new accounts.";

            public const string BalanceCheckFailure = "Failed to retrieve balance. " + GenericFailure;
            public const string BalanceCheckMissingCardDetails = "Enter MyWay card details to view balance.";

            public const string RecentTripsFailure = "Failed to retrieve trip history. " + GenericFailure;
            public const string RecentTripsMissingCardDetails = "Enter MyWay card details to view recent trips.";
        }

        public static class AnalyticsEvents
        {
            public const string BalanceRefershSuccess = nameof(BalanceRefershSuccess);
            public const string BalanceRefershFailure = nameof(BalanceRefershFailure);

            public const string RecentTripRefreshSuccess = nameof(RecentTripRefreshSuccess);
            public const string RecentTripRefreshFailure = nameof(RecentTripRefreshFailure);
        }
    }

    public enum ColourNames
    {
        Red,
        Green,
        Blue,
        Yellow,
        Purple
    }
}
