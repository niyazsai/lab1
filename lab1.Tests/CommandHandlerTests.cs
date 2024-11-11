namespace lab1.Tests;
using lab1;

[TestFixture]
public class CommandHandlerTests
{
    private ShoppingListRepository repository;
    private CommandHandler commandHandler;
    private TextReader originalConsoleIn;
    private TextWriter originalConsoleOut;

    [SetUp]
    public async Task SetUp()
    {
        // Сохраняем оригинальные потоки
        originalConsoleIn = Console.In;
        originalConsoleOut = Console.Out;

        repository = new ShoppingListRepository(new JsonSerializer());
        commandHandler = await CommandHandler.Initialize(repository);
    }


    [Test]
    public void GetShoppingLists_ReturnsShoppingLists()
    {
        // Arrange
        var testList = new ShoppingList("Test List");
        commandHandler.GetShoppingLists().Add(testList);

        // Act
        var result = commandHandler.GetShoppingLists();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Test List", result[0].Name);
    }

    [Test]
    public void CreateNewList_AddsNewListToShoppingLists()
    {
        // Arrange
        var input = new StringReader("Test List\nApple\nFruit\nBanana\nFruit\nготово\n");
        Console.SetIn(input);
        var output = new StringWriter();
        Console.SetOut(output);
    
        // Act
        commandHandler.CreateNewList();
    
        // Assert
        var shoppingLists = commandHandler.GetShoppingLists();
        Assert.AreEqual(1, shoppingLists.Count);
        Assert.AreEqual("Test List", shoppingLists[0].Name);
        Assert.AreEqual(2, shoppingLists[0].Products.Count);
        Assert.AreEqual("Apple", shoppingLists[0].Products[0].Name);
        Assert.AreEqual("Fruit", shoppingLists[0].Products[0].Category);
        Assert.AreEqual("Banana", shoppingLists[0].Products[1].Name);
        Assert.AreEqual("Fruit", shoppingLists[0].Products[1].Category);
        
    }

    [Test]
    public void ViewLists_NoLists_DisplaysNoListsMessage()
    {
        // Arrange
        var input = new StringReader("");
        Console.SetIn(input);
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        commandHandler.ViewLists();

        // Assert
        var consoleOutput = output.ToString();
        Assert.IsTrue(consoleOutput.Contains("Списков пока нет"));

        // Cleanup
        Console.SetIn(new StreamReader(Console.OpenStandardInput()));
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
    }

    [Test]
    public void ViewLists_WithLists_DisplaysLists()
    {
        // Arrange
        var testList = new ShoppingList("Test List");
        commandHandler.GetShoppingLists().Add(testList);
        var input = new StringReader("1\n6\n");
        Console.SetIn(input);
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        commandHandler.ViewLists();

        // Assert
        var consoleOutput = output.ToString();
        Assert.IsTrue(consoleOutput.Contains("Доступные списки:"));
        Assert.IsTrue(consoleOutput.Contains("1. Test List"));

        // Cleanup

        Console.SetIn(new StreamReader(Console.OpenStandardInput()));
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
    }

    [Test]
    public void RemoveProduct_RemovesProductFromList()
    {
        // Arrange
        var testList = new ShoppingList("Test List");
        var product = new Product("Apple", "Fruit");
        testList.AddProduct(product);
        commandHandler.GetShoppingLists().Add(testList);

        var input = new StringReader("1\n");
        Console.SetIn(input);
        var output = new StringWriter();
        Console.SetOut(output);

        // Используем рефлексию для вызова приватного метода RemoveProduct
        var removeProductMethod = typeof(CommandHandler).GetMethod("RemoveProduct", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        removeProductMethod.Invoke(commandHandler, new object[] { testList });

        // Assert
        Assert.AreEqual(0, testList.Products.Count, "Продукт не был удален из списка.");
        StringAssert.Contains("Товар успешно удален", output.ToString());

        // Cleanup
        Console.SetIn(new StreamReader(Console.OpenStandardInput()));
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
    }

    [Test]
    public void Test_MarkPurchase_MarksProductAsPurchased()
    {
        // Arrange
        var shoppingList = new ShoppingList();
        shoppingList.AddProduct(new Product("Молоко", "Молочные продукты"));
        shoppingList.AddProduct(new Product("Хлеб", "Хлебобулочные изделия"));

        // Симулируем ввод пользователя: выбираем первый товар
        var input = new StringReader("1\n");
        Console.SetIn(input);

        // Захватываем вывод консоли
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        var method = typeof(CommandHandler).GetMethod("MarkPurchase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(commandHandler, new object[] { shoppingList });

        // Assert
        Assert.IsTrue(shoppingList.Products[0].IsPurchased);
        Assert.IsFalse(shoppingList.Products[1].IsPurchased);
    }

    [Test]
    public void Test_EditList_AddsProductsToShoppingList()
    {
        // Arrange
        var shoppingList = new ShoppingList();

        // Симулируем ввод пользователя: добавляем два товара и вводим 'готово'
        var inputString = "Яблоки\nФрукты\nБананы\nФрукты\nготово\n";
        var input = new StringReader(inputString);
        Console.SetIn(input);

        // Захватываем вывод консоли
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        var method = typeof(CommandHandler).GetMethod("EditList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(commandHandler, new object[] { shoppingList });

        // Assert
        Assert.AreEqual(2, shoppingList.Products.Count);
        Assert.AreEqual("Яблоки", shoppingList.Products[0].Name);
        Assert.AreEqual("Фрукты", shoppingList.Products[0].Category);
        Assert.AreEqual("Бананы", shoppingList.Products[1].Name);
        Assert.AreEqual("Фрукты", shoppingList.Products[1].Category);
    }

    [Test]
    public void Test_ViewHistory_DisplaysHistory()
    {
        // Arrange
        var shoppingList = new ShoppingList();
        shoppingList.History.Changes.Add("Добавлено: Молоко");
        shoppingList.History.Changes.Add("Добавлено: Хлеб");

        // Захватываем вывод консоли
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        var method = typeof(CommandHandler).GetMethod("ViewHistory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(commandHandler, new object[] { shoppingList });

        // Assert
        var consoleOutput = output.ToString();
        Assert.IsTrue(consoleOutput.Contains("История изменений:"));
        Assert.IsTrue(consoleOutput.Contains("Добавлено: Молоко"));
        Assert.IsTrue(consoleOutput.Contains("Добавлено: Хлеб"));
    }

    [TearDown]
    public void TearDown()
    {
        // Восстанавливаем консольный ввод/вывод
        Console.SetIn(originalConsoleIn);
        Console.SetOut(originalConsoleOut);
    }
}


