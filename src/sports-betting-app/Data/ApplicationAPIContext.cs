using Newtonsoft.Json;
using RestSharp;
using sports_betting_app.Models;

namespace sports_betting_app.Data
{

    public class ApiParam
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public interface IAPIClientService<T> where T : class
    {
        Task<List<T>> GetAll(string subURL, List<ApiParam>? apiParams = null);

        Task<List<M>> GetByType<M>(string subURL, List<ApiParam>? apiParams = null);
        Task<RestResponse> Add(T model, string subURL);
        Task<RestResponse> AddByType<M>(M model, string subURL);
        Task<RestResponse> Update(T model, string subURL);
    }

    public class ApplicationAPIContext<T> : IAPIClientService<T> where T : class
    {
        private readonly RestClient _RestClient;
        private readonly string _baseurl = "http://sports-bet-app-web-api.us-east-2.elasticbeanstalk.com/";
        public ApplicationAPIContext()
        {
            _RestClient = new RestClient(_baseurl);
        }

        public async Task<List<T>> GetAll(string subURL, List<ApiParam>? apiParams = null)
        {
            var req = new RestRequest()
            {
                Resource = subURL,
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            };

            if (apiParams != null)
            {
                foreach (ApiParam qp in apiParams)
                {
                    req.AddQueryParameter(qp.key, qp.value);
                }
            }

            var response = await _RestClient.ExecuteAsync<List<T>>(req);
            return response.Data;
        }

        public async Task<List<M>> GetByType<M>(string subURL, List<ApiParam>? apiParams = null)
        {
            var req = new RestRequest()
            {
                Resource = subURL,
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            };

            if (apiParams != null)
            {
                foreach (ApiParam qp in apiParams)
                {
                    req.AddQueryParameter(qp.key, qp.value);
                }
            }

            var response = await _RestClient.ExecuteAsync<List<M>>(req);
            return response.Data;
        }

        public async Task<RestResponse> Add(T model, string subURL)
        {
            
            var req = new RestRequest()
            {
                Resource = subURL,
                Method = Method.Post
            };

            req.AddJsonBody(model);

            RestResponse response = await _RestClient.ExecuteAsync(req);
            return response;
        }

        public async Task<RestResponse> AddByType<M>(M model, string subURL)
        {

            var req = new RestRequest()
            {
                Resource = subURL,
                Method = Method.Post
            };

            req.AddJsonBody(JsonConvert.SerializeObject(model));
            RestResponse response = await _RestClient.ExecuteAsync(req);
            return response;
        }

        public async Task<RestResponse> Update(T model, string subURL)
        {
            var req = new RestRequest()
            {
                Resource = subURL,
                Method = Method.Put
            };

            req.AddJsonBody(model);
            RestResponse response = await _RestClient.ExecuteAsync(req);
            return response;

        }
    }
}
