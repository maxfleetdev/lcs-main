using System;
using UnityEngine.SceneManagement;

public static class SceneHandler
{
    public static event Action<int> OnChangeScene;

    /// <summary>
    /// Changes scene from within the build using an int
    /// </summary>
    /// <param name="scene"></param>
    public static void ChangeScene(int scene)
    {
        OnChangeScene?.Invoke(scene);
    }

    /// <summary>
    /// Returns the current scenes index within the build
    /// </summary>
    /// <returns></returns>
    public static int GetCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}