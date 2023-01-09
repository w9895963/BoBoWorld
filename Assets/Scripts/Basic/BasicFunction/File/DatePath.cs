using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;



namespace FileBundle
{
    public class LocalFile
    {


        const string DataFolderName = "DataFolder";
        public static string DataFolderPath
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsPlayer:
                        return new DirectoryInfo(Application.dataPath).Parent.FullName + "\\" + DataFolderName;
                    case RuntimePlatform.WindowsEditor:
                        return new DirectoryInfo(Application.dataPath).Parent.Parent.FullName + "\\" + DataFolderName;
                    default:
                        return Application.dataPath;
                }

            }
        }



        public readonly string localPath;
        public string FullPath => Path.Combine(DataFolderPath, localPath);


        private LocalFile(string localPath)
        {
            this.localPath = localPath;
        }

        public static LocalFile TryGet(string localPath, bool autoCreateFolder = false)
        {
            LocalFile re = null;
            string path = Path.Combine(DataFolderPath, localPath);
            path = Path.GetFullPath(path);
            var dirPath = new FileInfo(path).Directory.FullName;
            bool notExists = !Directory.Exists(dirPath);
            if (notExists & autoCreateFolder)
            {
                Directory.CreateDirectory(dirPath);
            }

            re = new LocalFile(localPath);

            return re;
        }
        public static string GetLocalPath(string fullPath)
        {
            if (fullPath.IsEmpty()) return null;

            fullPath = Path.GetFullPath(fullPath);
            string dataFolderPath = DataFolderPath;
            dataFolderPath = Path.GetFullPath(dataFolderPath);
            string localPath = fullPath.TrimStart(dataFolderPath.ToCharArray());
            return localPath;
        }
    }

}
