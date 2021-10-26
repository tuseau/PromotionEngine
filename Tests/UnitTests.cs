using PromotionEngine;
using PromotionEngine.Interfaces;
using PromotionEngine.Promotions;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class UnitTests
    {
        private List<IPromotion> _activePromos;

        public UnitTests()
        {
            _activePromos = new List<IPromotion>()
            {
                new Promo1(),
                new Promo2(),
                new Promo3()
            };
        }

        private List<IPromotion> activePromotionsFactory(bool promo1, bool promo2, bool promo3)
        {
            var promos = new List<IPromotion>();

            if (promo1)
                promos.Add(new Promo1());

            if (promo2)
                promos.Add(new Promo2());

            if (promo3)
                promos.Add(new Promo3());

            return promos;
        }

        private Order orderFactory(int a, int b, int c, int d)
        {
            var order = new Order()
            {
                Items = new List<Item>()
            };

            for (int i = 0; i < a; i++)
                order.Items.Add(new Item { Sku = new Sku { ID = "A", Price = 50 } });

            for (int i = 0; i < b; i++)
                order.Items.Add(new Item { Sku = new Sku { ID = "B", Price = 30 } });

            for (int i = 0; i < c; i++)
                order.Items.Add(new Item { Sku = new Sku { ID = "C", Price = 20 } });

            for (int i = 0; i < d; i++)
                order.Items.Add(new Item { Sku = new Sku { ID = "D", Price = 15 } });

            return order;
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(50, 1, 0, 0, 0)]
        [InlineData(30, 0, 1, 0, 0)]
        [InlineData(20, 0, 0, 1, 0)]
        [InlineData(15, 0, 0, 0, 1)]
        [InlineData(115, 1, 1, 1, 1)]
        [InlineData(150, 3, 0, 0, 0)]
        [InlineData(60, 0, 2, 0, 0)]
        [InlineData(35, 0, 0, 1, 1)]
        [InlineData(650, 0, 10, 10, 10)]
        [InlineData(850, 10, 0, 10, 10)]
        [InlineData(950, 10, 10, 0, 10)]
        [InlineData(1000, 10, 10, 10, 0)]
        [InlineData(11500, 100, 100, 100, 100)]
        public void NoPromotions(int expectedOrderTotal, int a, int b, int c, int d)
        {
            // arrange
            var order = orderFactory(a, b, c, d);
            var activePromotions = activePromotionsFactory(false, false, false);
            var promoEngine = new PromotionEngine.PromotionEngine(activePromotions);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(expectedOrderTotal, result.OrderTotal);
            Assert.False(result.Items.Exists(x => !string.IsNullOrWhiteSpace(x.AppliedPromotion)));
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(130, 3, 0, 0, 0)]
        [InlineData(260, 6, 0, 0, 0)]
        [InlineData(390, 9, 0, 0, 0)]
        [InlineData(440, 10, 0, 0, 0)]
        [InlineData(490, 11, 0, 0, 0)]
        [InlineData(45, 0, 2, 0, 0)]
        [InlineData(90, 0, 4, 0, 0)]
        [InlineData(135, 0, 6, 0, 0)]
        [InlineData(165, 0, 7, 0, 0)]
        [InlineData(30, 0, 0, 1, 1)]
        [InlineData(60, 0, 0, 2, 2)]
        [InlineData(90, 0, 0, 3, 3)]
        [InlineData(110, 0, 0, 4, 3)]
        [InlineData(105, 0, 0, 3, 4)]
        [InlineData(205, 3, 2, 1, 1)]
        [InlineData(410, 6, 4, 2, 2)]
        public void PromoSanity(int expectedOrderTotal, int a, int b, int c, int d)
        {
            // arrange
            var order = orderFactory(a, b, c, d);
            var activePromotions = activePromotionsFactory(true, true, true);
            var promoEngine = new PromotionEngine.PromotionEngine(activePromotions);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(expectedOrderTotal, result.OrderTotal);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(130, 3, 0, 0, 0)]
        [InlineData(260, 6, 0, 0, 0)]
        [InlineData(390, 9, 0, 0, 0)]
        [InlineData(440, 10, 0, 0, 0)]
        [InlineData(490, 11, 0, 0, 0)]
        [InlineData(60, 0, 2, 0, 0)]
        [InlineData(120, 0, 4, 0, 0)]
        [InlineData(180, 0, 6, 0, 0)]
        [InlineData(210, 0, 7, 0, 0)]
        [InlineData(35, 0, 0, 1, 1)]
        [InlineData(70, 0, 0, 2, 2)]
        [InlineData(105, 0, 0, 3, 3)]
        [InlineData(125, 0, 0, 4, 3)]
        [InlineData(120, 0, 0, 3, 4)]
        [InlineData(225, 3, 2, 1, 1)]
        [InlineData(450, 6, 4, 2, 2)]
        public void Promo1Sanity(int expectedOrderTotal, int a, int b, int c, int d)
        {
            // arrange
            var order = orderFactory(a, b, c, d);
            var activePromotions = activePromotionsFactory(true, false, false);
            var promoEngine = new PromotionEngine.PromotionEngine(activePromotions);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(expectedOrderTotal, result.OrderTotal);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(150, 3, 0, 0, 0)]
        [InlineData(300, 6, 0, 0, 0)]
        [InlineData(450, 9, 0, 0, 0)]
        [InlineData(500, 10, 0, 0, 0)]
        [InlineData(550, 11, 0, 0, 0)]
        [InlineData(45, 0, 2, 0, 0)]
        [InlineData(90, 0, 4, 0, 0)]
        [InlineData(135, 0, 6, 0, 0)]
        [InlineData(165, 0, 7, 0, 0)]
        [InlineData(35, 0, 0, 1, 1)]
        [InlineData(70, 0, 0, 2, 2)]
        [InlineData(105, 0, 0, 3, 3)]
        [InlineData(125, 0, 0, 4, 3)]
        [InlineData(120, 0, 0, 3, 4)]
        [InlineData(230, 3, 2, 1, 1)]
        [InlineData(460, 6, 4, 2, 2)]
        public void Promo2Sanity(int expectedOrderTotal, int a, int b, int c, int d)
        {
            // arrange
            var order = orderFactory(a, b, c, d);
            var activePromotions = activePromotionsFactory(false, true, false);
            var promoEngine = new PromotionEngine.PromotionEngine(activePromotions);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(expectedOrderTotal, result.OrderTotal);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(150, 3, 0, 0, 0)]
        [InlineData(300, 6, 0, 0, 0)]
        [InlineData(450, 9, 0, 0, 0)]
        [InlineData(500, 10, 0, 0, 0)]
        [InlineData(550, 11, 0, 0, 0)]
        [InlineData(60, 0, 2, 0, 0)]
        [InlineData(120, 0, 4, 0, 0)]
        [InlineData(180, 0, 6, 0, 0)]
        [InlineData(210, 0, 7, 0, 0)]
        [InlineData(30, 0, 0, 1, 1)]
        [InlineData(60, 0, 0, 2, 2)]
        [InlineData(90, 0, 0, 3, 3)]
        [InlineData(110, 0, 0, 4, 3)]
        [InlineData(105, 0, 0, 3, 4)]
        [InlineData(240, 3, 2, 1, 1)]
        [InlineData(480, 6, 4, 2, 2)]
        public void Promo3Sanity(int expectedOrderTotal, int a, int b, int c, int d)
        {
            // arrange
            var order = orderFactory(a, b, c, d);
            var activePromotions = activePromotionsFactory(false, false, true);
            var promoEngine = new PromotionEngine.PromotionEngine(activePromotions);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(expectedOrderTotal, result.OrderTotal);
        }

        [Fact]
        public void Promo1()
        {
            // arrange
            var order = new Order()
            {
                Items = new List<Item>
                {
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } }
                }
            };

            var promoEngine = new PromotionEngine.PromotionEngine(_activePromos);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(130, result.OrderTotal);
            Assert.Equal(new Promo1().Name, result.Items[0].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[1].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[2].AppliedPromotion);
        }

        [Fact]
        public void Promo2()
        {
            // arrange
            var order = new Order()
            {
                Items = new List<Item>
                {
                    new Item{ Sku = new Sku { ID = "B", Price = 30 } },
                    new Item{ Sku = new Sku { ID = "B", Price = 30 } }
                }
            };

            var promoEngine = new PromotionEngine.PromotionEngine(_activePromos);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(45, result.OrderTotal);
            Assert.Equal(new Promo2().Name, result.Items[0].AppliedPromotion);
            Assert.Equal(new Promo2().Name, result.Items[1].AppliedPromotion);
        }

        [Fact]
        public void Promo3()
        {
            // arrange
            var order = new Order()
            {
                Items = new List<Item>
                {
                    new Item{ Sku = new Sku { ID = "C", Price = 20 } },
                    new Item{ Sku = new Sku { ID = "D", Price = 15 } }
                }
            };

            var promoEngine = new PromotionEngine.PromotionEngine(_activePromos);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(30, result.OrderTotal);
            Assert.Equal(new Promo3().Name, result.Items[0].AppliedPromotion);
            Assert.Equal(new Promo3().Name, result.Items[1].AppliedPromotion);
        }

        [Fact]
        public void Promo3x2()
        {
            // arrange
            var order = new Order()
            {
                Items = new List<Item>
                {
                    new Item{ Sku = new Sku { ID = "C", Price = 20 } },
                    new Item{ Sku = new Sku { ID = "C", Price = 20 } },
                    new Item{ Sku = new Sku { ID = "D", Price = 15 } },
                    new Item{ Sku = new Sku { ID = "D", Price = 15 } }
                }
            };

            var promoEngine = new PromotionEngine.PromotionEngine(_activePromos);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(60, result.OrderTotal);
            Assert.Equal(new Promo3().Name, result.Items[0].AppliedPromotion);
            Assert.Equal(new Promo3().Name, result.Items[1].AppliedPromotion);
            Assert.Equal(new Promo3().Name, result.Items[2].AppliedPromotion);
            Assert.Equal(new Promo3().Name, result.Items[3].AppliedPromotion);
        }

        [Fact]
        public void Promo3x3()
        {
            // arrange
            var order = new Order()
            {
                Items = new List<Item>
                {
                    new Item{ Sku = new Sku { ID = "C", Price = 20 } },
                    new Item{ Sku = new Sku { ID = "C", Price = 20 } },
                    new Item{ Sku = new Sku { ID = "D", Price = 15 } },
                    new Item{ Sku = new Sku { ID = "D", Price = 15 } },
                    new Item{ Sku = new Sku { ID = "D", Price = 15 } },
                    new Item{ Sku = new Sku { ID = "C", Price = 20 } }
                }
            };

            var promoEngine = new PromotionEngine.PromotionEngine(_activePromos);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(90, result.OrderTotal);
            Assert.Equal(new Promo3().Name, result.Items[0].AppliedPromotion);
            Assert.Equal(new Promo3().Name, result.Items[1].AppliedPromotion);
            Assert.Equal(new Promo3().Name, result.Items[2].AppliedPromotion);
            Assert.Equal(new Promo3().Name, result.Items[3].AppliedPromotion);
            Assert.Equal(new Promo3().Name, result.Items[4].AppliedPromotion);
            Assert.Equal(new Promo3().Name, result.Items[5].AppliedPromotion);
        }

        [Fact]
        public void Promo1x2()
        {
            // arrange
            var order = new Order()
            {
                Items = new List<Item>
                {
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } }
                }
            };

            var promoEngine = new PromotionEngine.PromotionEngine(_activePromos);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(260, result.OrderTotal);
            Assert.Equal(new Promo1().Name, result.Items[0].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[1].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[2].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[3].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[4].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[5].AppliedPromotion);
        }

        [Fact]
        public void Promo1Plus1()
        {
            // arrange
            var order = new Order()
            {
                Items = new List<Item>
                {
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } }
                }
            };

            var promoEngine = new PromotionEngine.PromotionEngine(_activePromos);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(180, result.OrderTotal);
            Assert.Equal(new Promo1().Name, result.Items[0].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[1].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[2].AppliedPromotion);
            Assert.Null(result.Items[3].AppliedPromotion);
        }

        [Fact]
        public void Promo1Plus2()
        {
            // arrange
            var order = new Order()
            {
                Items = new List<Item>
                {
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } }
                }
            };

            var promoEngine = new PromotionEngine.PromotionEngine(_activePromos);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(230, result.OrderTotal);
            Assert.Equal(new Promo1().Name, result.Items[0].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[1].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[2].AppliedPromotion);
            Assert.Null(result.Items[3].AppliedPromotion);
            Assert.Null(result.Items[4].AppliedPromotion);
        }

        [Fact]
        public void Promo2Plus2()
        {
            // arrange
            var order = new Order()
            {
                Items = new List<Item>
                {
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } }
                }
            };

            var promoEngine = new PromotionEngine.PromotionEngine(_activePromos);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(230, result.OrderTotal);
            Assert.Equal(new Promo1().Name, result.Items[0].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[1].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[2].AppliedPromotion);
            Assert.Null(result.Items[3].AppliedPromotion);
            Assert.Null(result.Items[4].AppliedPromotion);
        }

        [Fact]
        public void Promo1Promo2Promo3()
        {
            // arrange
            var order = new Order()
            {
                Items = new List<Item>
                {
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "C", Price = 20 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "B", Price = 30 } },
                    new Item{ Sku = new Sku { ID = "D", Price = 15 } },
                    new Item{ Sku = new Sku { ID = "B", Price = 30 } },
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } }
                }
            };

            var promoEngine = new PromotionEngine.PromotionEngine(_activePromos);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(205, result.OrderTotal);
            Assert.Equal(new Promo1().Name, result.Items[0].AppliedPromotion);
            Assert.Equal(new Promo3().Name, result.Items[1].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[2].AppliedPromotion);
            Assert.Equal(new Promo2().Name, result.Items[3].AppliedPromotion);
            Assert.Equal(new Promo3().Name, result.Items[4].AppliedPromotion);
            Assert.Equal(new Promo2().Name, result.Items[5].AppliedPromotion);
            Assert.Equal(new Promo1().Name, result.Items[6].AppliedPromotion);
        }

        [Theory]
        [InlineData(100, 1, 1, 1, 0)]
        [InlineData(370, 5, 5, 1, 0)]
        [InlineData(280, 3, 5, 1, 1)]
        public void PromotionScenarios(int expectedOrderTotal, int a, int b, int c, int d)
        {
            // arrange
            var order = orderFactory(a, b, c, d);
            var activePromotions = activePromotionsFactory(true, true, true);
            var promoEngine = new PromotionEngine.PromotionEngine(activePromotions);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(expectedOrderTotal, result.OrderTotal);
        }

        [Fact]
        public void Promo4()
        {
            // arrange
            var order = new Order()
            {
                Items = new List<Item>
                {
                    new Item{ Sku = new Sku { ID = "A", Price = 50 } },
                    new Item{ Sku = new Sku { ID = "B", Price = 30 } },
                    new Item{ Sku = new Sku { ID = "C", Price = 20 } },
                    new Item{ Sku = new Sku { ID = "D", Price = 15 } }
                }
            };

            var activePromos = new List<IPromotion>()
            {
                new Promo4()
            };

            var promoEngine = new PromotionEngine.PromotionEngine(activePromos);

            // act
            var result = promoEngine.ApplyPromotions(order);

            // assert
            Assert.Equal(55, result.OrderTotal);
            Assert.Equal(new Promo4().Name, result.Items[0].AppliedPromotion);
            Assert.Null(result.Items[1].AppliedPromotion);
            Assert.Equal(new Promo4().Name, result.Items[2].AppliedPromotion);
            Assert.Null(result.Items[3].AppliedPromotion);
        }
    }
}