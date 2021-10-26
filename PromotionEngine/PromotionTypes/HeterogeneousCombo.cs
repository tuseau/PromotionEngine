using System.Linq;

namespace PromotionEngine.PromotionTypes
{
    public abstract class HeterogeneousCombo : Combo
    {
        protected override bool doesOrderContainCombination(Order order)
        {
            var orderSkus = order.Items
                .OrderBy(x => x.Sku.ID)
                .Where(x => string.IsNullOrWhiteSpace(x.AppliedPromotion))
                .Select(x => x.Sku.ID)
                .ToList();

            return !Combination.Except(orderSkus).Any();
        }
    }
}