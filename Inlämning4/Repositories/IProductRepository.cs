using System.Collections.Generic;
using Inlämning4.Classes;

namespace Inlämning4.Repositories
{
    public interface IProductRepository
    {
        string ProductJsonPath { get; }
        List<Product> ProductList { get; set; }
        IEnumerable<Product> GetAll();
        List<Product> SearchForProductByName(string searchString);
        void SearchForProductByPrice(decimal productPrice);
        void SortList();
        void Delete(Product product);
        void Insert(string name, decimal price, Manufacturer manufacturer);
        void Save(string filePath, IEnumerable<Product> records);    
    }
}