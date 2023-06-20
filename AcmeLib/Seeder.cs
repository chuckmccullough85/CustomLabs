

namespace AcmeLib;

public static class Seeder
{
    public static void Seed()
    {
        using var ctx = new BankDbContext();
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();
        var customer = new Customer("Hank Hill", "hank@propane.com", "123-456-7890");
        customer.HomeAddress = new Address()
        { Street = "123 Main St", City = "Arlen", State = "TX", ZipCode = "76010" };

        var a1 = new Account()
        {
            AccountType = AccountType.Checking,
            Number = "123",
            BeginningBalance = 1000
        };

        var t1 = new Transaction() { Amount = 100, Description="Check 101" };
        a1.Transactions.Add(t1);
        a1.Transactions.Add(new Transaction() { Amount = 200, Description = "ACH deposit" });
        a1.Transactions.Add(new Transaction() { Amount = -50, Description = "Bill Pay-Visa" });
        customer.Accounts.Add(a1);

        var a2 = new Account()
        {
            AccountType = AccountType.Savings,
            Number = "456",
            BeginningBalance = 5000
        };
        a2.Transactions.Add(new Transaction() { Amount = 100, Description = "Interest" });
        a2.Transactions.Add(new Transaction() { Amount = -50, Description = "Transfer to Checking" });
        customer.Accounts.Add(a2);
        ctx.Customers.Add(customer);

        var c2 = new Customer("Peggy Hill", "peggy@boggle.com", "123-456-7890");
        c2.HomeAddress = new Address()
        { Street = "123 Main St", City = "Arlen", State = "TX", ZipCode = "76010" };
        var a3 = new Account()
        {
            AccountType = AccountType.Checking,
            Number = "789",
            BeginningBalance = 1000
        };
        a3.Transactions.Add(new Transaction() { Amount = 100, Description = "Check 101" });
        a3.Transactions.Add(new Transaction() { Amount = -200, Description = "ACH deposit" });   
        c2.Accounts.Add(a3);
        ctx.Customers.Add(c2);

        ctx.SaveChanges();
    }
}
