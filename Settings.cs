using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace TinyCapture
{
    public static class Settings
    {
        private const string IniPath = ".\\TinyCapture.ini";

        public static readonly bool ShowTooltip = true;
        public static readonly string SavePath = Application.StartupPath;

        public static Keys CaptureWindowKey = Keys.F9;
        public static Keys CaptureScreenKey = Keys.F10;

        public static KeyModifier KeyModifier = KeyModifier.WIN;

        static Settings()
        {            
            try
            {
                if (!File.Exists(IniPath))
                    return;

                var lines = File.ReadAllLines(IniPath);

                foreach (var line in lines)
                {
                    if (line.Trim().StartsWith(";") || line.Trim().Length == 0) continue;
                    try
                    {
                        var s = line.Split('=');

                        var key = s[0].Trim();
                        var value = s[1].Trim();

                        if (string.Compare(key, "SavePath", true) == 0 && value.Length > 0)
                            SavePath = value;                        
                        if (string.Compare(key, "CaptureWindowKey", true) == 0)
                            CaptureWindowKey = (Keys)Enum.Parse(typeof(Keys), value);
                        if (string.Compare(key, "CaptureScreenKey", true) == 0)
                            CaptureScreenKey = (Keys)Enum.Parse(typeof(Keys), value);
                        if (string.Compare(key, "KeyModifier", true) == 0)
                            KeyModifier = (KeyModifier)Enum.Parse(typeof(KeyModifier), value);
                        if (string.Compare(key, "ShowTooltip", true) == 0)
                            ShowTooltip = Boolean.Parse(value);
                    }                    
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }                    
                }
            }            
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }            
        }

        public static bool Write()
        {
            var assembly = Assembly.GetEntryAssembly();
            try
            {
                using (var stream = assembly.GetManifestResourceStream(string.Format("{0}.TinyCapture.ini", assembly.GetName().Name)))
                {
                    if (stream == null) return false;
                    using (var reader = new StreamReader(stream))
                    {
                        var value = reader.ReadToEnd();
                        File.WriteAllText(IniPath, value);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }            
        }
    }
}
