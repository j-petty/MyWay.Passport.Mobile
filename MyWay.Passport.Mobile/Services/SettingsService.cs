using System;
using System.Collections.Generic;
using System.Linq;
using MyWay.Passport.Mobile.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace MyWay.Passport.Mobile.Services
{
    public static class SettingsService
    {
        /// <summary>
        /// Retrieves list of stored Cards.
        /// </summary>
        public static List<CardDetails> CardList
        {
            get
            {
                // Retrieve Cards from Preferences
                var cardsList = GetObject<List<CardDetails>>(Constants.SettingNames.CardList);

                if (cardsList == null)
                {
                    cardsList = new List<CardDetails>();
                }

                // Retrieve deprecated CardDetials from Preferences
                // NOTE: this is for backwards compatability with versions before multi-card support
                var existingCard = GetObject<CardDetails>(Constants.SettingNames.CardDetails);

                if (existingCard != null && !cardsList.Any(card => card.CardNumber == existingCard.CardNumber))
                {
                    // Add existing Card to CardsList if it's not already there
                    cardsList.Add(existingCard);

                    // Store CardList with existing value
                    CardList = cardsList;

                    // Clear the deprecated setting
                    RemoveSetting(Constants.SettingNames.CardDetails);
                }

                return cardsList;
            }
            set
            {
                // Serialise object into JsonString
                StoreObject(Constants.SettingNames.CardList, value);

                Console.WriteLine("Saved Cards: " + value?.Count);
            }
        }

        /// <summary>
        /// Replaces a Card if it exists or adds a new one.
        /// </summary>
        /// <param name="card">Card to store.</param>
        public static void AddOrReplaceCard(CardDetails card)
        {
            // Create local copy of CardList
            var cardList = CardList;

            // Check if Card already exists
            var cardIndex = cardList.FindIndex(c => c.CardId == card.CardId);

            if (cardIndex >= 0)
            {
                // Replace Card if it already exists
                cardList[cardIndex] = card;
            }
            else
            {
                // Add new Card if it doesn't exist
                cardList.Add(card);
            }

            // Update CardList
            CardList = cardList;
        }

        /// <summary>
        /// Clears all local storage.
        /// </summary>
        public static void ClearLocalData()
        {
            Preferences.Clear();
        }

        /// <summary>
        /// Deletes a particular setting.
        /// </summary>
        /// <param name="settingName">Name of the setting to delete.</param>
        public static void RemoveSetting(string settingName)
        {
            Preferences.Remove(settingName);
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
