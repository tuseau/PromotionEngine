using Microsoft.Extensions.DependencyInjection;
using PromotionEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace PromotionEngine.Client
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;

        public static void Main(string[] args)
        {
            try
            {
                RegisterServices();
                var promotionEngine = _serviceProvider.GetService<IPromotionEngine>();

                Console.WriteLine("Enter full path of file: ");
                var path = Console.ReadLine();
                var order = getOrderFromInputFile(path);
                var result = promotionEngine.ApplyPromotions(order);
                result.Items.ForEach(x => Console.WriteLine($"{x.Sku.ID}, {x.Sku.Price}, {x.AppliedPromotion.NullIfEmpty() ?? "<No Promo>"}"));
                Console.WriteLine(result.OrderTotal);
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.Write($"An error occured: {ex}");
                Console.Read();
            }
            finally
            {
                DisposeServices();
            }
        }

        private static Order getOrderFromInputFile(string path)
        {
            var order = new Order
            {
                Items = new List<Item>()
            };

            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    order.Items.Add(new Item 
                    { 
                        Sku = new Sku 
                        { 
                            ID = values[0], Price = int.Parse(values[1]) 
                        } 
                    });
                }
            }

            return order;
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();

            collection.AddTransient<IPromotionEngine, PromotionEngine>();

            _serviceProvider = collection.BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}