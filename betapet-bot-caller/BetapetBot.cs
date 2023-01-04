using System.Net;

namespace BetapetBotCaller
{
    public class BetapetBot
    {
        private HttpClient httpClient;

        private string handleRequestUrl;

        public BetapetBot(string username, string password, string hostPort)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            httpClient = new HttpClient(handler);

            handleRequestUrl = string.Format("http://localhost:{2}/bot/handleEverything?username={0}&password={1}", username, password, string.IsNullOrEmpty(hostPort) ? "5012" : hostPort);
        }

        public async Task<bool> HandleEverything()
        {
            HttpResponseMessage response = await GetResponseAsync(handleRequestUrl, HttpMethod.Post);

            return response.IsSuccessStatusCode;
        }

        private async Task<HttpResponseMessage> GetResponseAsync(string requestUrl, HttpMethod httpMethod)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage()
            {
                Method = httpMethod,
                RequestUri = new Uri(requestUrl),
            };

            return await httpClient.SendAsync(httpRequest);
        }
    }
}
