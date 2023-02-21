using System.Text.Json;

namespace read_json_file.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJsonString(this object thisObject) 
        {
            return JsonSerializer.Serialize(thisObject);
        }
        public static T DeserializeObject<T>(this string crcJson)
		{
			return JsonSerializer.Deserialize<T>(crcJson);
		}        

        public static int GetNumericOption(this string crcOptionName) 
        {
            var crcOptionNum = crcOptionName.Split('-').First();

            return Convert.ToInt32(crcOptionNum);            
        }
	}
}
