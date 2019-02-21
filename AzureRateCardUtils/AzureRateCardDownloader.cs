using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureRateCardUtils
{
    public class AzureRateCardDownloader
    {
        private string clientId;
        private string client_secret;
        private string tenantId;
        private string subscriptionId;
        private static HttpClient client = new HttpClient();
        public string OfferId { get; set; }
        public string Currency { get; set; }
        public string Locale { get; set; }
        public string RegionInfo { get; set; }

        public enum RIType { oneYear, threeYear };

        public AzureRateCardDownloader(string clientId, string client_secret, string tenantId, string subscriptionId)
        {
            this.clientId = clientId;
            this.client_secret = client_secret;
            this.tenantId = tenantId;
            this.subscriptionId = subscriptionId;
            this.OfferId = "MS-AZR-0003P";
            this.Currency = "KRW";
            this.Locale = "en-US";
            this.RegionInfo = "KR";
        }

        public async Task<AzureRateCard> DownloadPAYG()
        {
            return await GetAzureRateCard(await GetBearerToken());
        }

        public async Task<AzureRIRateCard> DownloadRI(RIType offerType)
        {
            return await GetAzureRIRateCard(offerType);
        }

        private async Task<string> GetBearerToken()
        {
            HttpResponseMessage response;

            // Build grantstring
            //var grant_string = String.Format("grant_type=client_credentials&client_id=%s&client_secret=%s&resource=https%3A%2F%2Fmanagement.core.windows.net%2F");
            var grant_string = $"grant_type=client_credentials&client_id={clientId}&client_secret={client_secret}&resource=https%3A%2F%2Fmanagement.core.windows.net%2F";

            //var uri = String.Format("https://login.microsoftonline.com/%s/oauth2/token", tenantId);
            var uri = $"https://login.microsoftonline.com/{tenantId}/oauth2/token";

            // Request body includes grant_string
            HttpContent contentPost = new StringContent(grant_string, Encoding.UTF8, "application/x-www-form-urlencoded");

            // Execute the REST API call.
            response = await client.PostAsync(uri, contentPost);

            // Get the JSON response.
            string answerString = await response.Content.ReadAsStringAsync();

            // convert to MicrosoftLoginOAuthToken Response Object
            var token = JsonConvert.DeserializeObject<MicrosoftLoginOAuthToken>(answerString);

            // Return bearer token
            return token.Access_token;
        }

        private async Task<AzureRateCard> GetAzureRateCard(string bearer_token)
        {
            // Request headers.
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await GetBearerToken());

            // Assemble the URI for the REST API Call.
            string uri = $"https://management.azure.com/subscriptions/{subscriptionId}/providers/Microsoft.Commerce/RateCard?api-version=2015-06-01-preview&$filter=OfferDurableId%20eq%20'{OfferId}'%20and%20Currency%20eq%20'{Currency}'%20and%20Locale%20eq%20'{Locale}'%20and%20RegionInfo%20eq%20'{RegionInfo}'";

            HttpResponseMessage response;

            // Execute the REST API call.
            response = await client.GetAsync(uri);

            // Get the JSON response.
            AzureRateCard rateCard = JsonConvert.DeserializeObject<AzureRateCard>(await response.Content.ReadAsStringAsync());
            return rateCard;
        }


        private async Task<AzureRIRateCard> GetAzureRIRateCard(RIType rIType)
        {
            var ri = "three";
            if (rIType == RIType.oneYear)
            {
                ri = "one";
            }
            // Assemble the URI for the REST API Call.
            string uri = $"https://azure.microsoft.com/api/v2/pricing/virtual-machines-base-{ri}-year/calculator/?culture=en-us&discount=mosp";

            HttpResponseMessage response;

            // Execute the REST API call.
            response = await client.GetAsync(uri);

            // Get the JSON response.
            AzureRIRateCard rateCard = JsonConvert.DeserializeObject<AzureRIRateCard>(await response.Content.ReadAsStringAsync());
            return rateCard;
        }

    }

}
