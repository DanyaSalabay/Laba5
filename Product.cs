using System;

namespace Laba5.Models
{
    public class Product
    {
        private int _article;
        private string _department;
        private string _name;
        private string _unit;
        private int _packQty;
        private decimal _price;

        public Product(int article, string department, string name,
            string unit, int packQty, decimal price)
        {
            _article = article;
            _department = department;
            _name = name;
            _unit = unit;
            _packQty = packQty;
            _price = price;
        }

        public int Article
        {
            get { return _article; }
        }

        public string Department
        {
            get { return _department; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Unit
        {
            get { return _unit; }
        }

        public int PackQty
        {
            get { return _packQty; }
        }

        public decimal Price
        {
            get { return _price; }
        }

        public override string ToString()
        {
            return $"Артикул: {_article}, {_name} (Отдел: " +
                   $"{_department}, Цена: {_price})";
        }
    }
}