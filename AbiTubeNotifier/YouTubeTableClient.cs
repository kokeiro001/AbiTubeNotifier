using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AbiTubeNotifier
{
    public class YouTubeTableClient
    {
        private readonly CloudStorageAccount storageAccount;
        private readonly string tableName;

        readonly CloudTable table;

        public YouTubeTableClient(CloudStorageAccount storageAccount, string tableName)
        {
            this.storageAccount = storageAccount;
            this.tableName = tableName;
            table = GetCloudTableAsync().Result;
        }

        private async Task<CloudTable> GetCloudTableAsync()
        {
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }

        public YouTubeEntity GetLatestItem()
        {
            var from = DateTime.UtcNow.AddMonths(-1);
            var propertyName = nameof(YouTubeEntity.PublishedAt);
            var filter1 = TableQuery.GenerateFilterConditionForDate(propertyName, QueryComparisons.GreaterThanOrEqual, from);

            var query = new TableQuery<YouTubeEntity>().Where(filter1);
            var items = table.ExecuteQuery(query).OrderByDescending(x => x.PublishedAt);
            return items.FirstOrDefault();
        }

        public async Task InsertAsync(YouTubeEntity tableEntity)
        {
            var op = TableOperation.InsertOrReplace(tableEntity);
            try
            {
                await table.ExecuteAsync(op);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class YouTubeEntity : TableEntity
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public DateTime PublishedAt { get; set; }

        public YouTubeEntity()
        {
        }

        public YouTubeEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
    }
}
