namespace AcmeLib;

public class FakeService : IBankService
{
    public IEnumerable<IdText> GetAccounts(string email)
    {
        return new List<IdText>
        {
            new IdText(1, "Checking"),
            new IdText(2, "Savings"),
            new IdText(3, "Money Market")
        };
    }

    public string GetUserName(string email)
    {
        return "Hank Hill"; 
    }
}
