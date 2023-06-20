namespace AcmeLib;

public class BankService : IBankService
{
    private BankDbContext ctx;

    public BankService(BankDbContext ctx)
    {
        this.ctx = ctx;
    }

    public IEnumerable<IdText> GetAccounts(string email)
    {
        return ctx.Customers
            .Single(c => c.Email == email)
            .Accounts
            .Select(a => new IdText(a.Id, a.AccountType.ToString()));
    }

    public string GetUserName(string email)
    {
        return ctx.Customers
            .Where(c => c.Email == email)
            .Select(c => c.Name)
            .First();
    }
}
