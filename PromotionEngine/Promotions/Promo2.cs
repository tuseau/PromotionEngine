using PromotionEngine.PromotionTypes;
using System.Collections.Generic;

namespace PromotionEngine.Promotions
{
    public class Promo2 : HomogeneousCombo
    {
        public override string Name => "Promo2";
        public override int OrderOfPrecedence => 1;
        public override List<string> Combination => new List<string>() { "B", "B" };
        public override int Price => 45;
    }
}