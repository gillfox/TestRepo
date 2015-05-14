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
    class Scan
    {
        int countFiles, countDir;
        string pathToFile;
        public Stopwatch stopWatch = new Stopwatch();
        
        public Scan()
        {
            countFiles = 0;
            countDir = 0;
            pathToFile = "";
            stopWatch.Start();
            
        }

        public void ScanPath(string pathToDir, string nameOfFile = "result.txt", bool saveToFile = true)
        {
            FileAttributes newGroupOfFiles = new FileAttributes();
           
            this.pathToFile = pathToDir + "\\" + nameOfFile;
            string startFolder = pathToDir;
            int charsToSkip = startFolder.Length;

            DirectoryInfo dir = new DirectoryInfo(startFolder);
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
            countDir = dir.GetDirectories().Count();
            countFiles = fileList.Count();
            Dictionary<string, string> filesAndHashes = new Dictionary<string, string>();
            
            Dictionary<string, List<string>> filterFiles = new Dictionary<string, List<string>>();

            foreach (var file in fileList)
            {
                filesAndHashes.Add(file.FullName, ComputeMD5Checksum(file.FullName));
            }
            
            Console.WriteLine("Starting");
            foreach(var file in fileList)
            {
                List<string> similFiles = new List<string>();
                string HashCodeOfFile = ComputeMD5Checksum(file.FullName);
                if (filterFiles.ContainsKey(HashCodeOfFile))
                {
                    continue;
                }
                foreach(var siFile in  filesAndHashes.Keys)
                {
                    if (HashCodeOfFile == filesAndHashes[siFile])
                    {
                        similFiles.Add(siFile);
                    }
                }
                if (similFiles.Count > 1 && HashCodeOfFile != "") 
                {
                    filterFiles.Add(HashCodeOfFile, similFiles);
                }
                Console.WriteLine("d");
                
            }

            Console.WriteLine("DONE");
            ShowResults(filterFiles);
            
        }

        private void ShowResults(Dictionary<string, List<string>> groupByExtList, bool saveToFile = true)
        {
            Console.WriteLine("Almost done");
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

            ICollection<string> Keys = groupByExtList.Keys;

            foreach (string key in Keys)
            {
                Console.WriteLine("\n\n\n\nGroup name : " + key);
                foreach (string item in groupByExtList[key])
                {
                    Console.WriteLine(item);
                }
            }
            if (saveToFile)
            {   
                StreamWriter textFile = new StreamWriter(pathToFile);
                
                foreach (string key in Keys)
                {
                    textFile.WriteLine("\n\n\n\nGroup name : " + key);
                    foreach (string item in groupByExtList[key])
                    {
                        textFile.WriteLine(item);
                    }
                }
                textFile.WriteLine("\n\n\nDirectories : {0} Files : {1} Run Time : {2} ", this.countDir, this.countFiles, elapsedTime);
                textFile.Close();
            }
            else
            {
                foreach (string key in Keys)
                {
                    Console.WriteLine("\n\n\n\nGroup name : " + key);
                    foreach (string item in groupByExtList[key])
                    {
                        Console.WriteLine(item);
                    }   
                }
                Console.WriteLine("\n\n\nDirectories : {0} Files : {1} Run Time : {2} ", this.countDir, this.countFiles, elapsedTime);

            }
        }

        private string ComputeMD5Checksum(string path)
        {
            try
            {
                FileStream file = new FileStream(path, FileMode.Open);
                MD5 hashProcessor = new MD5CryptoServiceProvider();
                byte[] retVal = hashProcessor.ComputeHash(file);
                file.Close();

                StringBuilder hashString = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    hashString.Append(retVal[i].ToString("x2"));
                }

                return hashString.ToString();
            }
            catch
            {
                return "";
            }
        }
    }
    }
    

