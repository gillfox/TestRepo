using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading;

namespace ScanProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string ChooseOpenFileOrNot;
            Scan pathToScan = new Scan();
            if (args.Length == 2)
            {
                string pathToDir = args[0];
                string nameOfFil = args[1];

                pathToScan.ScanPath(pathToDir, nameOfFil);
                
            }
            else if (args.Length == 1 && args[0].ToLower() == "help")
            {
                Console.WriteLine(@"

Usage : ScanProject.exe arg0 arg1 

arg0 -  Path to your directory. (Example - C:\\New foulder) 
arg1 -  Name of file result. (Example - result.txt)");
            }
            else
            {
                Console.Write("Please write path (Example - C:\\New foulder) : ");
                string p = Console.ReadLine();
                Console.WriteLine("Do you want to save results to a file ? (y/n):");
                ChooseOpenFileOrNot = Console.ReadLine();
                pathToScan.ScanPath(p, "result.txt", ChooseOpenFileOrNot);
                Console.WriteLine("The program is successfully done!");
                Console.ReadKey();
            }            
        }
    }

}
