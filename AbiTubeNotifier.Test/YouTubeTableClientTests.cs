using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;

namespace AbiTubeNotifier.Tests
{
    [TestClass()]
    public class YouTubeTableClientTests
    {
        [TestMethod()]
        public void GetLatestItemTest()
        {
            var tableName = ConfigurationManager.AppSettings["TableName"];
            var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            var tableClient = new YouTubeTableClient(storageAccount, tableName);
            tableClient.GetLatestItem();
        }
    }
}