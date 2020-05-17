using Newtonsoft.Json;

namespace BotAI.Files
{
    public class Json
    {
        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
        public static T Deserialize<T>(string content) where T : class
        {
            return (T)JsonConvert.DeserializeObject<T>(content);
        }
    }
}
