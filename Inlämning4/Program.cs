using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Inlämning4.Classes;
using Inlämning4.Repositories;

namespace Inlämning4
{
    class Program
    {
        private static void ProgramStart()
        {
            if (!Directory.Exists(Json.programPath))
            {
                Directory.CreateDirectory(Json.programPath);
            }
            
            UI.MainMenuInterface();
        }
        

        static void Main(string[] args)
        {
            ProgramStart();
        }
    }
}