namespace AcmeLib;

public class BankService : IBankService
{
    private BankDbContext ctx;

    public BankService(BankDbContext ctx)
    {
        this.ctx = ctx;
    }

    public (string AcctNum, decimal BBal, decimal EBal) GetAccountDetail(string email, int id)
    {
        var a = ctx.Customers
            .Single(c => c.Email == email)
            .Accounts.Single(a => a.Id == id);
        return (a.Number, a.BeginningBalance, a.Balance);
    }
    public IEnumerable<TransactionDetail> GetTransactions(string email, int id)
    {
        return ctx.Customers
            .Single(c => c.Email == email)
            .Accounts
            .Single(a => a.Id == id)
            .Transactions
            .Select(t => new TransactionDetail(t.Date, t.Description ?? "", t.Amount))
            .ToList();
    }

    public IEnumerable<IdText> GetAccounts(string email)
    {
        return ctx.Customers
            .Single(c => c.Email == email)
            .Accounts
            .Select(a => new IdText(a.Id, a.AccountType.ToString()))
            .ToList();
    }


    public string GetUserName(string email)
    {
        return ctx.Customers
            .Where(c => c.Email == email)
            .Select(c => c.Name)
            .First();
    }
}
