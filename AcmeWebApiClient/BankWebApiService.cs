
using AcmeClient;
using System.Text;
using System.Text.Json;

namespace AcmeWebApiClient;

public class BankWebApiService : IBankService
{
    private HttpClient client;
    public BankWebApiService(HttpClient httpClient)
    {
        client = httpClient;
        client.BaseAddress = new Uri(BaseURL);
    }

    public string BaseURL { get; set; } = "https://localhost:7049/api/AcmeBankApi/";

    public async Task DeleteScheduledBillPay(string email, int id)
    {
        var result = await client.DeleteAsync($"deletescheduledbillpay?email={email}&id={id}");
        if (!result.IsSuccessStatusCode) throw new Exception($"Delete failed {result.StatusCode}");
    }

    record JsonAcctDetail(string accountNum, decimal beginningBalance, decimal endingBalance);
    public async Task<(string AcctNum, decimal BBal, decimal EBal)> GetAccountDetail(string email, int id)
    {
        var stream = await client.GetStreamAsync($"getaccountdetail?email={email}&accountId={id}");
        var result = await JsonSerializer.DeserializeAsync<JsonAcctDetail>(stream);
        if (result == null) throw new Exception("Account not found");
        return (result.accountNum, result.beginningBalance, result.endingBalance);
    }

    public async Task<IEnumerable<IdText>> GetAccounts(string email)
    {
        JsonSerializerOptions opts = new() { PropertyNameCaseInsensitive = true };
        var stream = await client.GetStreamAsync($"getaccounts?email={email}");
        var result = await JsonSerializer.DeserializeAsync<IdText[]>(stream, opts);
        if (result == null) throw new Exception("Customer not found");
        return result;
    }

    public async Task<CustomerProfile> GetCustomer(string email)
    {
        JsonSerializerOptions opts = new() { PropertyNameCaseInsensitive = true };

        await using Stream stream = await client.GetStreamAsync($"getcustomer?email={email}");
        var result = await JsonSerializer.DeserializeAsync<CustomerProfile>(stream, opts);
        if (result == null) throw new Exception("Customer not found");
        return result;
    }

    public async Task<IEnumerable<ScheduledBillPay>> GetScheduledBillPayItems(string email)
    {
        JsonSerializerOptions opts = new() { PropertyNameCaseInsensitive = true };

        var stream = await client.GetStreamAsync($"getscheduledbillpayitems?email={email}");
        var result = await JsonSerializer.DeserializeAsync<ScheduledBillPay[]>(stream, opts);
        if (result == null) throw new Exception("Customer not found");
        return result;
    }

    public async Task<IEnumerable<TransactionDetail>> GetTransactions(string email, int id)
    {
        JsonSerializerOptions opts = new() { PropertyNameCaseInsensitive = true };
        var stream = await client.GetStreamAsync($"gettransactions?email={email}&accountId={id}");
        var result = await JsonSerializer.DeserializeAsync<TransactionDetail[]>(stream);
        if (result == null) throw new Exception("Customer/account not found");
        return result;
    }

    public async Task<string> GetUserName(string email)
    {
        return await client.GetStringAsync($"getcustomername?email={email}");
    }

    public async Task SaveCustomer(CustomerProfile customerProfile)
    {
        var opts = new JsonSerializerOptions { PropertyNamingPolicy=JsonNamingPolicy.CamelCase };
        string json = JsonSerializer.Serialize(customerProfile, opts);

        var result = await client.PutAsync("savecustomer", 
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (result.StatusCode != System.Net.HttpStatusCode.OK) throw new Exception($"Save failed {result.StatusCode}");
    }

    public async Task ScheduleBillPay(string email, ScheduledBillPay newItem)
    {
        var opts = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        string json = JsonSerializer.Serialize(newItem, opts);
        var results = await client.PostAsync($"schedulebillpay?email={email}", 
                       new StringContent(json, Encoding.UTF8, "application/json"));
        if (results.StatusCode != System.Net.HttpStatusCode.OK) throw new Exception($"Save failed {results.StatusCode}");
    }

    public async Task Transfer(string email, int fromAcct, int toAcct, decimal amount)
    {
        var result = await client.PostAsync($"transfer?email={email}&fromAcct={fromAcct}&toAcct={toAcct}&amount={amount}", null);
        if (result.StatusCode != System.Net.HttpStatusCode.OK) throw new Exception($"Save failed {result.StatusCode}");
    }
}
