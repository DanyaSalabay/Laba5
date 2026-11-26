using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelDataReader;
using ClosedXML.Excel;
using Laba5.Models;

namespace Laba5.Helpers
{
    public class DataHelper
    {
        private string _filePath;
        private List<Shop> _shops;
        private List<Product> _products;
        private List<Movement> _movements;

        public DataHelper(string filePath)
        {
            _filePath = filePath;
            _shops = new List<Shop>();
            _products = new List<Product>();
            _movements = new List<Movement>();
        }

        public List<Shop> Shops
        {
            get { return _shops; }
        }

        public List<Product> Products
        {
            get { return _products; }
        }

        public List<Movement> Movements
        {
            get { return _movements; }
        }

        public void LoadData()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("Файл не найден!");
                return;
            }

            System.Text.Encoding.RegisterProvider(
                System.Text.CodePagesEncodingProvider.Instance);

            try
            {
                using (var stream = File.Open(_filePath, FileMode.Open,
                    FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory
                        .CreateReader(stream))
                    {
                        var result = reader.AsDataSet();
                        
                        var tShops = result.Tables["Магазин"];
                        var tProducts = result.Tables["Товар"];
                        var tMovements = 
                            result.Tables["Движение товаров"];

                        if (tShops == null || tProducts == null 
                            || tMovements == null)
                        {
                            Console.WriteLine("Ошибка: Неверные листы!");
                            return;
                        }

                        LoadShops(tShops);
                        LoadProducts(tProducts);
                        LoadMovements(tMovements);

                        _movements = _movements
                            .OrderBy(m => m.Id).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения: {ex.Message}");
            }
        }

        private void LoadShops(System.Data.DataTable table)
        {
            int i = 1;
            for (i = 1; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                if (row[0] == DBNull.Value)
                {
                    continue;
                }

                string id = row[0].ToString();
                string district = row[1].ToString();
                string address = row[2].ToString();
                _shops.Add(new Shop(id, district, address));
            }
        }

        private void LoadProducts(System.Data.DataTable table)
        {
            int i = 1;
            for (i = 1; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                if (row[0] == DBNull.Value)
                {
                    continue;
                }

                int article = Convert.ToInt32(row[0]);
                string dept = row[1].ToString();
                string name = row[2].ToString();
                string unit = row[3].ToString();
                int packQty = Convert.ToInt32(row[4]);
                decimal price = Convert.ToDecimal(row[5]);

                _products.Add(new Product(article, dept, name,
                    unit, packQty, price));
            }
        }

        private void LoadMovements(System.Data.DataTable table)
        {
            int i = 1;
            for (i = 1; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                if (row[0] == DBNull.Value)
                {
                    continue;
                }

                int id = Convert.ToInt32(row[0]);
                DateTime date = Convert.ToDateTime(row[1]);
                string shopId = row[2].ToString();
                int article = Convert.ToInt32(row[3]);
                int qty = Convert.ToInt32(row[4]);
                string type = row[5].ToString().Trim();

                _movements.Add(new Movement(id, date, shopId,
                    article, qty, type));
            }
        }

        public bool AddMovement(Movement movement)
        {
            if (_movements.Any(m => m.Id == movement.Id))
            {
                return false;
            }
            
            _movements.Add(movement);
 
            _movements = _movements.OrderBy(m => m.Id).ToList();
            return true;
        }

        public bool RemoveMovement(int id)
        {
            var item = _movements.FirstOrDefault(m => m.Id == id);
            if (item != null)
            {
                _movements.Remove(item);
                return true; 
            }
            return false; 
        }

        public void SaveData()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("Файл не найден!");
                return;
            }

            try
            {
                using (var workbook = new XLWorkbook(_filePath))
                {
                    var ws =
                        workbook.Worksheet("Движение товаров");
                    ws.Rows(2, ws.LastRowUsed().RowNumber()).Delete();

                    int i;
                    for (i = 0; i < _movements.Count; i++)
                    {
                        var m = _movements[i];
                        ws.Cell(i + 2, 1).Value = m.Id;
                        ws.Cell(i + 2, 2).Value = m.Date;
                        ws.Cell(i + 2, 3).Value = m.ShopId;
                        ws.Cell(i + 2, 4).Value = m.Article;
                        ws.Cell(i + 2, 5).Value = m.Quantity;
                        ws.Cell(i + 2, 6).Value = m.Type;
                    }

                    workbook.Save();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка записи: {ex.Message}");
            }
        }
    }
}