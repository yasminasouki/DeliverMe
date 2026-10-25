

using DeliverMe;
// IMenuRepository menuRepository = new InMemoryMenuRepository(); this is th eold line
const string connectionString =
    "Server=localhost;Database=DeliverMe;Trusted_Connection=True;TrustServerCertificate=True";

IMenuRepository menuRepository = new AdoMenuRepository(connectionString);   // new: SQL Server
var menuService = new MenuService(menuRepository);// here we inject menuReposto the service 

var burger = menuService.FindByName("Classic Burger");
var fries = menuService.FindByName("Fries");
var lemonade = menuService.FindByName("Lemonade");

//c1

var maya = new Order("Maya", new NoDiscount());
maya.AddItem(burger, 2);
maya.AddItem(fries, 1);
maya.AddItem(lemonade, 2);
Console.WriteLine($"C1 Subtotal: {maya.Subtotal:F2}");                 // 23.50

try { maya.AddItem(fries, 0); }
catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"C1 Quantity 0 rejected: {ex.GetType().Name}"); }

try { new Order("", new NoDiscount()); }
catch (ArgumentException ex) { Console.WriteLine($"C1 Empty name rejected: {ex.GetType().Name}"); }

//c2

var order2 = new Order("Maya", new NoDiscount());

try { order2.MoveTo(OrderStatus.Delivered); }
catch (InvalidOrderStateException ex) { Console.WriteLine($"C2 {ex.Message}"); }

try { order2.MoveTo((OrderStatus)99); }
catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"C2 99 rejected: {ex.GetType().Name}"); }

order2.MoveTo(OrderStatus.Preparing);
order2.MoveTo(OrderStatus.OnTheWay);
order2.MoveTo(OrderStatus.Delivered);
Console.WriteLine($"C2 Valid path ends at: {order2.Status}");

try { order2.MoveTo(OrderStatus.Cancelled); }
catch (InvalidOrderStateException ex) { Console.WriteLine($"C2 {ex.Message}"); }

// C3


var regular = new Order("Maya", new NoDiscount());
var student = new Order("Maya", new StudentDiscount());
foreach (var o in new[] { regular, student })
{
    o.AddItem(burger, 2);
    o.AddItem(fries, 1);
    o.AddItem(lemonade, 2);
}
Console.WriteLine($"C3 NoDiscount total: {regular.Total:F2}");         // 23.50
Console.WriteLine($"C3 Student total: {student.Total:F2}");            // 21.15


var a1 = new Address("12 Cedar Street", "Beirut");
var a2 = new Address("12 Cedar Street", "Beirut");
Console.WriteLine($"C4 a1 == a2: {a1 == a2}");                        

var o1 = new Order("Maya", new NoDiscount());
var o2 = new Order("Maya", new NoDiscount());
Console.WriteLine($"C4 o1 == o2: {o1 == o2}");                        

var a3 = a1 with { City = "Tripoli" };
Console.WriteLine($"C4 a1 after 'with': {a1}");
Console.WriteLine($"C4 new copy a3: {a3}");

var plainSet = new HashSet<PlainAddress> { new("12 Cedar Street", "Beirut"), new("12 Cedar Street", "Beirut") };
var recordSet = new HashSet<Address> { new("12 Cedar Street", "Beirut"), new("12 Cedar Street", "Beirut") };
var fixedSet = new HashSet<FixedAddress> { new("12 Cedar Street", "Beirut"), new("12 Cedar Street", "Beirut") };
Console.WriteLine($"C4 HashSet count, plain class: {plainSet.Count}"); 
Console.WriteLine($"C4 HashSet count, record: {recordSet.Count}");     
Console.WriteLine($"C4 HashSet count, fixed class: {fixedSet.Count}"); 

// C5: 


var studentOrder = new Order("Maya", new StudentDiscount());
studentOrder.AddItem(burger, 2);
studentOrder.AddItem(fries, 1);
studentOrder.AddItem(lemonade, 2);

Checkout(studentOrder, 10m); 

try { Checkout(null!, 10m); } 
catch (ArgumentNullException ex) { Console.WriteLine($"C5 Null order failed loudly: {ex.GetType().Name}"); }

//di menuService
Console.WriteLine("DI Menu:");
foreach (var item in menuService.GetMenu())
    Console.WriteLine($"  {item.Name}: {item.Price:F2}");

Console.WriteLine($"DI Items under 5.00: {menuService.CheaperThan(5m).Count}");

try { menuService.FindByName("Pizza"); }
catch (ArgumentException ex) { Console.WriteLine($"DI {ex.Message}"); }
//
void Checkout(Order order, decimal walletBalance)
{
    ArgumentNullException.ThrowIfNull(order);

    try
    {
        if (walletBalance < order.Total)
            throw new InsufficientFundsException(walletBalance, order.Total);

        Console.WriteLine($"Paid {order.Total:F2}.");
    }
    catch (InsufficientFundsException ex)
    {
        Console.WriteLine(ex.Message);
    }
}