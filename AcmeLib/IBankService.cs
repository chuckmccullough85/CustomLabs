namespace AcmeLib
{
    public record CustomerProfile(int Id, string Name, string Email, string Phone, 
        Address? HomeAddress, 
        Address? BillingAddress);
    public record IdText(int Id, string Text);
    public record TransactionDetail (DateTime Date, string Description, decimal Amt);
    public interface IBankService
    {
        (string AcctNum, decimal BBal, decimal EBal) GetAccountDetail(string email, int id);
        IEnumerable<IdText> GetAccounts(string email);
        CustomerProfile GetCustomer(string email);
        IEnumerable<TransactionDetail> GetTransactions(string email, int id);
        string GetUserName(string email);
        void SaveCustomer(CustomerProfile customerProfile);
    }
}