using PromotionEngine.PromotionTypes;
using System.Collections.Generic;

namespace PromotionEngine.Promotions
{
    public class Promo1 : HomogeneousCombo
    {
        public override string Name => "Promo1";
        public override int OrderOfPrecedence => 0;
        public override List<string> Combination => new List<string>() { "A", "A", "A" };
        public override int Price => 130;
    }
}