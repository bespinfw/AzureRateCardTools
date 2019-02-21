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

        public CosmosDBUploader(string endpointUrl, string authorizationKey)
        {
            client = new DocumentClient(new Uri(endpointUrl), authorizationKey);

            // Set retry options high during initialization (default values).
            client.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = 30;
            client.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = 9;
            
        }

        public async Task BulkUpload<T>(List<T> list, string databaseId, string collectionId)
        {
            var collectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
            Console.WriteLine("CollectionLink... " + collectionLink);

            foreach (var item in list)
            {
                //await client.CreateDocumentAsync(collectionLink, item);
                await client.UpsertDocumentAsync(collectionLink, item, null, true);
            }
            Console.WriteLine("Import finished.");
        }

    }
}
