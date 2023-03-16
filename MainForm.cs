using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Currency_Converter_App_Namespace {
    public partial class MainForm : Form {

        CurrencyExchange converter = new CurrencyExchange();  //Creates the currencyExchange object with logic and variables and attempts to load rates and currencies from the web

        public MainForm() {
            InitializeComponent();
            InitializeGUI();
        }

        // sets some starting conditions for the UI 
        private void InitializeGUI() {
            
            this.Text = "Currency Converter";
            lblResult.Text = string.Empty;
            lblRate.Text = string.Empty;
            txtAmount.Text = "1";
            SetDateText();

            //Setup the currency comboboxes           
            InitializeComboBoxes();    
            comBoxFromCurrency.Text = "SEK";
            comBoxToCurrency.Text = "EUR";
 
        }
        // Sets date or displayes not found
        private void SetDateText() {
            lblDate.ForeColor = Color.Black;
            string date = converter.GetDate();
            lblDate.Text = date is null ? "Rates not found" : "Rates updated: " + date;
            if (date is null ) { lblDate.ForeColor = Color.Red; }
        }

        //Gets currency informtion from xml file data and adds to comboboxes
        private void InitializeComboBoxes() {
            string[] allCurrencies = converter.GetAllCurrencies();  //retrieve all currencies from XML link
            comBoxFromCurrency.Items.Clear();          
            comBoxFromCurrency.Items.AddRange(allCurrencies);
            comBoxFromCurrency.AutoCompleteCustomSource.AddRange(allCurrencies);
            comBoxToCurrency.Items.Clear();
            comBoxToCurrency.Items.AddRange(allCurrencies);
            comBoxToCurrency.AutoCompleteCustomSource.AddRange(allCurrencies);
        }

        // Event handler method for clicking the button
        private void btnConvert_Click(object sender, EventArgs e) {  // The object is the object that called the event
                                                               // (the button in this case). Eventargs are info of the event
            
            // Tries to load rates, currencies and data again if this failed at init
            if(!converter.RatesAreLoaded()) { 
                converter.LoadRates();             
                if (!converter.RatesAreLoaded()) { return; }
                SetDateText();
                InitializeComboBoxes();
            }           

            // Reads amount input
            bool amountInputOK = ReadAmount();
            // Reads currency input
            bool currencyInputOK = ReadCurrencies();
        
            if (amountInputOK && currencyInputOK) {
                //calculate exchange rate and converted amount
                converter.CalcExchangeRate();
                decimal convertedAmount = converter.CalculateNewAmount();

                //display results
                this.lblRate.Text = "1 " + converter.GetFromCurrencyName() + " = " 
                    + converter.GetExchangeRate().ToString("0.000000") + " " + converter.GetToCurrencyName(); ;

                this.lblResult.Text = converter.GetAmountToExchange() + " " + converter.GetFromCurrencyName() + 
                    " = " + convertedAmount.ToString("0.00") + " " + converter.GetToCurrencyName();

            }
        }

        // Reads the content of textbox. Stores amount and returns true if amount is a decimal and not negative.
        private bool ReadAmount() {

            bool ok;
            string strAmount = txtAmount.Text;  //Text from first textbox
            strAmount = strAmount.Trim();  //deletes spaces at begining and end of field
            decimal amount = 0.0M;

            ok = decimal.TryParse(strAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out amount);  //tries to convert to decimal, false if it fails

            //Checks if amount entered is valid decimal and not negativ. If not, error message is generated
            if (ok && amount >= 0 ) {
                converter.SetAmountToExchange(amount);              
            }
            else {
                MessageBox.Show("Error: Invalid amount.");
                ok = false;
            }
            return ok;
        }
        // Reads the text of currency comboBoxes, returns true if content matches pre-defined currencies.
        private bool ReadCurrencies() {

            bool ok = false;
            string fromCurrency = comBoxFromCurrency.Text.Trim();
            string toCurrency = comBoxToCurrency.Text.Trim();

            string[] allCurrencies = converter.GetAllCurrencies();

            //Check if input text matches pre-defined currencies and stores them. If not, error message is generated 
            if (allCurrencies.Contains(fromCurrency) && allCurrencies.Contains(toCurrency)) {
                ok = true;
                converter.SetFromCurrencyName(fromCurrency);
                converter.SetToCurrencyName(toCurrency);
            }
            else if (allCurrencies.Length<1) { // Currencies are not loaded
                MessageBox.Show("Error: No currencies loaded. Check internet connection and try refreshing rates.");
            }
            else { // Currencies loaded but input was invalid             
                MessageBox.Show("Error: Invalid currency.");
            }
            return ok;
        }

        //Swaps the to and from currencies
        private void btnSwap_Click(object sender, EventArgs e) {
            string tempCurrency = comBoxFromCurrency.Text.Trim();
            comBoxFromCurrency.Text = comBoxToCurrency.Text.Trim();
            comBoxToCurrency.Text = tempCurrency;
        }

        // Trys updating rates, and loading of rates and date. 
        private void btnUpdateRates_Click(object sender, EventArgs e) {
            converter.LoadRates();  // Trys to load the rates
            SetDateText();
            InitializeComboBoxes();
        }
    }
}
