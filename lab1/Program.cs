namespace lab1;

class Program
{
    static async Task Main(string[] args)
    {
        var repository = new ShoppingListRepository(new JsonSerializer());
        var commandHandler = await CommandHandler.Initialize(repository);
        var ui = new UserInterface(repository, commandHandler);
         await ui.MainMenu();
    }
}