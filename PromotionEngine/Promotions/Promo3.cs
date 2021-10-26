using PromotionEngine.PromotionTypes;
using System.Collections.Generic;

namespace PromotionEngine.Promotions
{
    public class Promo3 : HeterogeneousCombo
    {
        public override string Name => "Promo3";
        public override int OrderOfPrecedence => 2;
        public override List<string> Combination => new List<string>() { "C", "D" };
        public override int Price => 30;
    }
}