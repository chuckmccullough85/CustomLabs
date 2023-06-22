
namespace AcmeTest;

public class BankServiceTests : IDisposable
{
    BankService svc;
    BankDbContext ctx;

    public BankServiceTests()
    {
        Seeder.Seed();
        ctx = new BankDbContext();
        svc = new BankService(ctx);
    }

    public void Dispose()
    {
        ctx.Dispose();
    }

    [Fact]
    public async Task TestGetName()
    {
        var n = await svc.GetUserName("hank@propane.com");
        Assert.Equal("Hank Hill", n);
    }
    [Fact]
    public void TestGetAccounts()
    {
        var accounts = svc.GetAccounts("hank@propane.com").Result;
        Assert.Equal(2, accounts.Count());
    }

}
