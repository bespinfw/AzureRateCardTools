using Newtonsoft.Json;
using System;
using System.IO;

namespace AzureRateCardUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            var uploder = new AzureRateCardUtils.CosmosDBUploader();

            // read file into a string and deserialize JSON to a type
            AzureRateCardUtils.AzureRateCard rateCard = 
                JsonConvert.DeserializeObject<AzureRateCardUtils.AzureRateCard>(File.ReadAllText(@"AzureRateCard_PAYG_KRW.json"));

            try
            {
                uploder.BulkUpload(rateCard.Meters).Wait();
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
