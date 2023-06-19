

namespace AcmeTest;

public class CustomerTests
{

    [Fact]
    public void TestGetCustomerNetValue()
    {
        //Arrange
        var customer = new Customer("Hank Hill", "hank@propane.com", "123-456-7890");
        var a1 = new Account()
        {
            AccountType = AccountType.Checking, 
            BeginningBalance = 1000
        };
        var a2 = new Account()
        {
            AccountType = AccountType.Savings, 
            BeginningBalance = 2000
        };
        customer.Accounts.Add(a1);
        customer.Accounts.Add(a2);
        decimal total = customer.TotalNetValue;
        Assert.Equal(3000m, total);
    }

}
