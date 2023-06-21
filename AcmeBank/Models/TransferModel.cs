using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AcmeBank.Models;

public class TransferModel 
{
    public TransferModel()
    { }

    public TransferModel(IEnumerable<IdText> accounts, string name)
    {
        Accounts = accounts.Select(a => new SelectListItem(a.Text, a.Id.ToString()));
        Name = name;
    }
    [Range(1, int.MaxValue, ErrorMessage="Please Select an account")]
    public int FromAccount { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Please Select an account")]
    public int ToAccount { get; set; }
    [Range(1.00, 9999)]
    public decimal Amount { get; set; }
    public string? Name { get; set; }
    public IEnumerable<SelectListItem>? Accounts { get; set; }
}
