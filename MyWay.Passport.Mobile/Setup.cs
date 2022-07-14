using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace MyWay.Passport.Mobile
{
    /// <summary>
    /// Handles setup of configuration.
    /// </summary>
    public static class Setup
    {
        public static ConfigurationBuilder Configuration => new ConfigurationBuilder();

        public static ConfigurationBuilder ConfigureSharedProject(this ConfigurationBuilder builder)
        {
            builder.AddJsonFile(new EmbeddedFileProvider(typeof(Setup).Assembly), "appsettings.json", false, false);

            return builder;
        }

        public static ConfigurationBuilder ConfigurePlatformProject(this ConfigurationBuilder builder,
            Action<ConfigurationBuilder> configure)
        {
            configure(builder);

            return builder;
        }
    }
}

