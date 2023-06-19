namespace AcmeLib;

public enum AccountType
{
    Checking = 1,
    Savings,
    MoneyMarket,
    LineOfCredit
}

public class Account
{
    public int Id { get; set; }
    public string Number { get; set; } = "000";
    public AccountType Type { get; set; } = AccountType.Checking;
    public decimal BeginningBalance { get; set; } = 0;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}