using System;
using System.IO;
using UnityEngine;

namespace LCS
{
    namespace Data
    {
        public class SettingsDataFinder
        {
            // Variables for struct
            private readonly string filePath = Application.persistentDataPath;
            private readonly string name = "Settings";
            private readonly string fileExtension = ".lcs";

            public bool SaveSettings(SettingsData data)
            {
                string save_path = Path.Combine(filePath, name + fileExtension);
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(save_path));
                    string data_to_store = JsonUtility.ToJson(data, true);
                    using (FileStream stream = new FileStream(save_path, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(data_to_store);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when saving to file: " + save_path + "\n" + e);
                    return false;
                }
                Debugger.LogConsole($"Saved to: {filePath}", 0);
                return true;
            }

            public SettingsData LoadSettings()
            {
                string load_path = Path.Combine(filePath, name + fileExtension);
                SettingsData loaded_data = null;
                if (File.Exists(load_path))
                {
                    try
                    {
                        string data_to_load = "";
                        using (FileStream stream = new FileStream(load_path, FileMode.Open))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                data_to_load = reader.ReadToEnd();
                            }
                        }
                        Debug.Log($"Loaded File {load_path}");
                        loaded_data = JsonUtility.FromJson<SettingsData>(data_to_load);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Error occured when loading file: " + load_path + "\n" + e);
                        return null;
                    }
                }
                return loaded_data;
            }
        }
    }
}