namespace AcmeTest
{
    public class AccountTests
    {
        [Fact]
        public void TestAccountBalance()
        {
            Account account = new Account() { BeginningBalance = 0 } ;
            var t1 = new Transaction() { Amount = 100 };
            var t2 = new Transaction() { Amount = -50 };
            account.Transactions.Add(t1);   
            account.Transactions.Add(t2);
            Assert.Equal(50m, account.Balance);
        }
    }
}