using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pioneer.Logs.Models;

namespace Pioneer.Logs.Proxy
{
    public interface IElasticProxy
    {
        Task<Index[]> GetIndices();
        Task<SearchResponse> GetLogsAsync(string index, SearchRequest request);
    }

    /// <summary>
    /// Serves as a proxy to the ElasticSearch service.
    ///
    /// This can be used in a .net Web Api and sit behind a controller that is
    /// controlled by a secure endpoint.
    ///
    /// ElasticSearch itself is not secured, so we only expose it on an internal network. 
    /// 
    /// </summary>
    public class ElasticProxy : IElasticProxy
    {
        private readonly string _url;

        public ElasticProxy()
        {
            _url = "http://localhost:9200";
        }

        public ElasticProxy(string url)
        {
            _url = url;
        }

        /// <summary>
        /// Get all available indices: /_cat/indices?format=json
        /// </summary>
        /// <returns>Elastic Index result body</returns>
        public async Task<Index[]> GetIndices()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{_url}/_cat/indices?format=json");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Index[]>(responseBody);
            }
        }

        /// <summary>
        /// Perform a search against indices : /_search?format=json
        /// </summary>
        /// <param name="index">Comma separated list of indices to include in search</param>
        /// <param name="request">Elastic Search result body</param>
        /// <returns>Elastic search request body</returns>
        public async Task<SearchResponse> GetLogsAsync(string index, SearchRequest request)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(request.ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_url}/_cat/indices?format=json", content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SearchResponse>(responseBody);
            }
        }
    }
}
