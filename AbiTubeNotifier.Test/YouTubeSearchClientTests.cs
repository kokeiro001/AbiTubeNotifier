using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace AbiTubeNotifier.Tests
{
    [TestClass()]
    public class YouTubeSearchClientTests
    {
        [TestMethod()]
        public async Task SearchAsyncTest()
        {
            var apiKey = ConfigurationManager.AppSettings["YouTubeApiKey"];
            var searchClient = new YouTubeSearchClient(apiKey);
            var result = await searchClient.SearchAsync("アビゲイル");
            result.Count().IsNot(0);
        }
    }
}