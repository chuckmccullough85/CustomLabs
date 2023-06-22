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

    public async Task<(string AcctNum, decimal BBal, decimal EBal)> GetAccountDetail(string email, int id)
    {
        var a = GetAccount(email, id);
        return await Task.Run(()=>(a.Number, a.BeginningBalance, a.Balance));
    }
    public async Task<IEnumerable<TransactionDetail>> GetTransactions(string email, int id)
    {
        return GetAccount(email, id)
            .Transactions
            .Select(t => new TransactionDetail(t.Date, t.Description ?? "", t.Amount))
            .ToList();
    }

    public async Task<IEnumerable<IdText>> GetAccounts(string email)
    {
        return await Task.Run(()=> ctx.Customers
            .Single(c => c.Email == email)
            .Accounts
            .Select(a => new IdText(a.Id, a.AccountType.ToString()))
            .ToList());
    }

    public async Task<string> GetUserName(string email)
    {
        return await Task.Run(() => ctx.Customers
            .Where(c => c.Email == email)
            .Select(c => c.Name)
            .First());
    }

    public async Task<CustomerProfile> GetCustomer(string email)
    {
        var c = await Task.Run(()=> ctx.Customers.Single(c => c.Email == email));
        return new CustomerProfile(c.Id, c.Name, c.Email, c.Phone, c.HomeAddress, c.BillingAddress);
    }

    public async Task SaveCustomer(CustomerProfile customerProfile)
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

        await ctx.SaveChangesAsync();
    }

    public async Task Transfer(string email, int fromAcct, int toAcct, decimal amount)
    {
        var debitAccount = await Task.Run(()=>GetAccount(email, fromAcct));
        var creditAccount = await Task.Run(()=>GetAccount(email, toAcct));
        debitAccount.Transfer(creditAccount, amount);
        await ctx.SaveChangesAsync();
    }

    public async Task ScheduleBillPay(string email, ScheduledBillPay newItem)
    {
        var acct = ctx.Customers.Single(c=> c.Email == email).Accounts.First();
        acct.ScheduledBillPays.Add(newItem);

        await ctx.SaveChangesAsync();
    }

    public async Task<IEnumerable<ScheduledBillPay>> GetScheduledBillPayItems(string email)
    {
        return await Task.Run(()=> ctx.Customers.Single(c => c.Email == email).Accounts.First()
            .ScheduledBillPays.ToList().AsEnumerable());
    }

    public async Task DeleteScheduledBillPay(string email, int id)
    {
        var acct = ctx.Customers.Single(c => c.Email == email).Accounts.First();
        var item = acct.ScheduledBillPays.Single(i => i.Id == id);
        acct.ScheduledBillPays.Remove(item);
        await ctx.SaveChangesAsync();
    }
}
