using MockSys.FakeData.Data.Entities;
using MockSys.FakeData.Data.Providers;
using MockSys.FakeData.Domain.Models;
using MockSys.FakeData.Domain.Utilities;

namespace MockSys.FakeData.Domain.Services;

public interface IFakeDataService
{
    List<User> GetUsers();
    List<Product> GetProducts();
    List<Transaction> GetTransactions(DateTime date);
}

public class FakeDataService(JsonDataProvider jsonDbContext) : IFakeDataService
{
    public List<User> GetUsers() => [.. jsonDbContext.Users];

    public List<Product> GetProducts() => [.. jsonDbContext.Products];

    public List<Transaction> GetTransactions(DateTime date)
    {
        var users = GetUsers();
        var products = GetProducts();

        var random = new Random();
        var numTransactions = random.Next(10, 100);

        var transactions = new List<Transaction>();

        for (int i = 0; i < numTransactions; i++)
        {
            var user = users[random.Next(users.Count)];
            var product = products[random.Next(products.Count)];
            var dateRandomTime = DateTimeUtilities.ConvertToUtcWithRandomTime(date);
            transactions.Add(new(user, product, dateRandomTime));
        }

        return transactions;
    }
}
