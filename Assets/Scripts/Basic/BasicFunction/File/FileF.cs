using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileF
{
    public static Texture2D LoadTexture(string path, FilterMode filterMode = FilterMode.Point)
    {
        return FileBundle.TextureLoader.LoadTexture(path, filterMode);
    }
    public static Texture2D LoadTextureFromDateFolder(string localpath, FilterMode filterMode = FilterMode.Point)
    {
        return FileBundle.TextureLoader.LoadTexture(FileF.GetFullPathFromDataFolder(localpath), filterMode);
    }
    public static Sprite LoadSprite(string path, float pixelsPerUnit = 20)
    {
        return FileBundle.TextureLoader.LoadSprite(path, pixelsPerUnit);
    }


    #region Path And File

    public static string DataFolderPath => FileBundle.LocalFile.DataFolderPath;


    public static string GetFullPathFromDataFolder(string localPath, bool autoCreateMissFolder = false)
    {
        return FileBundle.LocalFile.TryGet(localPath, autoCreateMissFolder)?.FullPath;
    }
    public static string GetLocalPathFromDataFolder(string fullPath)
    {
      
        return FileBundle.LocalFile.GetLocalPath(fullPath);
    }

    public static bool AreSamePath(string path1, string path2)
    {
        return Path.GetFullPath(path1) == Path.GetFullPath(path2);
    }

    public static string[] GetAllFilesInFolder(string path, string match = "*", bool includeChildren = false)
    {
        if (!Directory.Exists(path)) { return null; }
        List<string> paths = Directory.GetFiles(path, match).ToList();
        if (includeChildren)
        {
            List<string> folders = Directory.GetDirectories(path).ToList();
            while (folders.Count > 0)
            {
                List<string> files = folders.SelectMany((x) => Directory.GetFiles(x, match)).ToList();
                folders = folders.SelectMany((x) => Directory.GetDirectories(x)).ToList();
                paths.AddRange(files);
            }
        }
        return paths.ToArray();
    }




    public static string[] GetAllFilesInLocalFolder(string localPath, string match = "*", bool includeChildren = false)
    {
        return GetAllFilesInFolder(GetFullPathFromDataFolder(localPath), match, includeChildren);
    }

    public static string[] GetAllFoldersFromLocal(string localPath)
    {
        return Directory.GetDirectories(GetFullPathFromDataFolder(localPath));
    }

    public static string GetFolderName(string path)
    {
        DirectoryInfo file = new DirectoryInfo(path);
        return file.Parent.Name;
    }

    public static string GetFolderPath(string path)
    {
        DirectoryInfo file = new DirectoryInfo(path);
        return file.Parent.FullName;
    }

    public static string GetFilePathInSameFolder(string path, string fileName)
    {

        return Path.GetDirectoryName(path) + "\\" + fileName;
    }
    #endregion
    // * Region Path And File End---------------------------------- 




    public static void ReWriteOrAddLine(string path, string title, string newLine)
    {
        List<string> list = File.ReadAllLines(path).ToList();
        int v = list.FindIndex((x) => x.StartsWith(title));

        if (v >= 0)
        {
            list[v] = newLine;
        }
        else
        {
            list.Add(newLine);
        }

        File.WriteAllLines(path, list);
    }
    public static void WriteLine(string path, int index, string newLine)
    {
        List<string> list = File.ReadAllLines(path).ToList();
        if (list.Count > index)
        {
            list[index] = newLine;
        }
        else
        {
            list.AddToIndex(index, newLine);
        }


        File.WriteAllLines(path, list);
    }

}
