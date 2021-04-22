namespace MyWay.Passport.Mobile
{
    public static class Constants
    {
        // TODO: tidy this up?
        public const string BalanceCheckApiUrl = "https://www.transport.act.gov.au/tickets-and-myway/check-myway-balance/check-your-balance?sq_content_src=%2BdXJsPWh0dHAlM0ElMkYlMkZmaWxlcy50cmFuc3BvcnQuYWN0Lmdvdi5hdSUyRkFSVFMlMkZ1c2VfRnVuY3MuYXNwJmFsbD0x";

        // Background app refresh frequency (seconds). Minimum 15 minutes.
        public const double BackgroundRefreshFrequency = 1800;

        // Balance to send out warning at
        public const double BalanceWarningLimit = 5.0;

        public static class SettingNames
        {
            public const string CardDetails = "CardDetails";
        }

        public static class EventNames
        {
            public const string OnResume = "OnResume";
        }
    }
}
