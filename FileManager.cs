using System;
using System.IO;
using Newtonsoft.Json;
namespace FileManager
{
    static class FileManager<T>
    {
        public static T Load(string Path)
        {
            if (!File.Exists(Path))
                return default(T);
            using (StreamReader reader = new StreamReader(Path))
            {
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }

        }
        public static void Save(T obj, string Path)
        {
            using (StreamWriter writer = new StreamWriter(Path))
            {
                writer.WriteLine(JsonConvert.SerializeObject(obj));
            }
        }
    }
}