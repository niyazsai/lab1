namespace lab1;

public interface ISerializer
{
    Task Serialize<T>(T data, string filePath);
    Task<T> Deserialize<T>(string filePath);
}