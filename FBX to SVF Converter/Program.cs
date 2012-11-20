using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FBX_to_SVF_Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() < 1)
                Console.WriteLine("To use the tool. Drag and drop fbx file(s) on the executable");

            foreach (string a in args)
            {
                if (CheckIsFBX(a))
                    BeginConversion(a);
            }

            Console.ReadKey();
        }

        static bool CheckIsFBX(string path)
        {
            if (path.Substring(path.Length - 3).ToLower() == "fbx")
            {
                return true;
            }

            else
            {
                Console.WriteLine("File: \n" + path + "\nis not an .fbx file. Are you sure you wish to attempt converting?");
                Console.WriteLine("y/n?");
                if (Console.ReadKey(false).Key == ConsoleKey.Y)
                    return true;
                else
                    return false;
            }
        }

        static bool BeginConversion(string path)
        {
            string file;
            int FBXVersion;

            // Read the file to a string
            StreamReader sw = new StreamReader(path);
            file = sw.ReadToEnd();
            sw.Close();

            // Determine the FBXVersion
            int a = file.IndexOf("FBXVersion");
            int b = file.Substring(a).IndexOf(' ');
            if (!int.TryParse(file.Substring(a + b + 1, 4), out FBXVersion)) return false;

            Console.WriteLine("FBXVersion: " + FBXVersion + " detected. Attempting to convert");

            switch (FBXVersion)
            {
                case 7300:
                    Convert7300(file);
                    break;
                case 6100:
                    Convert6100(file);
                    break;
                default:
                    Console.WriteLine("Non-Supported FBXVersion. Please contact the distributor or upgrade your modeling software.");
                    return false;
            }

            return true;
        }

        static bool Convert7300(string file)
        {
            int vertexCount, indexCount;
            string[] vertices, indices;

            int a = file.IndexOf("Vertices:");
            int b = file.Substring(a).IndexOf(' ') + 2;
            int c = file.Substring(a + b).IndexOf('{');
            if (!int.TryParse(file.Substring(a + b, c - 1), out vertexCount)) return false;

            Console.WriteLine(vertexCount);

            return true;
        }

        static bool Convert6100(string file)
        {
            return true;
        }
    }
}
