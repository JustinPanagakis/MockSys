using MockSys.Reporting.Data.DataClients.IntegrationModels;
using System.Net.Http.Json;

namespace MockSys.Reporting.Data.DataClients;

public interface IFakeDataApiClient
{
    Task<List<SalesTransaction>> GetSyncTransactionsAsync(DateTime date);
    IAsyncEnumerable<SalesTransaction> GetSyncTransactionsAsAsyncEnumerable(DateTime date);
}

public class FakeDataApiClient(HttpClient httpClient) : IFakeDataApiClient
{
    private const string transactionsUri = "/api/v1/transactions";

    public async Task<List<SalesTransaction>> GetSyncTransactionsAsync(DateTime date)
    {
        List<SalesTransaction> transactions = [];

        await foreach (var transaction in GetSyncTransactionsAsAsyncEnumerable(date))
        {
            if (transaction != null)
            {
                transactions.Add(transaction);
            }
        }

        return transactions;
    }

    public async IAsyncEnumerable<SalesTransaction> GetSyncTransactionsAsAsyncEnumerable(DateTime date)
    {
        await foreach (var transaction in httpClient.GetFromJsonAsAsyncEnumerable<SalesTransaction>($"{transactionsUri}?date={date}"))
        {
            if (transaction is not null)
            {
                yield return transaction;
            }
        }
    }
}