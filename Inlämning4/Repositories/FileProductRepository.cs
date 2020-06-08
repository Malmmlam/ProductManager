using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Inlämning4.Classes;

namespace Inlämning4.Repositories
{
    public class FileProductRepository : IProductRepository
    {
        private List<Product> _productList = new List<Product>();
        private string _productJsonPath = Path.Combine(Json.programPath, "Products.json");

        public List<Product> ProductList
        {
            get => _productList;
            set => _productList = value;
        }

        public string ProductJsonPath => _productJsonPath;

        public FileProductRepository()
        {
            if (!File.Exists(_productJsonPath))
            {
                Save(_productJsonPath, _productList);
            }
            else
            {
                var jsonUtf8Bytes = File.ReadAllBytes(_productJsonPath);
                var readOnlySpan = new ReadOnlySpan<byte>(jsonUtf8Bytes);
                _productList = JsonSerializer.Deserialize<List<Product>>(readOnlySpan);
            }
        }

        public IEnumerable<Product> GetAll()
        {
            return _productList;
        }

        //Search for the product in the list by name.    
        public List<Product> SearchForProductByName(string searchString)
        {
            var searchResults = _productList.FindAll
                (p => p.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
            return searchResults;
        }

        //Search for the product in the list by price.
        public void SearchForProductByPrice(decimal productPrice)
        {
            int count = _productList.Count(p => p.Price <= productPrice);
            Console.WriteLine($"Found {count} results:");

            foreach (var product in _productList.Where(product => product.Price <= productPrice))
            {
                Console.WriteLine(product);
            }
        }
        
        //Sort the product list.
        public void SortList()
        {
            if (_productList.Count > 1)
            {
                ProductList.Sort((productA, productB) => productA.Price.CompareTo(productB.Price));
            }
        }

        //Remove product from list.
            public void Delete(Product product)
            {
                _productList.Remove(product);
                Save(_productJsonPath, _productList);
            }

        //Insert product into list. 
        public void Insert(string name, decimal price, Manufacturer manufacturer)
        {
            var product = new Product(name, price, manufacturer);
            ProductList.Add(product);
        }

        //Save list to json file.
        public void Save(string filePath, IEnumerable<Product> records)
        {
            SortList();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.SerializeToUtf8Bytes(records, options);
            File.WriteAllBytes(filePath, jsonString);
        }

        
    }
}