using System.Collections.Generic;

namespace Inlämning4.Classes
{
    public class Store
    {
        private string name;
        private string location;
        private List<Product> stockedProducts = new List<Product>();

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Location
        {
            get => location; 
            set => location = value;
        }
        
        public List<Product> StockedProducts
        {
            get => stockedProducts;
            set => stockedProducts = value;
        }
        
        public Store ()
        {}
        
        public Store(string name, string location, List<Product> stockedProducts)
        {
            this.name = name;
            this.location = location;
            this.stockedProducts = stockedProducts;
        }
        
        public override string ToString()
        {
            return $"Name: {this.name} | Location: {this.location} | Stocked Products: {this.stockedProducts.Count}";
        }    
    }
}