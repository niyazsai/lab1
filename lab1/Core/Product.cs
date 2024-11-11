namespace lab1;

public class Product
{
    public string Name { get; set; }
    public bool IsPurchased { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string Category { get; set; }

    public Product() { }  

    public Product(string name, string category)
    {
        Name = name;
        IsPurchased = false;
        Category = category;
    }
}