namespace lab1;

public class ShoppingListRepository
{
    private readonly ISerializer serializer;
    private const string DataFilePath = "shoppingLists_data.json";

    public ShoppingListRepository(ISerializer serializer)
    {
        this.serializer = serializer;
    }

    public async Task<List<ShoppingList>> LoadData()
    {
        if (File.Exists(DataFilePath))
        {
            return await serializer.Deserialize<List<ShoppingList>>(DataFilePath);
        }
        return new List<ShoppingList>();
    }

    public async Task SaveData(List<ShoppingList> shoppingLists)
    {
        await serializer.Serialize(shoppingLists, DataFilePath);
    }
}