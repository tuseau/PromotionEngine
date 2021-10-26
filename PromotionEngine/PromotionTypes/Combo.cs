using PromotionEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.PromotionTypes
{
    public abstract class Combo : IPromotion
    {
        public abstract string Name { get; }

        public abstract int OrderOfPrecedence { get; }

        public abstract List<string> Combination { get; }

        public abstract int Price { get; }

        public Order Apply(Order order)
        {
            while (doesOrderContainCombination(order))
            {
                var matchCombo = new List<Item>();
                Combination.ForEach(sku => matchCombo.Add(order.Items
                    .Where(x => x.Sku.ID == sku && string.IsNullOrWhiteSpace(x.AppliedPromotion) && !matchCombo.Contains(x))
                        .First()));

                if (matchCombo.Select(x => x.Sku.ID).SequenceEqual(Combination))
                {
                    order.Items.Where(i => matchCombo.Contains(i)).ToList().ForEach(i => i.AppliedPromotion = Name);
                    order.OrderTotal += Price;
                }
            }

            return order;
        }

        protected abstract bool doesOrderContainCombination(Order order);
    }
}