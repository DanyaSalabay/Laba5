using System;
using System.Linq;
using Laba5.Helpers;
using Laba5.Services;
using Laba5.Models;

namespace Laba5
{
    class Program
    {
        static void Main(string[] args)
        {
            string path =
                @"C:\Users\Даниил\RiderProjects\Laba5\Laba5\LR5-var10.xlsx"; 
            
            DataHelper helper = new DataHelper(path);
            helper.LoadData();
            
            Queries queryService = new Queries(helper);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Просмотр БД");
                Console.WriteLine("2. Удаление по ID");
                Console.WriteLine("3. Добавление элемента");
                Console.WriteLine("4. Запрос 1 " +
                                  "(1 табл, сумма проданных)");
                Console.WriteLine("5. Запрос 2 " +
                                  "(2 табл, топ-5 продаж)");
                Console.WriteLine("6. Запрос 3 (3 табл," +
                                  " движение в Центральном)");
                Console.WriteLine("7. Запрос 4 (3 табл," +
                                  " гели для душа на Гагарина)");
                Console.WriteLine("8. Сохранить в Excel");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");

                string choice = Console.ReadLine()!;
                switch (choice)
                {
                    case "1":
                        ShowMenu(helper); 
                        break;
                    case "2":
                        DeleteRecord(helper);
                        break;
                    case "3":
                        AddNewMovement(helper);
                        break;
                    case "4":
                        Console.Clear();
                        queryService.Query1(); 
                        break;
                    case "5":
                        Console.Clear();
                        queryService.Query2();
                        break;
                    case "6":
                        Console.Clear();
                        queryService.Query3();
                        break;
                    case "7":
                        Console.Clear();
                        queryService.Query4();
                        break;
                    case "8":
                        Console.Clear();
                        Console.WriteLine("СОХРАНЕНИЕ В EXCEL");
                        helper.SaveData();
                        Console.WriteLine("\nДанные" +
                                          " успешно сохранены!");
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
                Console.WriteLine("\nНажмите любую клавишу...");
                Console.ReadKey();
            }
        }

        static void ShowMenu(DataHelper helper)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("ПРОСМОТР БАЗЫ ДАННЫХ");
                Console.WriteLine("1. Движения (операции)");
                Console.WriteLine("2. Товары");
                Console.WriteLine("3. Магазины");
                Console.WriteLine("0. Назад в главное меню");
                Console.Write("Выбор: ");

                string choice = Console.ReadLine()!;
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("ТАБЛИЦА: ДВИЖЕНИЕ ТОВАРОВ");
                        foreach (var m in helper.Movements)
                        {
                            Console.WriteLine(m);
                        }
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("ТАБЛИЦА: ТОВАРЫ");
                        foreach (var p in helper.Products)
                        {
                            Console.WriteLine(p);
                        }
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("ТАБЛИЦА: МАГАЗИНЫ");
                        foreach (var s in helper.Shops)
                        {
                            Console.WriteLine(s);
                        }
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
                Console.WriteLine("\nНажмите любую клавишу...");
                Console.ReadKey();
            }
        }

        static void DeleteRecord(DataHelper helper)
        {
            try
            {
                Console.Write("Введите ID для удаления: ");
                int id = int.Parse(Console.ReadLine()!);

                bool isDeleted = helper.RemoveMovement(id);

                if (isDeleted)
                {
                    Console.WriteLine("Запись удалена " +
                                      "(не забудьте сохранить).");
                }
                else
                {
                    Console.WriteLine("Операции с таким ID" +
                                      " не существует!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка ввода: {ex.Message}");
            }
        }

        static void AddNewMovement(DataHelper helper)
        {
            try
            {
                Console.WriteLine("ДОБАВЛЕНИЕ НОВОЙ ОПЕРАЦИИ");

                int id = 0;
                while (true)
                {
                    Console.Write("ID операции: ");
                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        Console.WriteLine("Ошибка:" +
                                          " ID должен быть числом!");
                        continue;
                    }

                    if (helper.Movements.Any(m => m.Id == id))
                    {
                        Console.WriteLine("Ошибка: Операция с таким ID" +
                                          " уже существует! Введите другой.");
                        continue;
                    }
                    break;
                }

                DateTime dt;
                while (true)
                {
                    Console.Write("Дата (yyyy-mm-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out dt))
                    {
                        Console.WriteLine("Ошибка: Неверный формат даты!" +
                                          " Попробуйте снова (гггг-мм-дд).");
                        continue;
                    }
                    break;
                }

                string shop;
                while (true)
                {
                    Console.Write("ID магазина (например, M1): ");
                    shop = Console.ReadLine()!;
                    
                    if (!helper.Shops.Any(s => s.Id == shop))
                    {
                        Console.WriteLine("Ошибка: Магазина с таким ID" +
                                          " нет в базе! Попробуйте снова.");
                        continue;
                    }
                    break;
                }

                int art = 0;
                while (true)
                {
                    Console.Write("Артикул: ");
                    if (!int.TryParse(Console.ReadLine(), out art))
                    {
                        Console.WriteLine("Ошибка:" +
                                          " Артикул должен быть числом!");
                        continue;
                    }

                    if (!helper.Products.Any(p => p.Article == art))
                    {
                        Console.WriteLine("Ошибка: Товара с таким артикулом" +
                                          " нет в базе! Попробуйте снова.");
                        continue;
                    }
                    break;
                }

                Console.Write("Количество: ");
                int qty = int.Parse(Console.ReadLine()!);

                Console.Write("Тип (Поступление/Продажа): ");
                string type = Console.ReadLine()!;

                Movement newMov = new Movement(id, dt, shop, art,
                    qty, type);

                if (helper.AddMovement(newMov))
                {
                    Console.WriteLine("\nОперация добавлена!");
                    Console.WriteLine("База отсортирована по ID.");
                }
                else
                {
                    Console.WriteLine("\nНе удалось добавить.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}