using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;




namespace FileBundle
{
    public static class TextureLoader
    {
        public static Dictionary<string, Texture2D> PathTextureDict = new Dictionary<string, Texture2D>();
        public static Texture2D LoadTexture(string path, FilterMode filterMode = FilterMode.Point)
        {
            Texture2D texture = null;
            PathTextureDict.TryGetValue(path, out texture);
            if (texture == null)
            {
                if (File.Exists(path))
                {
                    texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                    texture.filterMode = filterMode;
                    byte[] bytes = File.ReadAllBytes(path);
                    bool loadSuccess = ImageConversion.LoadImage(texture, bytes);
                    if (loadSuccess)
                    {
                        texture.name = Path.GetFileName(path);
                        PathTextureDict.Add(path, texture);
                    }
                }
            }
            return texture;
        }

        public static Sprite LoadSprite(string path, float pixelsPerUnit = 20)
        {
            Texture2D tex = LoadTexture(path);
            Rect rect = new Rect(0, 0, tex.width, tex.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(tex, rect, pivot, pixelsPerUnit);
            return sprite;

        }
        public static string GetPath(Texture2D tex)
        {
            string key = null;
            List<KeyValuePair<string, Texture2D>> list = PathTextureDict.ToList();
            KeyValuePair<string, Texture2D> pair = list.Find((x) => x.Value == tex);
            key = pair.Key;
            return key;
        }


    }
}

