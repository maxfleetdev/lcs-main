using System;
using UnityEngine.SceneManagement;

public static class SceneHandler
{
    public static event Action<string> OnChangeScene;
    public static event Action OnSceneChanged;

    /// <summary>
    /// Changes scene from within the build using an int
    /// </summary>
    /// <param name="scene"></param>
    public static void ChangeScene(string scene)
    {
        OnChangeScene?.Invoke(scene);
    }

    /// <summary>
    /// Called once the SceneSystem has loaded and activated the newest scene
    /// </summary>
    public static void SceneChanged()
    {
        OnSceneChanged?.Invoke();
    }

    /// <summary>
    /// Returns the current scenes index within the build
    /// </summary>
    /// <returns></returns>
    public static int CurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// Returns the current scenes name
    /// </summary>
    /// <returns></returns>
    public static string CurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}