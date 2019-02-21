using AzureRateCardUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace AzureRateCardUpload
{
    public class Program
    {

        public Program() { }

        // CosmosDBUploader Helper
        private CosmosDBUploader uploder = new CosmosDBUploader(GetAppSetting("endpointURL"), GetAppSetting("AuthorizationKey"));

        // download and process rate card 'OfferID' and 'Currency' can be changed
        private AzureRateCardDownloader downloader = new AzureRateCardDownloader(
               GetAppSetting("clientId"),
               GetAppSetting("client_secret"),
               GetAppSetting("tenantId"),
               GetAppSetting("subscriptionId"));
        //downloader.OfferId = "MS-AZR-0003P"; //PAYG
        //downloader.Currency = "KRW";
        //downloader.RegionInfo = "KR";
        //downloader.Locale = "en-US";

        private void ProcessPAYG()
        {
            try
            {
                AzureRateCard rateCard = downloader.DownloadPAYG().Result;

                uploder.BulkUpload(rateCard.Meters, GetAppSetting("DatabaseId"), GetAppSetting("PAYGCollectionId")).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine("Import failed:" + e.Message);
            }
        }

        private void ProcessRI()
        {
            try
            {
                AzureRIRateCard rateCard = downloader.DownloadRI(AzureRateCardDownloader.RIType.oneYear).Result;
                List<RIRateCard> convertedRIRateCardList = new List<RIRateCard>();

                foreach (var item in rateCard.Offers)
                {
                    convertedRIRateCardList.Add(new RIRateCard(item.Key, item.Value));
                }

                uploder.BulkUpload(convertedRIRateCardList, GetAppSetting("DatabaseId"), GetAppSetting("RICollectionId")).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine("Import failed:" + e.Message);
            }
        }

        private void TestRI()
        {
            try
            {
                AzureRIRateCard rateCard = downloader.DownloadRI(AzureRateCardDownloader.RIType.oneYear).Result;

                foreach (var item in rateCard.Offers)
                {
                    foreach (var region in item.Value.Prices)
                    {
                        var msg = $"{item.Key} - price for {region.Key} is {region.Value.Value}\n";
                        Console.Write(msg);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Import failed:" + e.Message);
            }
            
        }

        public void ImportAll()
        {
            this.ProcessPAYG();
            this.ProcessRI();
        }

        private static string GetAppSetting(string setting)
        {
            var value = ConfigurationManager.AppSettings[setting];
            if (value == null)
            {
                value = Environment.GetEnvironmentVariable("APPSETTING_" + setting);
            }

            return value;
        }

    }
}
