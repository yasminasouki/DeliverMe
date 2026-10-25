namespace DeliverMe;

public enum OrderStatus { Placed, Preparing, OnTheWay, Delivered, Cancelled }

public record MenuItem(string Name, decimal Price);

public record OrderLine(MenuItem Item, int Quantity);

public class Order
{
    // C1: the real list is private; only Order can change it
    private readonly List<OrderLine> _lines = new();

    // C3: the discount policy is plugged in from outside (composition)
    private readonly IDiscountPolicy _discountPolicy;

    // C1 + C3: an order needs a name and a discount policy to exist
    public Order(string customerName, IDiscountPolicy discountPolicy)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("Customer name is required.", nameof(customerName));
        ArgumentNullException.ThrowIfNull(discountPolicy);

        CustomerName = customerName;
        _discountPolicy = discountPolicy;
        Status = OrderStatus.Placed;
    }

    // C1: set once in the constructor, never changed
    public string CustomerName { get; }

    // C1: everyone can read it, only Order can change it
    public OrderStatus Status { get; private set; }

    // C1: outsiders can read the lines but not modify the list
    public IReadOnlyList<OrderLine> Lines => _lines.AsReadOnly();

    public decimal Subtotal => _lines.Sum(l => l.Item.Price * l.Quantity);

    // C3: the policy calculates the total
    public decimal Total => _discountPolicy.Apply(Subtotal);

    // C1: the only way to add a line, with checks
    public void AddItem(MenuItem item, int quantity)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);
        _lines.Add(new OrderLine(item, quantity));
    }

    // C2: the only way to change the status, following the rules
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