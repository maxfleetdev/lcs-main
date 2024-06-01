using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    private string activeScene;

    #region Start/End

    private void Awake()
    {
        SceneHandler.OnChangeScene += ChangeScene;
    }

    private void OnDestroy()
    {
        SceneHandler.OnChangeScene -= ChangeScene;
    }

    #endregion

    private void ChangeScene(string scene)
    {
        activeScene = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadScene(scene));
    }

    // Unloads current scene, loads and activates new scene
    private IEnumerator LoadScene(string to_load)
    {
        // Load Requested Scene
        AsyncOperation scene_to_load = SceneManager.LoadSceneAsync(to_load, LoadSceneMode.Additive);
        scene_to_load.allowSceneActivation = false;
        while (!scene_to_load.isDone)
        {
            if (scene_to_load.progress >= 0.9f)
            {
                scene_to_load.allowSceneActivation = true;
            }
            yield return null;
        }

        // Unload Previous Scene
        AsyncOperation async_unload = SceneManager.UnloadSceneAsync(activeScene);
        while (!async_unload.isDone)
        {
            yield return null;
        }

        // Activate Scene
        Scene loaded_scene = SceneManager.GetSceneByName(to_load);
        if (loaded_scene != null && loaded_scene.isLoaded)
        {
            SceneManager.SetActiveScene(loaded_scene);
            Debugger.LogConsole($"Activated Scene: {loaded_scene.name}", 0);
        }

        // Call to listeners
        SceneHandler.SceneChanged();
    }
}