namespace lab1;

public class History
{
    public List<string> Changes { get; set; } = new List<string>();

    public void AddEntry(string entry)
    {
        Changes.Add($"{DateTime.Now}: {entry}");
    }
}