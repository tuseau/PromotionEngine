using System.Collections.Generic;

namespace PromotionEngine
{
    public class Order
    {
        public List<Item> Items { get; set; }
        public int OrderTotal { get; set; }
    }
}