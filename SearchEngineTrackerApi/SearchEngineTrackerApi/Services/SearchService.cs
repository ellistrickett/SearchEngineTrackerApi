using SearchEngineTrackerApi.Interfaces;
using SearchEngineTrackerApi.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace SearchEngineTrackerApi.Services
{
    public class SearchService : ISearchService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SearchService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<string> Search(SearchModel searchModel)
        {
            string requestUrl = "https://www.google.co.uk/search?num=100&q=" + searchModel.SearchPhrase.Replace(" ", "+");
            string responseHtml = await GetWebPageHtml(requestUrl);

            List<int> orderNumbers = GetUrlOrderNumbers(responseHtml, searchModel.TargetUrl);

            if (orderNumbers.Count > 0)
            {
                return String.Join(",", orderNumbers.ToArray());
            }
            else
            {
                return "0";
            }
        }
        private async Task<string> GetWebPageHtml(string requestUrl)
        {
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.9999.99 Safari/537.36");

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            
            var response = await httpClient.SendAsync(request);
            using var reader = new StreamReader(response.Content.ReadAsStream());
            return reader.ReadToEnd();
        }

        private static List<int> GetUrlOrderNumbers(string htmlContent, string targetUrl)
        {
            string urlPattern = @"<a href=""https?://([^""]+)""";
            MatchCollection matches = Regex.Matches(htmlContent, urlPattern, RegexOptions.Singleline);

            List<int> orderNumbers = new List<int>();
            int orderNumber = 1;

            foreach (Match match in matches)
            {
                string url = match.Groups[1].Value;
                if (url.Contains(targetUrl))
                {
                    orderNumbers.Add(orderNumber);
                }
                orderNumber++;
            }

            return orderNumbers;
        }
    }
}
