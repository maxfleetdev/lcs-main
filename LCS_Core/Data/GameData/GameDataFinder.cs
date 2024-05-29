using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LCS
{
    namespace Data
    {
        public class GameDataFinder
        {
            private string filePath;
            private int dataIndex;
            private string dataName;
            private string dataPath;
            private string saveFolderPath = Application.persistentDataPath + folder;

            private const string folder = "/saves/";
            private const string fileExtension = ".lcs";

            public GameDataFinder()
            {
                this.filePath = Application.persistentDataPath + folder + dataName + "/";
                this.dataPath = Path.Combine(filePath, dataName + fileExtension);
            }
            public GameDataFinder(int index)
            {
                this.dataIndex = index;
                this.dataName = $"Data_{dataIndex}";
                this.filePath = Application.persistentDataPath + folder + dataName + "/";
                this.dataPath = Path.Combine(filePath, dataName + fileExtension);
            }

            /// <summary>
            /// Saves GameData to disk using JSON
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public bool SaveData(GameData data)
            {
                try
                {
                    if (File.Exists(dataPath))
                    {
                        Debugger.LogConsole($"Overwriting Data_{dataIndex}", 1);
                    }
                    Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
                    string data_to_store = JsonUtility.ToJson(data, true);
                    using (FileStream stream = new FileStream(dataPath, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(data_to_store);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when saving to file: " + dataPath + "\n" + e);
                    return false;
                }
                Debugger.LogConsole($"Saved to: {filePath}", 0);
                return true;
            }

            /// <summary>
            /// Loads GameData from disk using JSON
            /// </summary>
            /// <returns></returns>
            public GameData LoadData()
            {
                GameData loaded_data;
                if (File.Exists(dataPath))
                {
                    try
                    {
                        string data_to_load = "";
                        using (FileStream stream = new FileStream(dataPath, FileMode.Open))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                data_to_load = reader.ReadToEnd();
                            }
                        }
                        Debug.Log($"Loaded Save {dataPath}");
                        loaded_data = JsonUtility.FromJson<GameData>(data_to_load);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Error occured when loading file: " + dataPath + "\n" + e);
                        return null;
                    }
                }
                else
                {
                    Debugger.LogConsole($"No File exists with DataIndex {dataIndex}", 0);
                    return null;
                }
                return loaded_data;
            }

            /// <summary>
            /// Deletes GameData from disk
            /// </summary>
            /// <param name="index"></param>
            public void DeleteData()
            {
                if (File.Exists(dataPath))
                {
                    File.Delete(dataPath);
                    Directory.Delete(filePath);
                    Debugger.LogConsole($"Removed Data_{dataIndex}", 1);
                }

                else
                {
                    Debugger.LogConsole($"No File exists with DataIndex {dataIndex}", 0);
                }
            }

            /// <summary>
            /// Reads and returns all GameData from within the persistent data path
            /// </summary>
            /// <returns></returns>
            public List<GameData> LoadAllData()
            {
                List<GameData> loaded_data = new List<GameData>();
                if (Directory.Exists(saveFolderPath))
                {
                    foreach (string path in Directory.GetDirectories(saveFolderPath))
                    {
                        string[] files = Directory.GetFiles(path);
                        if (File.Exists(files[0]))
                        {
                            try
                            {
                                string data_to_load = "";
                                using (FileStream stream = new FileStream(files[0], FileMode.Open))
                                {
                                    using (StreamReader reader = new StreamReader(stream))
                                    {
                                        data_to_load = reader.ReadToEnd();
                                    }
                                }
                                Debug.Log($"Loaded File {files[0]}");
                                loaded_data.Add(JsonUtility.FromJson<GameData>(data_to_load));
                            }
                            catch (Exception e)
                            {
                                Debug.LogError("Error occured when loading file: " + files[0] + "\n" + e);
                                return null;
                            }
                        }
                    }
                }
                else
                {
                    Debugger.LogConsole("No SaveFolder exists", 1);
                    return null;
                }

                return loaded_data;
            }

            public int NextValidIndex()
            {
                List<GameData> all_data = LoadAllData();
                if (all_data == null)
                    return 0;
                List<int> all_indices = new List<int>();
                foreach (GameData data in all_data)
                {
                    all_indices.Add(data.DataIndex);
                }
                int nextIndex = 0;
                while (all_indices.Contains(nextIndex))
                {
                    nextIndex++;
                }
                return nextIndex;
            }
        }
    }
}