using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Inlämning4.Classes;

namespace Inlämning4.Repositories
{
    class FileManufacturerRepository : IManufacturerRepository
    {
        private List<Manufacturer> _manufacturersList = new List<Manufacturer>();
        private string _manufacturerJsonPath = Path.Combine(Json.programPath, "Manufacturer.json");


        public FileManufacturerRepository()
        {
            this._manufacturersList = new List<Manufacturer>();
            if (!File.Exists(_manufacturerJsonPath))
            {
                Save(_manufacturerJsonPath, _manufacturersList);
            }
            else
            {
                var jsonUtf8Bytes = File.ReadAllBytes(_manufacturerJsonPath);
                var readOnlySpan = new ReadOnlySpan<byte>(jsonUtf8Bytes);
                _manufacturersList = JsonSerializer.Deserialize<List<Manufacturer>>(readOnlySpan);
            }
        }

        public string ManufacturerJsonPath => _manufacturerJsonPath;

        public List<Manufacturer> ManufacturersList
        {
            get => _manufacturersList;
            set => _manufacturersList = value;
        }

        public IEnumerable<Manufacturer> GetAll()
        {
            return _manufacturersList;
        }

        public void SortList()
        {
            if (_manufacturersList.Count > 1)
            {
                _manufacturersList.Sort((manufacturerA, manufacturerB) =>
                    string.Compare(manufacturerA.Name, manufacturerB.Name, StringComparison.Ordinal));
            }
        }

        public void Delete(Manufacturer manufacturer)
        {
            var toBeDeleted = _manufacturersList.Find(m => m.Name.Contains(manufacturer.Name));
            _manufacturersList.Remove(toBeDeleted);
            Save(_manufacturerJsonPath, _manufacturersList);
        }

        public void Insert(string name, string location)
        {
            var manufacturer = new Manufacturer(name, location);
            if (_manufacturersList != null)
            {
                _manufacturersList.Add(manufacturer);
            }
            else
            {
                var fileManufacturerRepository = new FileManufacturerRepository();
                fileManufacturerRepository._manufacturersList.Add(manufacturer);
            }
        }

        public void Save(string filePath, IEnumerable<Manufacturer> records)
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