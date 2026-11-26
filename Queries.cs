using System;
using System.Linq;
using Laba5.Models;
using Laba5.Helpers;

namespace Laba5.Services
{
    public class Queries
    {
        private DataHelper _helper;

        public Queries(DataHelper helper)
        {
            _helper = helper;
        }

        public void Query1()
        {
            var totalSold = (from m in _helper.Movements
                             where m.Type == "Продажа"
                             select m.Quantity).Sum();

            Console.WriteLine("ЗАПРОС 1 (1 таблица)");
            Console.WriteLine("Общее количество проданных упаковок:");
            Console.WriteLine($" {totalSold} шт.");
        }

        public void Query2()
        {
            var query = (from m in _helper.Movements
                    join s in _helper.Shops on m.ShopId equals s.Id
                    where m.Type.Trim() == "Продажа"
                    group m by s.Address into g
                    select new { 
                        Address = g.Key, 
                        TotalSold = g.Sum(x => x.Quantity)
                    })
                .OrderByDescending(x => x.TotalSold)
                .Take(5)
                .ToList();

            Console.WriteLine("ЗАПРОС 2 (2 таблицы)");
            Console.WriteLine("ТОП-5 магазинов по объему продаж (шт.):");
            
            int rank = 1;
            foreach (var item in query)
            {
                Console.WriteLine($"{rank}. {item.Address} : " +
                                  $"{item.TotalSold} шт.");
                rank++;
            }
        }

        public void Query3()
        {
            var query = from m in _helper.Movements
                        join s in _helper.Shops on m.ShopId equals s.Id
                        join p in _helper.Products 
                        on m.Article equals p.Article
                        where s.District.Trim() == "Центральный"
                        select new { 
                            p.Name, 
                            s.Address, 
                            m.Quantity,
                            m.Type
                        };

            Console.WriteLine("ЗАПРОС 3 (3 таблицы)");
            Console.WriteLine("Движение товаров в магазинах");
            Console.WriteLine("Центрального района:");
            
            foreach (var item in query)
            {
                Console.WriteLine(
                    $"[{item.Type}] {item.Name} -> " +
                    $"{item.Address} : {item.Quantity} шт.");
            }
        }

        public void Query4()
        {
            var totalQty = (from m in _helper.Movements
                join s in _helper.Shops 
                    on m.ShopId equals s.Id
                join p in _helper.Products 
                    on m.Article equals p.Article
                where s.Address.Contains("Гагарина")
                      && m.Date >= new DateTime(2023, 7, 1)
                      && m.Date <= new DateTime(2023, 7, 10)
                      && m.Type.Trim() == "Продажа"
                      && p.Name.Contains("Гель для душа")
                select m.Quantity).Sum();

            Console.WriteLine("ЗАПРОС 4 (3 таблицы)");
            Console.WriteLine("Гелей для душа продано на ул. Гагарина");
            Console.WriteLine("с 1 по 10 июля (в упаковках):");
            Console.WriteLine($" {totalQty} шт.");
        }
    }
}