
namespace AcmeBank.Models;

public record AccountSummaryPageModel (
    string UserName,
    IEnumerable<IdText> Accounts
    );

