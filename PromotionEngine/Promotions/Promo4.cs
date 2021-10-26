using PromotionEngine.PromotionTypes;
using System.Collections.Generic;

namespace PromotionEngine.Promotions
{
    public class Promo4 : HeterogeneousCombo
    {
        public override string Name => "Promo4";
        public override int OrderOfPrecedence => 3;
        public override List<string> Combination => new List<string>() { "A", "C" };
        public override int Price => 10;
    }
}