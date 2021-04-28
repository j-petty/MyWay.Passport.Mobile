﻿using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace MyWay.Passport.Mobile.Models
{
    public class CardDetails : BaseModel
    {
        private string cardNumber;
        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; OnPropertyChanged(); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }

        private DateTime? dateOfBirth;
        public DateTime? DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; OnPropertyChanged(); }
        }

        private DateTime? lastUpdated;
        public DateTime? LastUpdated
        {
            get { return lastUpdated; }
            set { lastUpdated = value; OnPropertyChanged(); }
        }

        private double? lastBalance = 0.0;
        public double? LastBalance
        {
            get { return lastBalance; }
            set { lastBalance = value; OnPropertyChanged(); }
        }

        public CardDetails()
        {
            DateOfBirth = DateTime.Today;
        }

        /// <summary>
        /// Checks if required CardDetails have been completed.
        /// </summary>
        /// <returns>Returns whether required card details are provided.</returns>
        public bool CheckFilled()
        {
            return !string.IsNullOrEmpty(CardNumber) && !string.IsNullOrEmpty(Password) && DateOfBirth != null && DateOfBirth <= DateTime.Today;
        }
    }
}
