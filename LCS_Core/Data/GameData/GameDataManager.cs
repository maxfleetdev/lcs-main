using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LCS
{
    namespace Data
    {
        public class GameDataManager : MonoBehaviour
        {
            private List<IDataPersistence> dataObjects = new List<IDataPersistence>();
            private GameDataFinder gameDataWriter = null;
            private GameData currentLoadData = null;

            #region Start/Stop

            private void Awake()
            {
                GameDataHandler.SaveScene += SaveScene;
                GameDataHandler.SaveNewScene += NewSaveScene;
                GameDataHandler.LoadScene += LoadScene;
                GameDataHandler.DeleteSave += DeleteData;
            }

            private void OnDisable()
            {
                dataObjects.Clear();
                GameDataHandler.SaveScene -= SaveScene;
                GameDataHandler.SaveNewScene -= NewSaveScene;
                GameDataHandler.LoadScene -= LoadScene;
                GameDataHandler.DeleteSave -= DeleteData;
            }

            #endregion

            #region Save Logic

            private void CacheObjects()
            {
                dataObjects.Clear();
                IEnumerable<IDataPersistence> persistence_objects = FindObjectsOfType<MonoBehaviour>().
                    OfType<IDataPersistence>();
                foreach (IDataPersistence mono in persistence_objects)
                {
                    dataObjects.Add(mono);
                }
            }

            private void SaveScene(int index)
            {
                // Load Writer
                gameDataWriter = new GameDataFinder(index);
                GameData data = gameDataWriter.LoadData();
                
                // Creates new GameData
                if (data == null)
                {
                    data = new GameData();
                    data.DataIndex = index;
                }

                // Save Game Information
                data.SaveLocation = SaveCache.GetSaveLocation();
                data.SaveTime = System.DateTime.Now.ToString();
                data.SaveAmount++;

                // Load current SceneData for overwriting
                data = SaveSceneData(data);

                // Find all scene objects
                CacheObjects();
                foreach (IDataPersistence objs in dataObjects)
                {
                    objs.SaveData(data);
                }

                // Save GameData
                gameDataWriter.SaveData(data);
            }

            private void NewSaveScene()
            {
                gameDataWriter = new GameDataFinder();
                int index = gameDataWriter.NextValidIndex();
                SaveScene(index);
            }

            private void LoadScene(int index)
            {
                // Get Data
                GameDataFinder gameDataWriter = new GameDataFinder(index);
                GameData data = gameDataWriter.LoadData();
                if (data == null)
                    return;
                currentLoadData = data;

                // Get SceneData Index
                LoadSceneData(data);
            }

            private void DeleteData(int index)
            {
                gameDataWriter = new GameDataFinder(index);
                gameDataWriter.DeleteData();
            }

            #endregion

            #region Scene Logic

            private GameData SaveSceneData(GameData data)
            {
                string current_scene = SceneHandler.CurrentSceneName();
                if (!data.SceneData.ContainsKey(current_scene))
                {
                    SceneData scene_data = new SceneData();
                    data.SceneData.Add(current_scene, scene_data);
                }
                data.CurrentScene = current_scene;
                return data;
            }

            private void LoadSceneData(GameData data)
            {
                string scene_name = data.CurrentScene;
                SceneHandler.OnSceneChanged += SceneDataLoaded;
                SceneHandler.ChangeScene(scene_name);
            }

            private void SceneDataLoaded(bool result)
            {
                if (result)
                {
                    CacheObjects();
                    foreach (IDataPersistence objs in dataObjects)
                    {
                        objs.LoadData(currentLoadData);
                    }
                }
                else
                {
                    Debugger.LogConsole($"ERROR: No Scene Exists ({currentLoadData.CurrentScene})", 2);
                }
                SceneHandler.OnSceneChanged -= SceneDataLoaded;
            }

            #endregion
        }
    }
}