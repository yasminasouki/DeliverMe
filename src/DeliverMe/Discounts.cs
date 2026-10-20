namespace DeliverMe;

public interface IDiscountPolicy
{
    decimal Apply(decimal subtotal);
}

public class NoDiscount : IDiscountPolicy
{
    public decimal Apply(decimal subtotal) => subtotal;
}

public class StudentDiscount : IDiscountPolicy
{
    public decimal Apply(decimal subtotal) => Math.Round(subtotal * 0.90m, 2);
}