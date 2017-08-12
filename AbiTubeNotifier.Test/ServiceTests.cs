using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using AbiTubeNotifier.Services;
using System.Configuration;

namespace AbiTubeNotifier.Test
{
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public async Task TestMethod1Async()
        {
            var service = new AbiNotifierService(
                CloudStorageAccount.DevelopmentStorageAccount,
                ConfigurationManager.AppSettings["TableName"],
                ulong.Parse(ConfigurationManager.AppSettings["DiscordWebhookId"]),
                ConfigurationManager.AppSettings["DiscordWebhookToken"],
                ConfigurationManager.AppSettings["YouTubeApiKey"]);
            await service.RunAsync();
        }
    }
}
