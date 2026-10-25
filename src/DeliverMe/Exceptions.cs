namespace DeliverMe;

// C5: parent of every DeliverMe-specific error
public class DeliverMeException : Exception
{
    public DeliverMeException(string message) : base(message) { }
}

// C2: a status move that breaks the rules
public class InvalidOrderStateException : DeliverMeException
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

// C5: not enough money in the wallet
public class InsufficientFundsException : DeliverMeException
{
    public decimal Balance { get; }
    public decimal Required { get; }

    public InsufficientFundsException(decimal balance, decimal required)
        : base($"Wallet balance {balance:F2} is lower than {required:F2}.")
    {
        Balance = balance;
        Required = required;
    }
}