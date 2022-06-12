using System;
using System.IO;
using System.Xml.Serialization;

namespace Daw.Core
{
    public class OptionManager
    {
        const string FILENAME = "option.xml";
        const string PATH = "Daw";

        static string DataFolder
        {
            get
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), PATH);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }

        static string OptionPath => Path.Combine(DataFolder, FILENAME);

        public static Option Value { get; private set; }

        public static void Load()
        {
            if (!File.Exists(OptionPath)) { Value = Option.Default; return; }
            
            var x = XmlSerializer.FromTypes(new[] { typeof(Option) })[0];
            using var r = new StreamReader(OptionPath);
            try
            {
                Value = (Option)x.Deserialize(r);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Value = Option.Default;
            }
        }

        public static void Save()
        {
            var x = new XmlSerializer(typeof(Option));
            using var w = new StreamWriter(OptionPath);
            try
            {
                x.Serialize(w, Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
