
namespace AcmeLib;

public class ScheduledBillPay
{
    public ScheduledBillPay()
    { }
    public ScheduledBillPay(int Id, DateOnly Scheduled, decimal Amount, string Payee)
    {
        this.Id = Id;
        this.Scheduled = Scheduled.ToDateTime(TimeOnly.MinValue);
        this.Amount = Amount;
        this.Payee = Payee;
    }

    public int Id { get;  set; }
    public DateTime Scheduled { get;  set; }
    public decimal Amount { get;  set; }
    public string Payee { get;  set; }
}
