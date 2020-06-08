using System.Collections.Generic;
using Inlämning4.Classes;

namespace Inlämning4.Repositories
{
    public interface IManufacturerRepository
    {
        string ManufacturerJsonPath { get; }
        List<Manufacturer> ManufacturersList { get; set; }
        IEnumerable<Manufacturer> GetAll();
        void SortList();
        void Delete(Manufacturer manufacturer);
        void Insert(string name, string location);
        void Save(string filePath, IEnumerable<Manufacturer> records);  
    }
}