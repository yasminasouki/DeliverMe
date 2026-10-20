namespace DeliverMe;

public enum OrderStatus { Placed, Preparing, OnTheWay, Delivered, Cancelled }

public record MenuItem(string Name, decimal Price);

public record OrderLine(MenuItem Item, int Quantity);

public class Order
{
    public string CustomerName { get; set; } = "";
    public OrderStatus Status { get; set; }
    public List<OrderLine> Lines { get; set; } = new();
    public decimal Subtotal => Lines.Sum(l => l.Item.Price * l.Quantity);
}