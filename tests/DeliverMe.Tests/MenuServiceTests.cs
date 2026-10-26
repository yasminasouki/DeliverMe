using DeliverMe;

namespace DeliverMe.Tests;

public class FakeMenuRepository : IMenuRepository
{
    public IReadOnlyList<MenuItem> GetAll() => new List<MenuItem>
    {
        new("Fries", 2.50m),
        new("Classic Burger", 8.50m)
    };
}

public class MenuServiceTests
{
    [Fact]
    public void CheaperThan_ReturnsOnlyCheapItems()
    {
        // Arrange: inject the fake (DI)
        var service = new MenuService(new FakeMenuRepository());

        // Act
        var result = service.CheaperThan(5m);

        // Assert
        Assert.Single(result);
        Assert.Equal("Fries", result[0].Name);
    }

    [Fact]
    public void FindByName_IgnoresCase()
    {
        var service = new MenuService(new FakeMenuRepository());

        var item = service.FindByName("fries");

        Assert.Equal(2.50m, item.Price);
    }

    [Fact]
    public void FindByName_NotOnMenu_Throws()
    {
        var service = new MenuService(new FakeMenuRepository());

        Assert.Throws<ArgumentException>(() => service.FindByName("Pizza"));
    }

    [Fact]
    public void Constructor_NullRepository_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new MenuService(null!));
    }
}