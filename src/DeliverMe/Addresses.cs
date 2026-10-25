namespace DeliverMe;

// C4: a record compares by value (Equals, GetHashCode and == are generated)
public record Address(string Street, string City);

// C4: a plain class compares by reference (same object in memory)
public class PlainAddress
{
    public string Street { get; }
    public string City { get; }

    public PlainAddress(string street, string city)
    {
        Street = street;
        City = city;
    }
}

// C4: the same class fixed by hand to compare by value (still a class, not a record)
public class FixedAddress : IEquatable<FixedAddress>
{
    public string Street { get; }
    public string City { get; }

    public FixedAddress(string street, string city)
    {
        Street = street;
        City = city;
    }

    public bool Equals(FixedAddress? other) =>
        other is not null && Street == other.Street && City == other.City;

    public override bool Equals(object? obj) => Equals(obj as FixedAddress);

    // Must match Equals: equal objects must have equal hash codes
    public override int GetHashCode() => HashCode.Combine(Street, City);
}