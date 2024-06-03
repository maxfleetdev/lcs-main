using System;
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
                // Save Events
                GameDataHandler.SaveScene += SaveScene;
                GameDataHandler.SaveNewScene += NewSaveScene;
                GameDataHandler.LoadScene += LoadScene;
                GameDataHandler.DeleteSave += DeleteData;

                // Scene Events
                SceneHandler.OnSceneChanged += SceneDataLoaded;
            }

            private void OnDisable()
            {
                // Flush Events
                dataObjects.Clear();
                GameDataHandler.SaveScene -= SaveScene;
                GameDataHandler.SaveNewScene -= NewSaveScene;
                GameDataHandler.LoadScene -= LoadScene;
                GameDataHandler.DeleteSave -= DeleteData;
                SceneHandler.OnSceneChanged -= SceneDataLoaded;
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
                data.SaveTime = DateTime.Now.ToString();
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
                // Get Data from Index
                GameDataFinder gameDataWriter = new GameDataFinder(index);
                GameData data = gameDataWriter.LoadData();
                if (data == null)
                    return;

                // Cache Current Save
                currentLoadData = data;

                // Load Previous Scene
                string scene_name = currentLoadData.CurrentScene;
                SceneHandler.ChangeScene(scene_name);
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
                
                // Save Current Scene into SceneData
                if (!data.SceneData.ContainsKey(current_scene))
                {
                    SceneData scene_data = new SceneData();
                    data.SceneData.Add(current_scene, scene_data);
                }
                data.CurrentScene = current_scene;
                return data;
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
            }

            #endregion
        }
    }
}