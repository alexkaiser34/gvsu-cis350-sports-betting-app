using Newtonsoft.Json;

namespace Backend.Services
{

    public class AwsKeys
    {
        public string accessKey { get; set; }

        public string secretKey { get; set; }

    }

    public class IJsonAppData
    {
        public string apiKey { get; set; }

        public AwsKeys awsKeys { get; set; }

        
    }
    internal class JsonHelper
    {
        private readonly string _json;
        private readonly IJsonAppData _data;

        public JsonHelper()
        {
            using (StreamReader r = new StreamReader("../../../appSettings.json"))
            {
                _json = r.ReadToEnd();
                if (_json != null)
                {
                    _data = JsonConvert.DeserializeObject<IJsonAppData>(_json);
                }
            }
        }

        public string getApiKey()
        {
            return _data.apiKey;
        }

        public AwsKeys getAwsKeys()
        {
            return _data.awsKeys;
        }
    }
}
