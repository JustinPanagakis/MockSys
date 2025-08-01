using MockSys.FakeData.Data.Entities;

namespace MockSys.FakeData.Domain.Models;

public record Transaction
{
    public Transaction(User user, Product product, DateTime date)
    {
        Id = Guid.NewGuid();
        Timestamp = date.ToUniversalTime();
        UserId = user.Id;
        UserName = user.FirstName + " " + user.LastName;
        UserEmail = user.Email;
        ProductId = product.Id;
        ProductName = product.Name;
        Brand = product.Brand;
        Category = product.Category;
        Price = product.Price;
        Tax = Price * 0.07M; // 7% tax on the product price
    }

    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }

    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string Brand { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public decimal Tax { get; set; }
}
