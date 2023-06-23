

namespace AcmeClient;

public class Address
{
    public Address() : this(0, String.Empty, String.Empty, String.Empty, String.Empty)
    { }
    public Address(int id, string street, string city, string state, string zip)
    {
        Id = id;
        Street = street;
        City = city;
        State = state;
        ZipCode = zip;
    }
    public int Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}
