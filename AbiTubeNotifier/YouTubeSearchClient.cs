using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace AbiTubeNotifier
{
    public class YouTubeSearchClient
    {
        private readonly string apiKey;

        public YouTubeSearchClient(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<IEnumerable<SearchResult>> SearchAsync(string keyword)
        {
            var init = new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
            };

            var youtubeService = new YouTubeService(init);

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = keyword; // Replace with your search term.
            searchListRequest.MaxResults = 50;
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
            searchListRequest.RegionCode = "JP";

            var searchListResponse = await searchListRequest.ExecuteAsync();

            return searchListResponse.Items;
        }
    }
}
