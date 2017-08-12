using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;
using Microsoft.WindowsAzure.Storage;
using Discord.Webhook;

namespace AbiTubeNotifier.Services
{
    public class AbiNotifierService
    {
        private readonly CloudStorageAccount storageAccount;
        private readonly string tableName;
        private readonly ulong discordWebhookId;
        private readonly string discordWebhookToken;
        private readonly string youtubeApiKey;
        private readonly string SearchWord = @"アビゲイル";

        public AbiNotifierService(CloudStorageAccount storageAccount, string tableName, ulong discordWebhookId, string discordWebhookToken, string youtubeApiKey)
        {
            this.storageAccount = storageAccount;
            this.tableName = tableName;
            this.discordWebhookId = discordWebhookId;
            this.discordWebhookToken = discordWebhookToken;
            this.youtubeApiKey = youtubeApiKey;
        }

        public async Task RunAsync()
        {
            var searchClient = new YouTubeSearchClient(youtubeApiKey);
            var result = await searchClient.SearchAsync(SearchWord);

            var tableClient = new YouTubeTableClient(storageAccount, tableName);

            var savedLatestItem = tableClient.GetLatestItem();
            var latestPublishedAt = savedLatestItem?.PublishedAt ?? DateTime.UtcNow.AddHours(-6);

            var newItems = result
                            .Where(x => x.Snippet.PublishedAt.Value.ToUniversalTime() > latestPublishedAt)
                            .ToList();

            foreach (var item in newItems)
            {
                var tableEntity = item.ToEntity();
                try
                {
                    await tableClient.InsertAsync(tableEntity);
                    await Notify2DiscordAsync(tableEntity);
                }
                catch (Exception e)
                {
                    // TODO: log
                    Debug.WriteLine($"{e.Message} {e.StackTrace}");
                }
            }
        }

        private async Task Notify2DiscordAsync(YouTubeEntity tableEntity)
        {
            var client = new DiscordWebhookClient(discordWebhookId, discordWebhookToken);

            var text = $"https://www.youtube.com/watch?v={tableEntity.VideoId}";

            await client.SendMessageAsync(text);
        }
    }

    static class SearchResultExtentions
    {
        public static YouTubeEntity ToEntity(this SearchResult searchResult)
        {
            var videoId = searchResult.Id.VideoId;
            var publishedAt = searchResult.Snippet.PublishedAt.Value;

            return new YouTubeEntity(videoId, publishedAt.Ticks.ToString())
            {
                VideoId = videoId,
                Title = searchResult.Snippet.Title,
                PublishedAt = publishedAt,
            };
        }
    }
}
