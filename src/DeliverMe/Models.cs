namespace DeliverMe;

public enum OrderStatus { Placed, Preparing, OnTheWay, Delivered, Cancelled }

public record MenuItem(string Name, decimal Price);

public record OrderLine(MenuItem Item, int Quantity);

public class Order
{
    private readonly List<OrderLine> _lines = new();
    private readonly IDiscountPolicy _discountPolicy;

    public Order(string customerName, IDiscountPolicy discountPolicy)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("Customer name is required.", nameof(customerName));
        ArgumentNullException.ThrowIfNull(discountPolicy);

        CustomerName = customerName;
        _discountPolicy = discountPolicy;
        Status = OrderStatus.Placed;
    }

    public string CustomerName { get; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyList<OrderLine> Lines => _lines.AsReadOnly();
    public decimal Subtotal => _lines.Sum(l => l.Item.Price * l.Quantity);
    public decimal Total => _discountPolicy.Apply(Subtotal);

    public void AddItem(MenuItem item, int quantity)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);
        _lines.Add(new OrderLine(item, quantity));
    }

    public void MoveTo(OrderStatus next)
    {
        if (!Enum.IsDefined(next))
            throw new ArgumentOutOfRangeException(nameof(next), next, "Unknown order status.");

        bool allowed = (Status, next) switch
        {
            (OrderStatus.Placed, OrderStatus.Preparing) => true,
            (OrderStatus.Preparing, OrderStatus.OnTheWay) => true,
            (OrderStatus.OnTheWay, OrderStatus.Delivered) => true,
            (OrderStatus.Placed, OrderStatus.Cancelled) => true,
            (OrderStatus.Preparing, OrderStatus.Cancelled) => true,
            _ => false
        };

        if (!allowed)
            throw new InvalidOrderStateException(Status, next);

        Status = next;
    }
}