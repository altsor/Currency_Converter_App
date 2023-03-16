using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

// Class that reads in XML file of todays currency rates and stores currency abreviation and exchange rates seperatly
// It also reads and stores the current date

namespace Currency_Converter_App_Namespace {
    internal class ExchangeOperations {

        public static readonly Dictionary<string, decimal> ExchangeRateToEuro = new Dictionary<string, decimal>();
        public static List<string> Currencies = new List<string>();
        public static string date;
        public static string fileName = "CurrencyConvRatesFile.xml";
        public static XmlDocument xmlDoc = new XmlDocument();

        public static void LoadRates() {         
            try {
                xmlDoc.Load("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml");  //load xml file from web
                xmlDoc.Save(fileName);  //Save that file locally if found on web
                readXMLData();  // Reads the data from the xml file and stores in public variables
            }
            catch (System.Net.WebException) {  //If web loading failed
                try {
                    xmlDoc.Load(fileName);
                    readXMLData();
                    MessageBox.Show("Warning: Could not load current exchange rates. Rates from earlier date loaded");
                }
                catch (FileNotFoundException) {  //If web and local file reading failed
                    MessageBox.Show("Error: Could not load exchange rates. Check internet connection");                 

                }

            }
        }
        
        // Reads in currencies, rates and date from XML file
        private static void readXMLData() {
            ExchangeRateToEuro.Clear();
            Currencies.Clear();

            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes[2].ChildNodes[0].ChildNodes) {

                // Read and store rates
                ExchangeRateToEuro.Add(node.Attributes["currency"].Value, decimal.Parse(node.Attributes["rate"].Value, NumberStyles.Any, CultureInfo.InvariantCulture)); //Invariant culture to deal with comma and dot
                
                // Read and store currencies              
                Currencies.Add(node.Attributes["currency"].Value);

            }
            // Adds EUR to EUR rates the dictionary and list
            ExchangeRateToEuro.Add("EUR", 1.00M);
            Currencies.Add("EUR");

            // Read and store date from XML
            date = xmlDoc.DocumentElement.ChildNodes[2].ChildNodes[0].Attributes["time"].Value;
           
        }
        
    }
}
