using Newtonsoft.Json;

namespace Backend.Services
{

    public class IJsonAppData
    {
        public string apiKey { get; set; }
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
    }
}
