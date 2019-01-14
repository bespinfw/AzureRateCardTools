using AzureRateCardUtils;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;

namespace AzureRateCardUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            var uploder = new CosmosDBUploader();

            // read file into a string and deserialize JSON to a type
            //AzureRateCardUtils.AzureRateCard rateCard = 
            //    JsonConvert.DeserializeObject<AzureRateCardUtils.AzureRateCard>(File.ReadAllText(@"AzureRateCard_PAYG_KRW.json"));

            // download and process rate card 'OfferID' and 'Currency' can be changed
            var downloader = new AzureRateCardDownloader(
                   ConfigurationManager.AppSettings["clientId"],
                   ConfigurationManager.AppSettings["client_secret"],
                   ConfigurationManager.AppSettings["tenantId"],
                   ConfigurationManager.AppSettings["subscriptionId"]);
            //downloader.OfferId = "MS-AZR-0003P"; //PAYG
            //downloader.Currency = "KRW";
            //downloader.RegionInfo = "KR";
            //downloader.Locale = "en-US";

            AzureRateCard rateCard = downloader.Download().Result;
                
            try
            {
                uploder.BulkUpload(rateCard.Meters).Wait();
                //Console.WriteLine(rateCard.Currency);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            

            //foreach (var item in rateCard.Meters)
            //{
            //    Console.WriteLine(item.MeterSubCategory + " " + item.MeterRates["0"]);
            //}

        }
    }
}
