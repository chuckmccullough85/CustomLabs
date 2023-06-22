namespace AcmeBank.Models
{
    public record BillPayItem(int? Id, DateOnly Scheduled, decimal Amount, string Payee);

    public record BillPayPageModel(BillPayItem NewItem, IEnumerable<BillPayItem>? Items);
}
