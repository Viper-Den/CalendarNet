using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WpfApp4
{
    public class MapElement
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Text { get; set; } = "Text";
        public string Style { get; set; }
    }

    public class MapFile
    {
        public DateTime DateOfCreating { get; set; }
        public IList<MapElement> Elements { get; set; } = new List<MapElement>();
    }
    
    public class MapFileFactory
    {
        public const string File_Extension = ".mf";
        public const string File_Filter = "Map File|*.mf";
        public static string ConvertToJSON(MapFile value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }
        public static MapFile ConvertFromJSON(string value)
        {
            var r = JsonConvert.DeserializeObject<MapFile>(value);

            if (r == null)
                r = new MapFile();

            return r;
        }
        public static void SaveSettingsToJsonFile(MapFile value, string filePath)
        {
            var directoryName = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(filePath);

            File.WriteAllText(filePath, MapFileFactory.ConvertToJSON(value));
        }
        public static MapFile LoadSettingsFromJsonFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);
            if (!(Path.GetExtension(filePath) == MapFileFactory.File_Extension))
                throw new Exception("Wrong File Extension");

            return MapFileFactory.ConvertFromJSON(File.ReadAllText(filePath));
        }
    }
}
