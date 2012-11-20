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
            string file, fileName;
            int FBXVersion;

            // Get the file name
            int p = path.LastIndexOf("\\");
            if (p != -1)
                fileName = path.Substring(p + 1);
            else
                fileName = path;

            fileName = fileName.Substring(0, fileName.Length - 4);

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
                    Convert7300(file, fileName);
                    break;
                case 6100:
                    Convert6100(file, fileName);
                    break;
                default:
                    Console.WriteLine("Non-Supported FBXVersion. Please contact the distributor or upgrade your modeling software.");
                    return false;
            }

            return true;
        }

        static bool Convert7300(string file, string fileName)
        {
            int vertexCount, indexCount;
            string[] vertices, indices;

            // Find vertex count
            int a = file.IndexOf("Vertices:");
            int b = file.Substring(a).IndexOf(' ');
            int c = file.Substring(a + b + 2).IndexOf('{');
            if (!int.TryParse(file.Substring(a + b + 2, c - 1), out vertexCount)) return false;
            vertexCount /= 3;

            Console.WriteLine("Model contains " + vertexCount + " vertices");

            // Find vertices
            int d = file.Substring(a + b + c).IndexOf("a: ") + a + b + c;
            int e = file.Substring(d).IndexOf("\n");

            vertices = file.Substring(d + 3, e - 3).Split(',');

            for (int i = 0; i < vertices.Count(); i++)
            {
                if (i % 3 == 0)
                    Console.WriteLine("\n"+ "--" + (i / 3 + 1) + ":");

                Console.WriteLine(vertices[i]);
            }

            // Print a space between vertices and indices
            Console.WriteLine();

            // Find index count
            int f = file.IndexOf("PolygonVertexIndex:");
            int g = file.Substring(f).IndexOf(' ');
            int h = file.Substring(f + g + 2).IndexOf('{');
            if (!int.TryParse(file.Substring(a + b + 2, c - 1), out indexCount)) return false;

            Console.WriteLine("Model contains " + indexCount + " indices");

            // Find indices
            int j = file.Substring(f + g + h).IndexOf("a: ") + f + g + h;
            int k = file.Substring(j).IndexOf("\n");

            indices = file.Substring(j + 3, k - 3).Split(',');

            foreach (string s in indices)
            {
                Console.Write(s + ", ");
            }

            SaveToFile(vertexCount, indexCount, vertices, indices, fileName);

            return true;
        }

        static bool Convert6100(string file, string fileName)
        {
            return true;
        }

        static bool SaveToFile(int vertexCount, int indexCount, string[] vertices, string[] indices, string fileName)
        {
            if (!Directory.Exists("SVF"))
                Directory.CreateDirectory("SVF");

            StreamWriter sw = new StreamWriter("SVF\\" + fileName + ".svf");

            sw.WriteLine("Vertices: " + vertexCount);
            sw.WriteLine("Indices: " + indexCount);

            sw.Close();

            return true;
        }
    }
}
