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
    class ScanAndGroup
    {
        public int countFiles, countDir;
        public string pathToFile;
        public bool saveToFile { get; set;}
        public Dictionary<string, List<string>> filterFiles;

        public ScanAndGroup()
        {
            countFiles = 0;
            countDir = 0;
            pathToFile = "";
            saveToFile = false;
            filterFiles = new Dictionary<string, List<string>>();
        }

        public void ScanPath(string pathToDir, string nameOfFile = "result.txt")
        {
            this.pathToFile = pathToDir + "\\" + nameOfFile;
            DirectoryInfo dir = new DirectoryInfo(pathToDir);
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
            countDir = dir.GetDirectories().Count();
            countFiles = fileList.Count();


            Dictionary<string, string> filesAndHashes = new Dictionary<string, string>();
            
            foreach (var file in fileList)
            {
                filesAndHashes.Add(file.FullName, ComputeMD5Checksum(file.FullName));
            }

            FilterForFiles(filesAndHashes);
        }

        public void FilterForFiles (Dictionary<string, string> filesAndHashes)
        {
            foreach (var hash in filesAndHashes.Values)
            {
                List<string> similFiles = new List<string>();
                
                if (filterFiles.ContainsKey(hash))
                {
                    continue;
                }
                foreach (var similKey in filesAndHashes.Keys)
                {
                    if (hash == filesAndHashes[similKey])
                    {
                        similFiles.Add(similKey);
                    }
                }
                if (similFiles.Count > 1 && hash != "")
                {
                    filterFiles.Add(hash, similFiles);
                }
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
    

