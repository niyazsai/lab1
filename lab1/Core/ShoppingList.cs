namespace lab1;

public class ShoppingList
{
    public string Name { get; set; }
    private List<Product> products = new List<Product>();
    private History history = new History();

    public List<Product> Products => products;
    public History History => history;

    public ShoppingList() { }  

    public ShoppingList(string name)
    {
        Name = name;
    }

    public void AddProduct(Product product)
    {
        products.Add(product);
        history.AddEntry($"Добавлен товар '{product.Name}'");
    }

    public void RemoveProduct(int index)
    {
        if (index >= 0 && index < products.Count)
        {
            var productName = products[index].Name;
            products.RemoveAt(index);
            history.AddEntry($"Удален товар '{productName}'");
        }
    }

    public void MarkAsPurchased(int index)
    {
        if (index >= 0 && index < products.Count)
        {
            products[index].IsPurchased = true;
            products[index].PurchaseDate = DateTime.Now;
            history.AddEntry($"Товар '{products[index].Name}' отмечен как купленный");
        }
    }
}