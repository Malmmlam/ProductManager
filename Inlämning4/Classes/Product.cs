using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Inlämning4.Repositories;

namespace Inlämning4.Classes
{
    public class Product
    {
        private string _name;
        private decimal _price;
        private Manufacturer _manufacturer;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public decimal Price
        {
            get => _price;
            set => _price = value;
        }

        public Manufacturer Manufacturer
        {
            get => _manufacturer;
            set => _manufacturer = value;
        }

        private Product()
        {
        }

        public Product(string name, decimal price, Manufacturer manufacturer)
        {
            _manufacturer = manufacturer;
            _name = name;
            _price = price;
        }

        public override string ToString()
        {
            return $"Name: {_name} | Cost: {_price} | Manufacturer - {_manufacturer}";
        }    
    }
}