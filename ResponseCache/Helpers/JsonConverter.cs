using System.Text.Json;
namespace ResponseCache.Helpers
{
    public static class JsonConverter
    {
        private static JsonSerializerOptions JSONSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = false,
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        public static string SerializeObject(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static string SerializeObjectCamelCase(this object obj)
        {
            return JsonSerializer.Serialize(obj, JSONSerializerOptions());
        }

        public static T DeserializeObject<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, JSONSerializerOptions());
        }

    }
}
