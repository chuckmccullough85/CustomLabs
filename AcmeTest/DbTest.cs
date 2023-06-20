
namespace AcmeTest;

public class DbTest
{

    [Fact]
    public void TestSeedDatabase()
    {
        Seeder.Seed();
    }

}
