namespace AcmeLib;

public class BankService : IBankService
{
    private BankDbContext ctx;

    public BankService(BankDbContext ctx)
    {
        this.ctx = ctx;
    }

    private Account GetAccount(string email, int accountId)
    {
        return ctx.Customers
            .Single(c => c.Email == email)
            .Accounts
            .Single(a => a.Id == accountId);
    }

    public (string AcctNum, decimal BBal, decimal EBal) GetAccountDetail(string email, int id)
    {
        var a = GetAccount(email, id);
        return (a.Number, a.BeginningBalance, a.Balance);
    }
    public IEnumerable<TransactionDetail> GetTransactions(string email, int id)
    {
        return GetAccount(email, id)
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

    public CustomerProfile GetCustomer(string email)
    {
        var c = ctx.Customers.Single(c => c.Email == email);
        return new CustomerProfile(c.Id, c.Name, c.Email, c.Phone, c.HomeAddress, c.BillingAddress);
    }

    public void SaveCustomer(CustomerProfile customerProfile)
    {
        var cust = ctx.Customers.Single(c => c.Id == customerProfile.Id);
        cust.Name = customerProfile.Name;
        cust.Email = customerProfile.Email;
        cust.Phone = customerProfile.Phone;

        if (cust.HomeAddress == null)
        {
            cust.HomeAddress = customerProfile.HomeAddress;
        }
        else
        {
            cust.HomeAddress.Street = customerProfile.HomeAddress?.Street ?? "";
            cust.HomeAddress.City = customerProfile.HomeAddress?.City ?? "";
            cust.HomeAddress.State = customerProfile.HomeAddress?.State ?? "";
            cust.HomeAddress.ZipCode = customerProfile.HomeAddress?.ZipCode ?? "";
        }
        if(cust.BillingAddress == null)
        {
            cust.BillingAddress = customerProfile.BillingAddress;
        }
        else
        {
            cust.BillingAddress.Street = customerProfile.BillingAddress?.Street ?? "";
            cust.BillingAddress.City = customerProfile.BillingAddress?.City ?? "";
            cust.BillingAddress.State = customerProfile.BillingAddress?.State ?? "";
            cust.BillingAddress.ZipCode = customerProfile.BillingAddress?.ZipCode ?? "";
        }

        ctx.SaveChanges();
    }

    public void Transfer(string email, int fromAcct, int toAcct, decimal amount)
    {
        var debitAccount = GetAccount(email, fromAcct);
        var creditAccount = GetAccount(email, toAcct);
        debitAccount.Transfer(creditAccount, amount);
        ctx.SaveChanges();
    }

}
