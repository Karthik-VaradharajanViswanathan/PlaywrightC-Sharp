using Newtonsoft.Json;
using System.Diagnostics;

namespace playwrightcs.utilities.helpers;

public class JsonObjectHelper<T> where T : class
{
    public JsonObjectHelper()
    { }

    public static async Task<T> GetAsync()
    {
        return await JsonDataReaderAsync();
    }

    private static async Task<T> JsonDataReaderAsync()
    {
        try
        {
            dynamic obj = Activator.CreateInstance(typeof(T));
            string jsonFileName = obj.FileName;

            string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string filePath = $@"{dir}\testdata\";
            string fullPathAndFileName = $"{filePath + jsonFileName}";
            string dataAsJson = "";

            using FileStream stream = new(fullPathAndFileName, FileMode.Open);
            using StreamReader reader = new(stream);

            dataAsJson = await reader.ReadToEndAsync();

            return JsonConvert.DeserializeObject<T>(dataAsJson);
        }
        catch (Exception e)
        {
            Debug.Fail("failed to load: " + e.Message);
            return null;
        }
    }
}