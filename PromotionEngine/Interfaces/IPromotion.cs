namespace PromotionEngine.Interfaces
{
    public interface IPromotion
    {
        Order Apply(Order order);

        string Name { get; }

        int OrderOfPrecedence { get; }
    }
}