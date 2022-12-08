using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace Global
{
    public static class FileUtility
    {

        public static string RootFolderPath
        {
            get
            {
                string path = default;
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor: //<path to project folder>/Assets
                    case RuntimePlatform.WindowsEditor:
                        path = Application.dataPath;
                        break;
                    case RuntimePlatform.WindowsPlayer: //<path to executablename_Data folder>
                        path = Application.dataPath;
                        break;
                    case RuntimePlatform.OSXPlayer: //<path to player app bundle>/Contents
                        break;
                    default:
                        break;
                }

                if (path != default)
                {
                    path = Path.GetDirectoryName(path);
                }
                path = path.Replace(@"\", "/");
                return path;
            }
        }


        public static string GetFullPath(string localPath)
        {
            return FileUtility.CombinePath(RootFolderPath, localPath);
        }


        public static List<LocalFile> GetAllFiles(string path, string[] extensions = null)
        {
            List<LocalFile> all = new List<LocalFile>();
            if (Directory.Exists(path))
            {
                int level = 0;
                LocalFile curr = new LocalFile(path, true);
                List<LocalFile> currentFolders = new List<LocalFile> { curr };
                do
                {
                    level++;
                    List<LocalFile> newFoundFolders = new List<LocalFile>();
                    currentFolders.ForEach(((x) =>
                    {
                        string[] files = Directory.GetFiles(x.FullPath);
                        files.ForEach((f) =>
                        {
                            bool extensionTest = true;
                            if (extensions != null)
                            {
                                if (!extensions.Contains(Path.GetExtension(f)))
                                {
                                    extensionTest = false;
                                }
                            }
                            if (extensionTest)
                            {
                                LocalFile file = new LocalFile(f, true);
                                all.Add(file);
                            }
                        });
                        string[] dirs = Directory.GetDirectories((string)x.FullPath);
                        dirs.ForEach((dir) =>
                        {
                            LocalFile dirFile = new LocalFile(dir, true);
                            newFoundFolders.Add(dirFile);

                            bool extensionTest = true;
                            if (extensions != null)
                            {
                                if (!extensions.Contains(Path.GetExtension(dir)))
                                {
                                    extensionTest = false;
                                }
                            }
                            if (extensionTest)
                            {
                                all.Add(dirFile);
                            }
                        });
                    }));
                    currentFolders = newFoundFolders;
                } while (currentFolders.Count > 0);
            }
            return all;
        }


        public static List<string> GetFiles(string localPath, List<string> extensions)
        {
            List<string> result = new List<string>();
            string path = GetFullPath(localPath);
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                result = files.ToList().FindAll((x) => extensions.Contains(Path.GetExtension(x)));
            }
            return result;
        }


        public static void WriteAllText(string fullPath, string content)
        {
            string folder = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            File.WriteAllText(fullPath, content);
        }


        public static bool LoadImage(string localPath, Texture2D texture, string name = null)
        {
            string path = GetFullPath(localPath);
            if (File.Exists(path))
            {
                byte[] bytes = File.ReadAllBytes(path);
                bool v = ImageConversion.LoadImage(texture, bytes);
                return v;
            }

            if (name != null)
            {
                texture.name = name;
            }
            else
            {
                texture.name = Path.GetFileName(path);
            }
            return false;
        }

        internal static string CombinePath(string fullPath, string dataFilename)
        {
            string path = Path.Combine(fullPath, dataFilename);
            return path.Replace(@"\", "/");
        }

        public static string FindFile(string folderPath, string fileName)
        {
            string[] files = Directory.GetFiles(folderPath);
            string filePath = files.ToList().Find((path) => Path.GetFileName(path) == fileName);
            return filePath;
        }
        public static string GetLocalPath(string fullPath)
        {
            return fullPath.Remove(0, RootFolderPath.Length + 1);
        }



        public static LocalFile GetFile(string fullPath)
        {
            LocalFile file = null;
            if (fullPath.Contains(RootFolderPath))
            {
                if (File.Exists(fullPath) | Directory.Exists(fullPath))
                {
                    file = new LocalFile(fullPath, true);
                }
            }
            return file;
        }

        public static void BuildNecessaryFolder(string path)
        {

        }


        //* Class Definition
        [System.Serializable]
        public class LocalFile
        {
            public string localPath;
            //* Public Property
            public string FullPath => GetFullPath(localPath);
            public bool IsFolder => Directory.Exists(FullPath);
            public bool IsFile => File.Exists(FullPath);
            public LocalFile Parent => new LocalFile(Path.GetDirectoryName(FullPath), true);

            public LocalFile(string path, bool isFullPath = false)
            {
                if (isFullPath)
                {
                    this.localPath = GetLocalPath(path);
                }
                else
                {
                    this.localPath = path;
                }

            }


            //* Public Property
            public string Name => Path.GetFileName(FullPath);
            public string NameNoSuffix => Path.GetFileNameWithoutExtension(FullPath);
            public string Extension => Path.GetExtension(FullPath).ToLower();
            //* Public Method
            public LocalFile FindFileSamePlace(string name)
            {
                return Parent.GetAllChildren().Find((x) => x.Name == name);
            }
            public List<LocalFile> GetAllChildren()
            {
                List<LocalFile> result = new List<LocalFile>();
                if (IsFolder)
                {
                    string fullPath = FullPath;
                    var files = Directory.GetFiles(fullPath).ToList().Select((x) => new LocalFile(x, true)).ToList();
                    result.AddRange(files);
                    var folders = Directory.GetDirectories(fullPath).ToList().Select((x) => new LocalFile(x, true)).ToList();
                    result.AddRange(folders);
                }
                return result;
            }

            public T ReadJson<T>()
            {
                string v = ReadText();
                return JsonUtility.FromJson<T>(v);
            }
            public string ReadText()
            {
                return File.ReadAllText(FullPath);
            }

            public void CreateFolder(string modFolderName)
            {
                Directory.CreateDirectory(Path.Combine(FullPath, modFolderName));
            }
        }


    }


}