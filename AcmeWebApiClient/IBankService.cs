namespace AcmeClient
{
    public record CustomerProfile(int Id, string Name, string Email, string Phone, 
        Address? HomeAddress, 
        Address? BillingAddress);
    public record IdText(int Id, string Text);
    public record TransactionDetail (DateTime Date, string Description, decimal Amt);


    public interface IBankService
    {
        Task<(string AcctNum, decimal BBal, decimal EBal)> GetAccountDetail(string email, int id);
        Task<IEnumerable<IdText>> GetAccounts(string email);
        Task<CustomerProfile> GetCustomer(string email);
        Task ScheduleBillPay(string email, ScheduledBillPay newItem);
        Task<IEnumerable<ScheduledBillPay>> GetScheduledBillPayItems(string email);
        Task<IEnumerable<TransactionDetail>> GetTransactions(string email, int id);
        Task<string> GetUserName(string email);
        Task SaveCustomer(CustomerProfile customerProfile);
        Task Transfer(string email, int fromAcct, int toAcct, decimal amount);
        Task DeleteScheduledBillPay(string email, int id);
    }
}