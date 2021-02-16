using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace notes.api.tests.HelperTests
{
    public static class HttpExtensions
    {
        public static StringContent ToJsonContent<T>(this T obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
        public static T ToObject<T>(this HttpResponseMessage message)
        {
            return JsonConvert.DeserializeObject<T>(message.Content.ReadAsStringAsync().Result);
        }
    }
}
