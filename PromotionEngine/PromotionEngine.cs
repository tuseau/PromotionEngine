using PromotionEngine.Interfaces;
using PromotionEngine.Promotions;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine
{
    public class PromotionEngine : IPromotionEngine
    {
        private List<IPromotion> _activePromotions;

        public PromotionEngine(List<IPromotion> activePromotions)
        {
            _activePromotions = activePromotions;
        }

        public PromotionEngine()
        {
            _activePromotions = new List<IPromotion>()
            {
                new Promo1(),
                new Promo2(),
                new Promo3()
            };
        }

        public Order ApplyPromotions(Order order)
        {
            _activePromotions.OrderBy(x => x.OrderOfPrecedence)
                .ToList()
                .ForEach(p => p.Apply(order));

            order.OrderTotal += nonPromotedItemsSum(order);
            return order;
        }

        private int nonPromotedItemsSum(Order order)
        {
            return order.Items
                .Where(x => string.IsNullOrWhiteSpace(x.AppliedPromotion))
                .Select(x => x.Sku.Price)
                .Sum();
        }
    }
}