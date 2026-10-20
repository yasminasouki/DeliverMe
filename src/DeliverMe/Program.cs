
using DeliverMe;
//testing the data
var burger = new MenuItem("Classic Burger", 8.50m);
var fries = new MenuItem("Fries", 2.50m);
var lemonade = new MenuItem("Lemonade", 2.00m);

var maya = new Order("Maya");
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

