using System;

namespace Laba5.Models
{
    public class Movement
    {
        private int _id;
        private DateTime _date;
        private string _shopId;
        private int _article;
        private int _quantity;
        private string _type;

        public Movement(int id, DateTime date, string shopId,
            int article, int quantity, string type)
        {
            _id = id;
            _date = date;
            _shopId = shopId;
            _article = article;
            _quantity = quantity;
            _type = type;
        }

        public int Id
        {
            get { return _id; }
        }

        public DateTime Date
        {
            get { return _date; }
        }

        public string ShopId
        {
            get { return _shopId; }
        }

        public int Article
        {
            get { return _article; }
        }

        public int Quantity
        {
            get { return _quantity; }
        }

        public string Type
        {
            get { return _type; }
        }

        public override string ToString()
        {
            return $"ID: {_id}, {_date:yyyy-MM-dd}, " +
                   $"Магазин: {_shopId}, Артикул: {_article}, " +
                   $"Кол-во: {_quantity}, Тип: {_type}";
        }
    }
}