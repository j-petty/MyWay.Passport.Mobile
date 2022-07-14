using System;
using Microsoft.Extensions.Configuration;

namespace MyWay.Passport.Mobile.iOS
{
    /// <summary>
    /// Handles setup of configuration.
    /// </summary>
    public class Setup
    {
        public static Action<ConfigurationBuilder> Configuration => (builder) =>
        {
            // Add any project specific configuration here
        };
    }
}

