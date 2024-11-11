namespace lab1;

public class CommandHandler
{
    private List<ShoppingList> shoppingLists;
    private readonly ShoppingListRepository repository;

    public CommandHandler(ShoppingListRepository repository)
    {
        this.repository = repository;
    }
    
    public static async Task<CommandHandler> Initialize(ShoppingListRepository repository)
    {
        var handler = new CommandHandler(repository);
        handler.shoppingLists = await repository.LoadData();
        return handler;
    }

    public List<ShoppingList> GetShoppingLists()
    {
        return shoppingLists;
    }

    public void CreateNewList()
    {
        //Console.Clear();
        string listName = InputValidator.GetNonEmptyString("Введите название нового списка: ");
        var shoppingList = new ShoppingList(listName);

        Console.WriteLine("Добавьте товары в список. Введите 'готово' для завершения.");
        while (true)
        {
            string productName = InputValidator.GetNonEmptyString("Товар: ");
            if (productName.ToLower() == "готово")
            {
                break;
            }

            Console.Write("Категория товара: ");
            string category = Console.ReadLine();

            shoppingList.AddProduct(new Product(productName, category));
        }

        shoppingLists.Add(shoppingList);
        Console.WriteLine("Список успешно создан. Нажмите любую клавишу для продолжения.");
        Console.ReadLine();
        //Console.ReadKey();
    }

    public void ViewLists()
    {
        if (shoppingLists.Count == 0)
        {
            Console.WriteLine("Списков пока нет. Нажмите любую клавишу для продолжения.");
            Console.ReadLine();
            //Console.ReadKey();
            return;
        }
        
        //Console.Clear();
        Console.WriteLine("Доступные списки:");
        for (int i = 0; i < shoppingLists.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {shoppingLists[i].Name}");
        }

        int choice = InputValidator.GetValidatedInt("Выберите список для просмотра: ", 1, shoppingLists.Count);
        OpenShoppingList(shoppingLists[choice - 1]);
    }

    private void OpenShoppingList(ShoppingList shoppingList)
    {
        while (true)
        {
            //Console.Clear();
            Console.WriteLine($"=== Список: {shoppingList.Name} ===");
            Console.WriteLine("1. Просмотреть товары");
            Console.WriteLine("2. Отметить покупку");
            Console.WriteLine("3. Изменить список");
            Console.WriteLine("4. Просмотреть историю");
            Console.WriteLine("5. Удалить товар");
            Console.WriteLine("6. Вернуться");

            int choice = InputValidator.GetValidatedInt("Выберите действие: ", 1, 6);

            switch (choice)
            {
                case 1:
                    ViewProducts(shoppingList);
                    break;
                case 2:
                    MarkPurchase(shoppingList);
                    break;
                case 3:
                    EditList(shoppingList);
                    break;
                case 4:
                    ViewHistory(shoppingList);
                    break;
                case 5:
                    RemoveProduct(shoppingList);
                    break;
                case 6:
                    return;
            }
        }
    }

    private void ViewProducts(ShoppingList shoppingList)
    {
        //Console.Clear();
        Console.WriteLine("Товары в списке:");
        for (int i = 0; i < shoppingList.Products.Count; i++)
        {
            var product = shoppingList.Products[i];
            string status = product.IsPurchased ? "(Куплено)" : "(Не куплено)";
            Console.WriteLine($"{i + 1}. {product.Name} [{product.Category}] {status}");
        }
        Console.WriteLine("Нажмите любую клавишу для продолжения.");
        Console.ReadLine();
        //Console.ReadKey();
    }

    private void MarkPurchase(ShoppingList shoppingList)
    {
        //Console.Clear();
        Console.WriteLine("Товары в списке:");
        for (int i = 0; i < shoppingList.Products.Count; i++)
        {
            var product = shoppingList.Products[i];
            string status = product.IsPurchased ? "(Куплено)" : "(Не куплено)";
            Console.WriteLine($"{i + 1}. {product.Name} [{product.Category}] {status}");
        }

        int choice = InputValidator.GetValidatedInt("Выберите номер товара для отметки: ", 1, shoppingList.Products.Count);
        shoppingList.MarkAsPurchased(choice - 1);
        Console.WriteLine("Товар успешно отмечен как купленный. Нажмите любую клавишу для продолжения.");
        Console.ReadLine();
    }

    private void EditList(ShoppingList shoppingList)
    {
        //Console.Clear();
        Console.WriteLine("Добавьте товары в список. Введите 'готово' для завершения.");
        while (true)
        {
            string productName = InputValidator.GetNonEmptyString("Товар: ");
            if (productName.ToLower() == "готово")
            {
                break;
            }

            Console.Write("Категория товара: ");
            string category = Console.ReadLine();

            shoppingList.AddProduct(new Product(productName, category));
        }
        Console.WriteLine("Список успешно обновлен. Нажмите любую клавишу для продолжения.");
        Console.ReadLine();
    }

    private void ViewHistory(ShoppingList shoppingList)
    {
        //Console.Clear();
        if (shoppingList.History.Changes.Count == 0)
        {
            Console.WriteLine("История изменений пуста.");
        }
        else
        {
            Console.WriteLine("История изменений:");
            foreach (var entry in shoppingList.History.Changes)
            {
                Console.WriteLine(entry);
            }
        }
        Console.WriteLine("Нажмите любую клавишу для продолжения.");
        Console.ReadLine();
    }

    private void RemoveProduct(ShoppingList shoppingList)
    {
        //Console.Clear();
        Console.WriteLine("Товары в списке:");
        for (int i = 0; i < shoppingList.Products.Count; i++)
        {
            var product = shoppingList.Products[i];
            Console.WriteLine($"{i + 1}. {product.Name} [{product.Category}]");
        }

        int choice = InputValidator.GetValidatedInt("Выберите номер товара для удаления: ", 1, shoppingList.Products.Count);
        shoppingList.RemoveProduct(choice - 1);
        Console.WriteLine("Товар успешно удален. Нажмите любую клавишу для продолжения.");
        Console.ReadLine();
        //Console.ReadKey();
    }
}
