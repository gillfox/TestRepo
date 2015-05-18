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
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string OpenFileOrNot;
            Searcher pathToScan = new Searcher();
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
                string pathToDir = Console.ReadLine();

                Console.WriteLine("Do you want to save results to a file ? (y/n):");
                OpenFileOrNot = Console.ReadLine();
                pathToScan.saveToFile = OpenFileOrNot == "y";
                pathToScan.ScanPath(pathToDir, "result.txt");
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            ShowResults(pathToScan, elapsedTime);
            Console.WriteLine("The program was successfully done!");
            Console.ReadKey();
        }


        static void ShowResults(Searcher pathToScan, string elapsedTime)
        {
            ICollection<string> Keys = pathToScan.filterFiles.Keys;
            if (pathToScan.saveToFile)
            {
                StreamWriter textFile = new StreamWriter(pathToScan.pathToFile);

                foreach (string key in Keys)
                {
                    textFile.WriteLine("\n\n\n\nGroup name : " + key);
                    foreach (string item in pathToScan.filterFiles[key])
                    {
                        textFile.WriteLine(item);
                    }
                }
                textFile.WriteLine("\n\n\nDirectories : {0} Files : {1} Run Time : {2} ", pathToScan.countDir, pathToScan.countFiles, elapsedTime);
                textFile.Close();
            }
            else
            {
                foreach (string key in Keys)
                {
                    Console.WriteLine("\n\n\n\nGroup name : " + key);
                    foreach (string item in pathToScan.filterFiles[key])
                    {
                        Console.WriteLine(item);
                    }
                }
                Console.WriteLine("\n\n\nDirectories : {0} Files : {1} Run Time : {2} ", pathToScan.countDir, pathToScan.countFiles, elapsedTime);
            }
        }
    }

}