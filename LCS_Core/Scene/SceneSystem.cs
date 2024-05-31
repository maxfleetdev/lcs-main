using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    private int activeScene;

    #region Start/End

    private void Awake()
    {
        SceneHandler.OnChangeScene += ChangeActiveScene;
    }

    private void OnDestroy()
    {
        SceneHandler.OnChangeScene -= ChangeActiveScene;
    }

    #endregion

    private void ChangeActiveScene(int scene)
    {
        activeScene = scene;
        SceneManager.LoadScene(activeScene, LoadSceneMode.Single);
    }

    private void SaveCurrentScene()
    {

    }
}