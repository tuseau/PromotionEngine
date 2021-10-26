using System.Linq;

namespace PromotionEngine.PromotionTypes
{
    public abstract class HomogeneousCombo : Combo
    {
        protected override bool doesOrderContainCombination(Order order)
        {
            return string.Join(',', order.Items
                .OrderBy(x => x.Sku.ID)
                .Where(x => string.IsNullOrWhiteSpace(x.AppliedPromotion))
                .Select(x => x.Sku.ID)
                .ToList())
                .Contains(string.Join(',', Combination));
        }
    }
}