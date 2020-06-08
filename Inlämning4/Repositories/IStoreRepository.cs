using System.Collections.Generic;
using Inlämning4.Classes;

namespace Inlämning4.Repositories
{
    public interface IStoreRepository
    {
        List<Store> StoreList { get; set; }
        string StoreJsonPath { get; }
        public List<Store> SearchForStoreByName(string searchString);
        void RemoveProductFromStore(Product product);
        void AddProductToStore(Store store, Product product);
        void SortList();
        void Delete(Store store);
        void Insert(string name, string location);
        void Save(string filePath, IEnumerable<Store> records); 
    }
}