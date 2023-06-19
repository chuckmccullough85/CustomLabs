namespace AcmeLib;

public class Account
{
    public Account()
    {}
    public int Id { get; set; }
    public string Number { get; set; } = "000";
    public AccountType AccountType { get; set; } = AccountType.Checking;
    public decimal BeginningBalance { get; set; } = 0;
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public decimal Balance 
        => BeginningBalance + Transactions.Sum(t => t.Amount);
}