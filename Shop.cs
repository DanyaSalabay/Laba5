using System;

namespace Laba5.Models
{
    public class Shop
    {
        private string _id;
        private string _district;
        private string _address;

        public Shop(string id, string district, string address)
        {
            _id = id;
            _district = district;
            _address = address;
        }

        public string Id
        {
            get { return _id; }
        }

        public string District
        {
            get { return _district; }
        }

        public string Address
        {
            get { return _address; }
        }

        public override string ToString()
        {
            return $"Магазин: {_id}, Район: {_district}, " +
                   $"Адрес: {_address}";
        }
    }
}