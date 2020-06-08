using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Inlämning4.Classes;

namespace Inlämning4.Classes
{
    public class Json
    {
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static string programPath = Path.Combine(path, "ProductManager");

        public static void RunProgram()
        {
            if (!File.Exists(programPath))
            {
                File.Create(programPath);
            }
        }

        // public static void SaveToJson<T>(string filePath, IEnumerable<T> records)
        // {
        //     
        //     var options = new JsonSerializerOptions
        //     {
        //         WriteIndented = true
        //     };
        //     var jsonString = JsonSerializer.SerializeToUtf8Bytes(records, options);
        //     File.WriteAllBytes(filePath, jsonString);
        // }
        //
        // public static IEnumerable<T> LoadFromJson<T>(string filePath)
        // {
        //     var jsonString = File.ReadAllText(filePath);
        //
        //     var records = JsonSerializer.Deserialize<List<T>>(jsonString);
        //     return records;
        // }
    }
}