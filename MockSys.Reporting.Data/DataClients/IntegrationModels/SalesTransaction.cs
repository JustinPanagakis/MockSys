using MockSys.Reporting.Data.Entities;

namespace MockSys.Reporting.Data.DataClients.IntegrationModels;

public record SalesTransaction
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Tax { get; set; }

    public SyncTransaction ToSyncTransaction()
    {
        return new()
        {
            Id = Id,
            Timestamp = Timestamp,
            UserId = UserId,
            UserName = UserName,
            UserEmail = UserEmail,
            ProductId = ProductId,
            ProductName = ProductName,
            Brand = Brand,
            Category = Category,
            Price = Price,
            Tax = Tax,
        };
    }
}