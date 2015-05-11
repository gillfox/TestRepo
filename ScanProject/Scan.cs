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
        string openFile, pathToFile;
        public Stopwatch stopWatch = new Stopwatch();
        
        public Scan()
        {
            countFiles = 0;
            countDir = 0;
            pathToFile = "";
            openFile = "";
            stopWatch.Start();
            
        }

        public void ScanPath(string pathToDir, string nameOfFile = "result.txt", string openF = "y")
        {
            this.openFile = openF;
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
                    new Key { HashCode = ComputeMD5Checksum(file.FullName) }   into fileGroup
                where fileGroup.Count() > 1
                select fileGroup;

            var list = queryDupFiles.ToList();

            ShowResults<Key, string>(queryDupFiles, openFile);
        }

        private void ShowResults<K, V>(IEnumerable<System.Linq.IGrouping<K, V>> groupByExtList, string openFile)
        {
            int numOfGroup = 1;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            if (openFile == "y")
            {   
                StreamWriter textFile = new StreamWriter(pathToFile);
                
                foreach (var filegroup in groupByExtList)
                {
                    textFile.WriteLine("\n\n\nGroup name - {0}", numOfGroup);

                    foreach (var fileName in filegroup)
                    {
                        textFile.WriteLine("{0}", fileName);
                    }
                    numOfGroup++;
                }
                textFile.WriteLine("\n\n\nDirectories : {0} Files : {1} Run Time : {2} ", this.countDir, this.countFiles, elapsedTime);
                textFile.Close();
            }
            else
            {
                foreach (var filegroup in groupByExtList)
                {
                    Console.WriteLine("\n\n\nGroup name - {0}", numOfGroup);
                    foreach (var fileName in filegroup)
                    {
                        Console.WriteLine("{0}", fileName);
                    }
                    numOfGroup++;
                }
                Console.WriteLine("\n\n\nDirectories : {0} Files : {1} Run Time : {2} ", this.countDir, this.countFiles, elapsedTime);

            }
        }

        private string ComputeMD5Checksum(string path)
        {
            try
            {
                FileStream file = new FileStream(path, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }
    }
    }
    

