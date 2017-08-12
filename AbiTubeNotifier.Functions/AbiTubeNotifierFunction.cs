using System;
using System.Threading.Tasks;
using AbiTubeNotifier.Services;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AbiTubeNotifier.Functions
{
    public static class AbiTubeNotifierFunction
    {
        static readonly string TableName = @"AbiTubeNotifier";

        [FunctionName("AbiTubeNotifierTimerTrigger")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var storageAccount = CloudStorageAccountUtility.GetDefaultStorageAccount();

            var discordWebhookId = ulong.Parse(CloudConfigurationManager.GetSetting("DiscordWebhookId"));
            var discordWebhookToken = CloudConfigurationManager.GetSetting("DiscordWebhookToken");
            var youtubeApiKey = CloudConfigurationManager.GetSetting("YouTubeApiKey");

            var service = new AbiNotifierService(
                storageAccount,
                TableName,
                discordWebhookId,
                discordWebhookToken,
                youtubeApiKey);

            await service.RunAsync();
        }
    }

}