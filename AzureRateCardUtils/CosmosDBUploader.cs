using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security;
using System.Threading.Tasks;

namespace AzureRateCardUtils
{
    public class CosmosDBUploader
    {
        // CosmosDB SQL client
        private DocumentClient client;

        private static readonly string endpointUrl = ConfigurationManager.AppSettings["endpointURL"];

        private static readonly string authorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];

        private static readonly string databaseId = ConfigurationManager.AppSettings["DatabaseId"];

        private static readonly string collectionId = ConfigurationManager.AppSettings["CollectionId"];

        public CosmosDBUploader()
        {
            client = new DocumentClient(new Uri(endpointUrl), authorizationKey);

            // Set retry options high during initialization (default values).
            client.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = 30;
            client.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = 9;

        }

        public async Task BulkUpload<T>(List<T> list)
        {
            var collectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
            Console.WriteLine("CollectionLink... " + collectionLink);

            foreach (var item in list)
            {
                //await client.CreateDocumentAsync(collectionLink, item);
                await client.UpsertDocumentAsync(collectionLink, item, null, true);
            }
 
        }

    }
}
