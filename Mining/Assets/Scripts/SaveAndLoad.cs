using System.IO;
using UnityEngine;

namespace Techyte.General
{
    public class SaveAndLoad<Type>
    {
        public static void SaveJson(Type data, string path)
        {
            File.WriteAllText(path, JsonUtility.ToJson(data, true));
        }

        public static Type LoadJson(string path)
        {
            return JsonUtility.FromJson<Type>(File.ReadAllText(path));
        }
    }   
}