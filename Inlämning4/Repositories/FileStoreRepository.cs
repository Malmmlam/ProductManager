using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Inlämning4.Classes;

namespace Inlämning4.Repositories
{
    class FileStoreRepository : IStoreRepository
    {
        private List<Store> _storeList = new List<Store>();
        private readonly string _storeJsonPath = Path.Combine(Json.programPath, "Stores.json");

        public string StoreJsonPath => _storeJsonPath;

        public List<Store> StoreList
        {
            get => _storeList;
            set => _storeList = value;
        }

        public FileStoreRepository()
        {
            _storeList = new List<Store>();
            if (!File.Exists(_storeJsonPath))
            {
                Save(_storeJsonPath, _storeList);
            }
            else
            {
                var jsonUtf8Bytes = File.ReadAllBytes(_storeJsonPath);
                var readOnlySpan = new ReadOnlySpan<byte>(jsonUtf8Bytes);
                _storeList = JsonSerializer.Deserialize<List<Store>>(readOnlySpan);
            }
        }

        public List<Store> SearchForStoreByName(string searchString)
        {
            var searchResults = _storeList.FindAll
                (store => store.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
            return searchResults;
        }
        
        public void RemoveProductFromStore(Product product)
        {
            foreach (var store in _storeList)
            {
                store.StockedProducts.RemoveAll(p => p.Name == product.Name);
            }
            
            Save(_storeJsonPath, _storeList);
        }

        public void AddProductToStore(Store store, Product product)
        {
            var matchList = _storeList.Where(s => s.Equals(store)).ToList();
            foreach (var store1 in matchList)
            {
                store1.StockedProducts.Add(product);
            }
            Save(_storeJsonPath, _storeList);
        }
        
        public void SortList()
        {
            if (_storeList.Count > 1)
            {
                _storeList.Sort((storeA, storeB) => string.Compare(storeA.Name, storeB.Name, StringComparison.Ordinal));
            }
        }

        public void Delete(Store store)
        {
            _storeList.Remove(store);
            Save(_storeJsonPath, _storeList);
        }

        public void Insert(string name, string location)
        {
            var store = new Store(name, location, new List<Product>());
            if (_storeList != null)
            {
                _storeList.Add(store);
            }
            else
            {
                var fileStoreRepository = new FileStoreRepository();
                fileStoreRepository._storeList.Add(store);
            }
        }

        public void Save(string filePath, IEnumerable<Store> records)
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