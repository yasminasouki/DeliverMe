namespace DeliverMe;

// The rule (the socket): anything that can give us the menu
public interface IMenuRepository
{
    IReadOnlyList<MenuItem> GetAll();
}

// Version 1: the menu is just a list in memory (no database yet)
public class InMemoryMenuRepository : IMenuRepository

{
    private readonly List<MenuItem> _items = new()
    {
        new MenuItem("Classic Burger", 8.50m),
        new MenuItem("Fries", 2.50m),
        new MenuItem("Lemonade", 2.00m)
    };

    public IReadOnlyList<MenuItem> GetAll() => _items.AsReadOnly();
}
public class MenuService
{
    private readonly IMenuRepository _repository;

    public MenuService(IMenuRepository repository)//constructor which receives the repos and doesnt create it
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    public IReadOnlyList<MenuItem> GetMenu() => _repository.GetAll();

    public IReadOnlyList<MenuItem> CheaperThan(decimal maxPrice) =>
        _repository.GetAll().Where(item => item.Price < maxPrice).ToList();//puts all results intoa list

    public MenuItem FindByName(string name)
    {
        var item = _repository.GetAll()
            .FirstOrDefault(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));//fries FRIES FRies all the same

        return item ?? throw new ArgumentException($"'{name}' is not on the menu.", nameof(name));
    }
}