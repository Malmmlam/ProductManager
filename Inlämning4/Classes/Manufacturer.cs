using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inlämning4.Classes
{
    public class Manufacturer
    {
        private string _name;
        private string _location;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Location
        {
            get => _location;
            set => _location = value;
        }

        public Manufacturer()
        {
        }

        public Manufacturer(string name, string location)
        {
            _name = name;
            _location = location;
        }

        public override string ToString()
        {
            return
                $"Name: {_name} | Location: {Location}";
        }
    }
}