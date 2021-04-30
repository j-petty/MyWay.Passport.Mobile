using System;
using MyWay.Passport.Mobile.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace MyWay.Passport.Mobile.Services
{
    public static class SettingsService
    {
        /// <summary>
        /// Used to verify identy with AWS Cognito.
        /// </summary>
        public static CardDetails CardDetails
        {
            get
            {
                // Retrieve CardDetials from Preferences
                return GetObject<CardDetails>(Constants.SettingNames.CardDetails);
            }
            set
            {
                // Serialise object into JsonString
                StoreObject(Constants.SettingNames.CardDetails, value);

                Console.WriteLine("Saved Card Details: " + value?.CardNumber);
            }
        }

        /// <summary>
        /// Clears all local storage.
        /// </summary>
        public static void ClearLocalData()
        {
            Preferences.Clear();
        }

        private static void StoreObject<T>(string settingName, T value) where T : class
        {
            // Serialise object into JsonString
            var stringVal = JsonConvert.SerializeObject(value);

            Preferences.Set(settingName, stringVal);
        }

        private static T GetObject<T>(string settingName) where T : class
        {
            // Retrieve value from Preferences
            var stringVal = Preferences.Get(settingName, null);

            if (!string.IsNullOrEmpty(stringVal))
            {
                // Deserialize returned value
                return JsonConvert.DeserializeObject<T>(stringVal);
            }

            return null;
        }
    }
}
