using System.Security.Principal;

namespace AcmeLib;

public class Customer
{
    public Customer()
        : this("", "", "")
    { }
    public Customer(string name, string email, string phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Accounts = new List<Account>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Address? HomeAddress { get; set; }
    public Address? BillingAddress { get; set; }

    public ICollection<Account> Accounts { get; set; }

    public override bool Equals(object? obj) => obj is Customer customer &&
               Id == customer.Id;

    public override int GetHashCode() => HashCode.Combine(Id);

    public override string ToString() => $"{Id} - {Name} \t {Email}";


}