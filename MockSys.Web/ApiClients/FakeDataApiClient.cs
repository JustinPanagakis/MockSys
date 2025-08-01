using System.Text.Json.Serialization;

namespace MockSys.Web.ApiClients;

public class FakeDataApiClient(HttpClient httpClient)
{
    public async Task<List<User>> GetFakeUsersAsync(CancellationToken cancellationToken = default)
    {
        List<User> users = [];

        await foreach (var user in httpClient.GetFromJsonAsAsyncEnumerable<User>("/api/v1/users", cancellationToken))
        {
            if (user != null)
            {
                users.Add(user);
            }
        }

        return users;
    }

    public async Task<List<Product>> GetFakeProductsAsync(CancellationToken cancellationToken = default)
    {
        List<Product> products = [];

        await foreach (var product in httpClient.GetFromJsonAsAsyncEnumerable<Product>("/api/v1/products", cancellationToken))
        {
            if (product != null)
            {
                products.Add(product);
            }
        }

        return products;
    }

    public async Task<List<Transaction>> GetFakeTransactionsAsync(CancellationToken cancellationToken = default)
    {
        List<Transaction> transactions = [];

        await foreach (var transaction in httpClient.GetFromJsonAsAsyncEnumerable<Transaction>($"/api/v1/transactions?date={DateTime.UtcNow.Date}", cancellationToken))
        {
            if (transaction != null)
            {
                transactions.Add(transaction);
            }
        }

        return transactions;
    }
}

public record User
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}

public record Product
{

    public required Guid Id { get; set; }
    public required string Brand { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required string Category { get; set; }
}

public record Transaction
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
}