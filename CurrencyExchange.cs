using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Contains all the variables and logic needed for currency conversion
namespace Currency_Converter_App_Namespace {
    internal class CurrencyExchange {

        private string fromCurrencyName;
        private string toCurrencyName;
        private decimal exchangeRate;
        private decimal amountToExchange;

        public ExchangeOperations exchangeOperations = new ExchangeOperations();  // Class for loading xml file and getting currencies, rates and date. 

        // At init
        public CurrencyExchange() {
            LoadRates();
        }
        public void LoadRates() {
            ExchangeOperations.LoadRates();  // Reads in XML file
        }
        //Setters and Getters
        public string[] GetAllCurrencies() {
            return ExchangeOperations.Currencies.ToArray();
        }

        public string GetDate() {
            return ExchangeOperations.date;
        }

        public string GetToCurrencyName() {
            return toCurrencyName;
        }
        public void SetToCurrencyName(string toCurrencyName) {
            this.toCurrencyName = toCurrencyName;
        }
        public string GetFromCurrencyName() {
            return fromCurrencyName;
        }
        public void SetFromCurrencyName(string fromCurrencyName) {
            this.fromCurrencyName = fromCurrencyName;
        }

        public decimal GetAmountToExchange() {
            return amountToExchange;
        }
        public void SetAmountToExchange(decimal amountToExchange) {
            this.amountToExchange = amountToExchange;           
        }

        public decimal GetExchangeRate() {
            return exchangeRate;
        }

        //Calculates the exchange rate based on the Exchange rates to EUR of the two currencies. 
        public void CalcExchangeRate() {

            exchangeRate = ExchangeOperations.ExchangeRateToEuro[toCurrencyName] /
                                        ExchangeOperations.ExchangeRateToEuro[fromCurrencyName];
        }

        // Calculate the converted amount
        public decimal CalculateNewAmount() {
            return amountToExchange * exchangeRate;
        }

        public bool RatesAreLoaded() {
            if (ExchangeOperations.date != null) { return true; }
            return false;
        }

    }
}
