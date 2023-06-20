namespace AcmeLib
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public virtual Account Account { get; set; }
    }
}