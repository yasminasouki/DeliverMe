using DeliverMe;

namespace DeliverMe.Tests;

public class OrderTests
{
    private static readonly MenuItem Burger = new("Classic Burger", 8.50m);
    private static readonly MenuItem Fries = new("Fries", 2.50m);
    private static readonly MenuItem Lemonade = new("Lemonade", 2.00m);

    [Fact]
    public void Subtotal_MayasBasket_Is23_50()
    {
        // Arrange
        var order = new Order("Maya", new NoDiscount());

        // Act
        order.AddItem(Burger, 2);
        order.AddItem(Fries, 1);
        order.AddItem(Lemonade, 2);

        // Assert
        Assert.Equal(23.50m, order.Subtotal);
    }
}