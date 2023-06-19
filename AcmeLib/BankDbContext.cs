
using Microsoft.EntityFrameworkCore;

namespace AcmeLib;

public class BankDbContext :DbContext
{

    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("""
            Data Source=(localdb)\ProjectModels;Initial Catalog=BankDatabase;
            Integrated Security=True;Connect Timeout=30;
            Encrypt=False;Trust Server Certificate=False;
            Application Intent=ReadWrite;Multi Subnet Failover=False
            """);
        optionsBuilder.UseLazyLoadingProxies();
        base.OnConfiguring(optionsBuilder);
    }

}
