using Newtonsoft.Json;

namespace lab1;

public class JsonSerializer : ISerializer
{
    
    public async Task Serialize<T>(T data, string filePath)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        await File.WriteAllTextAsync(filePath, json);
    }

    // Асинхронный метод десериализации
    public async Task<T> Deserialize<T>(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        return JsonConvert.DeserializeObject<T>(json);
    }
}