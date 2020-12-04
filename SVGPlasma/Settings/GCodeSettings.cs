using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SVGPlasma
{
    public class GCodeSettings
    {
        public static readonly string SettingsFile = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SVGPlasma", "settings.cfg");
        public List<GCodeMachineSettings> machines { get; set; }
        public List<GCodeMaterialSettings> materials { get; set; }

        public GCodeSettings()
        {
            machines = new List<GCodeMachineSettings>();
            materials = new List<GCodeMaterialSettings>();
        }

        public static GCodeSettings Load()
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(SettingsFile));
            GCodeSettings gc;
            if (System.IO.File.Exists(SettingsFile))
            {
                System.IO.FileStream fs = System.IO.File.Open(SettingsFile, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                XmlSerializer xml = new XmlSerializer(typeof(GCodeSettings));
                gc = (GCodeSettings)xml.Deserialize(fs);
                fs.Close();
            }
            else
            {
                gc = new GCodeSettings();
            }            
            return gc;           
        }

        public void Save()
        {
            System.IO.FileStream fs = System.IO.File.Open(SettingsFile, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            XmlSerializer xml = new XmlSerializer(typeof(GCodeSettings));
            xml.Serialize(fs, this);
            fs.Close();
        }
    }
}
