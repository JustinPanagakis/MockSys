using MockSys.FakeData.Data.Entities;
using System.Text.Json;

namespace MockSys.FakeData.Data.Providers;

public class JsonDataProvider
{
    private const string UserFile = "Users";
    private const string ProductFile = "Products";

    private List<User> _users = null!;
    private List<Product> _products = null!;

    public IEnumerable<User> Users
    {
        get
        {
            _users ??= GetData<User>(UserFile);
            return _users;
        }
    }
    public IEnumerable<Product> Products
    {
        get
        {
            _products ??= GetData<Product>(ProductFile);
            return _products;
        }
    }

    private static List<T> GetData<T>(string fileName) where T : class
    {
        try
        {
            string exeRoot = AppContext.BaseDirectory;
            string jsonPath = Path.Combine(exeRoot, "LocalData", $"{fileName}.json");
            string jsonContent = File.ReadAllText(jsonPath);

            return JsonSerializer.Deserialize<List<T>>(jsonContent) ?? [];
        }
        catch
        {
            throw new IOException($"Failed to read JSON file: {fileName}");
        }
    }
}
