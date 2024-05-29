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
                CacheObjects();
                GameDataFinder gameDataWriter = new GameDataFinder(index);
                GameData data = gameDataWriter.LoadData();
                if (data == null)
                    return;
                foreach (IDataPersistence objs in dataObjects)
                {
                    objs.LoadData(data);
                }
            }

            private void DeleteData(int index)
            {
                gameDataWriter = new GameDataFinder(index);
                gameDataWriter.DeleteData();
            }

            #endregion
        }
    }
}