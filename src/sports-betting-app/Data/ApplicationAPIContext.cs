using RestSharp;

namespace sports_betting_app.Data
{
    public interface IAPIClientService<T> where T : class
    {
        Task<List<T>> GetAll(string subURL);
        Task<RestResponse> Add(T model, string subURL);
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
        public async Task<List<T>> GetAll(string subURL)
        {
            var req = new RestRequest()
            {
                Resource = subURL,
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            };

            var response = await _RestClient.ExecuteAsync<List<T>>(req);
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
