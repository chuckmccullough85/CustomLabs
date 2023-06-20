
namespace AcmeTest;

public class DbTest
{

    [Fact]
    public void TestSeedDatabase()
    {
        using var ctx = new BankDbContext();
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();
        var customer = new Customer("Hank Hill", "hank@propane.com", "123-456-7890");
        customer.HomeAddress = new Address()
        { Street = "123 Main St", City = "Arlington", State = "TX", ZipCode = "76010" };

        var a1 = new Account()
        {
            AccountType = AccountType.Checking,
            Number = "123",
            BeginningBalance = 1000
        };
        var t1 = new Transaction() { Amount = 100 };
        a1.Transactions.Add(t1);
        customer.Accounts.Add(a1);
        ctx.Customers.Add(customer);
        ctx.SaveChanges();
    }

}
