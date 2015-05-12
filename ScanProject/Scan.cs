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

        public void ScanPath(string pathToDir, string nameOfFile = "result.txt", bool openFileOrNot = true)
        {
            this.pathToFile = pathToDir + "\\" + nameOfFile;
            string startFolder = pathToDir;
            int charsToSkip = startFolder.Length;

            DirectoryInfo dir = new DirectoryInfo(startFolder);
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
            countDir = dir.GetDirectories().Count();
            countFiles = fileList.Count();
            

            var queryDupFiles =
                from file in fileList
                group file.FullName.Substring(charsToSkip) by
                    new Key { HashCode = ComputeMD5Checksum(file.FullName) } into fileGroup
                where fileGroup.Count() > 1 && fileGroup.Key.HashCode != ""
                select fileGroup;
            
            ShowResults<Key, string>(queryDupFiles, openFileOrNot);
        }

        private void ShowResults<K, V>(IEnumerable<System.Linq.IGrouping<K, V>> groupByExtList, bool saveToFile = true)
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            if (saveToFile)
            {   
                StreamWriter textFile = new StreamWriter(pathToFile);
                
                foreach (var filegroup in groupByExtList)
                {
                    textFile.WriteLine("\n\n\nGroup name - {0}", filegroup.Key.ToString());

                    foreach (var fileName in filegroup)
                    {
                        textFile.WriteLine("{0}", fileName);
                    }
                }
                textFile.WriteLine("\n\n\nDirectories : {0} Files : {1} Run Time : {2} ", this.countDir, this.countFiles, elapsedTime);
                textFile.Close();
            }
            else
            {
                foreach (var filegroup in groupByExtList)
                {
                    Console.WriteLine("\n\n\nGroup name - {0}", filegroup.Key.ToString());
                    foreach (var fileName in filegroup)
                    {
                        Console.WriteLine("{0}", fileName);
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
    

