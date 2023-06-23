
using AcmeWebApiClient;
namespace AcmeTest;

public class WebApiClientTests
{
    private const string Email = "hank@propane.com";

    [Fact]
    public void TestGetCustomer()
    {
        var client = new HttpClient();
        var subject = new BankWebApiService(client);
        var result = subject.GetCustomer(Email).Result;
        Assert.False(string.IsNullOrEmpty(result.Name));
    }

    [Fact]
    public void TestDeleteScheduledBillPay()
    {
        var client = new HttpClient();
        var subject = new BankWebApiService(client);
        var items = subject.GetScheduledBillPayItems(Email).Result;
        var id = items.First().Id;
        var count = items.Count();
        subject.DeleteScheduledBillPay(Email, id).Wait();
        var resultCount = subject.GetScheduledBillPayItems(Email).Result.Count();
        Assert.Equal(count - 1, resultCount);
    }

    [Fact]
    public async Task TestGetAccount()
    {
        var client = new HttpClient();
        var subject = new BankWebApiService(client);
        var acctId = (await subject.GetAccounts(Email)).First().Id;
        var result = await subject.GetAccountDetail(Email, acctId);
        Assert.False(string.IsNullOrEmpty(result.AcctNum));
    }

    [Fact]
    public async Task TestGetTransactions()
    {
        var client = new HttpClient();
        var subject = new BankWebApiService(client);
        var acctId = (await subject.GetAccounts(Email)).First().Id;
        var result = await subject.GetTransactions(Email, acctId);
        Assert.True(result.Count() > 0);
    }

    [Fact]
    public async Task TestGetUserName()
    {
        var client = new HttpClient();
        var subject = new BankWebApiService(client);
        var result = await subject.GetUserName(Email);
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public async Task TestSaveCustomerProfile()
    {
        var client = new HttpClient();
        var subject = new BankWebApiService(client);
        var result = await subject.GetCustomer(Email);
        var newCust = result with { Phone = "000-555-1212" };
        await subject.SaveCustomer(newCust);
        result = await subject.GetCustomer(Email);
        Assert.Equal("000-555-1212", result.Phone);
    }

    [Fact]
    public async Task TestScheduleBillPay()
    {
        var client = new HttpClient();
        var subject = new BankWebApiService(client);
        var items = subject.GetScheduledBillPayItems(Email).Result;
        var count = items.Count();
        await subject.ScheduleBillPay(Email, 
            new AcmeClient.ScheduledBillPay (0, DateOnly.FromDateTime(DateTime.Today), 123.40m, "Test"));
        var newcount = subject.GetScheduledBillPayItems(Email).Result.Count();
        Assert.Equal(count + 1, newcount);
    }

    [Fact]
    public async Task TestTransfer()
    {
        var client = new HttpClient();
        var subject = new BankWebApiService(client);
        var acctId = (await subject.GetAccounts(Email)).First().Id;
        var fromId = (await subject.GetAccounts(Email)).Last().Id;
        var count = (await subject.GetTransactions(Email, acctId)).Count();
        await subject.Transfer(Email, fromId, acctId, 123.45m);

        var newcount = (await subject.GetTransactions(Email, acctId)).Count();
        Assert.Equal(count + 1, newcount);

    }
}
