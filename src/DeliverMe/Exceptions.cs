namespace DeliverMe;

public class InvalidOrderStateException : Exception
{
    public OrderStatus From { get; }
    public OrderStatus To { get; }

    public InvalidOrderStateException(OrderStatus from, OrderStatus to)
        : base($"Cannot move an order from {from} to {to}.")
    {
        From = from;
        To = to;
    }
}