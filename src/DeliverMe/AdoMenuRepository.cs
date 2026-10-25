using Microsoft.Data.SqlClient;

namespace DeliverMe;

public class AdoMenuRepository : IMenuRepository
{
    private readonly string _connectionString;

    public AdoMenuRepository(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string is required.", nameof(connectionString));

        _connectionString = connectionString;
    }

    public IReadOnlyList<MenuItem> GetAll()
    {
        var items = new List<MenuItem>();

        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand(
            "SELECT Name, Price FROM MenuItems ORDER BY Id", connection);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            items.Add(new MenuItem(reader.GetString(0), reader.GetDecimal(1)));
        }

        return items.AsReadOnly();
    }
}