namespace DeliverMe;

public enum OrderStatus { Placed, Preparing, OnTheWay, Delivered, Cancelled }

public record MenuItem(string Name, decimal Price);

public record OrderLine(MenuItem Item, int Quantity);

public class Order
{
    private readonly List<OrderLine> _lines = new();

    public Order(string customerName)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("Customer name is required.", nameof(customerName));

        CustomerName = customerName;
        Status = OrderStatus.Placed;
    }

    public string CustomerName { get; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyList<OrderLine> Lines => _lines.AsReadOnly();
    public decimal Subtotal => _lines.Sum(l => l.Item.Price * l.Quantity);

    public void AddItem(MenuItem item, int quantity)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);
        _lines.Add(new OrderLine(item, quantity));
    }
}
