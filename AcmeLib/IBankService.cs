namespace AcmeLib
{
    public record IdText(int Id, string Text);
    public interface IBankService
    {
        IEnumerable<IdText> GetAccounts(string email);
        string GetUserName(string email);
    }
}