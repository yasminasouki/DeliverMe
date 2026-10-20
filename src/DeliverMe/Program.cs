
using DeliverMe;
//testing the data
var burger = new MenuItem("Classic Burger", 8.50m);
var fries = new MenuItem("Fries", 2.50m);
var lemonade = new MenuItem("Lemonade", 2.00m);

var maya = new Order("Maya", new NoDiscount());
maya.AddItem(burger, 2);
maya.AddItem(fries, 1);
maya.AddItem(lemonade, 2);
Console.WriteLine($"C1 Subtotal: {maya.Subtotal}");

try
{
    maya.AddItem(fries, 0);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"C1 Quantity 0 rejected: {ex.GetType().Name}");
}

var order2 = new Order("Maya", new NoDiscount());

try
{
    order2.MoveTo(OrderStatus.Delivered);
}
catch (InvalidOrderStateException ex)
{
    Console.WriteLine($"C2 {ex.Message}");
}

try
{
    order2.MoveTo((OrderStatus)99);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"C2 99 rejected: {ex.Message}");
}

order2.MoveTo(OrderStatus.Preparing);
order2.MoveTo(OrderStatus.OnTheWay);
order2.MoveTo(OrderStatus.Delivered);
Console.WriteLine($"C2 Valid path ends at: {order2.Status}");

//c3
var regular = new Order("Maya", new NoDiscount());
var student = new Order("Maya", new StudentDiscount());

foreach (var o in new[] { regular, student })
{
    o.AddItem(burger, 2);
    o.AddItem(fries, 1);
    o.AddItem(lemonade, 2);
}

Console.WriteLine($"C3 NoDiscount total: {regular.Total}");
Console.WriteLine($"C3 Student total: {student.Total}");