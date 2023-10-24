using RestSharp;
using Newtonsoft.Json;

namespace Backend.Services
{
    public class QueryParam
    {
        public string key { get; set; }
        public string value { get; set; }

    }

    internal class OddsAPI
    {
        private readonly RestClient _restClient;
        private readonly string _baseUrl = "https://api.the-odds-api.com/v4/sports";
        public OddsAPI(string apiKey)
        {
            _restClient = new RestClient(_baseUrl);
            _restClient.AddDefaultHeader("Content-Type", "application/json");
            _restClient.AddDefaultQueryParameter("apiKey", apiKey);
        }

        public void addHeader(string key, string value)
        {
            _restClient.AddDefaultHeader(key, value);
        }

        public void addParameter(string key, string value)
        {
            _restClient.AddDefaultQueryParameter(key, value);
        }

        public async Task<IEnumerable<T>> makeRequest<T>(string subURL, List<QueryParam>? queryParams = null)
        {
            var req = new RestRequest()
            {
                Resource = subURL,
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            };

            if (queryParams != null)
            {
                foreach (QueryParam qp in queryParams)
                {
                    req.AddQueryParameter(qp.key, qp.value);
                }
            }

            var response = await _restClient.ExecuteAsync(req);
            if (response == null)
            {
                return Enumerable.Empty<T>();
            }
            var res = JsonConvert.DeserializeObject<IEnumerable<T>>(response.Content);
            return res;

        }

    }
}
