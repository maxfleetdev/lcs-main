using UnityEngine;
using UnityEngine.SceneManagement;

/*
    This class loads all API's and the main menu scene.
    Cannot be called more than once.
 */

public class Bootstrapper : MonoBehaviour
{
    // Private //
    [SerializeField] private GameObject[] gameManagers;

    private static Bootstrapper instance;

    // Public Properties //
    public static Bootstrapper Instance
    {
        get => instance;
        private set => instance = value;
    }

    #region Scene Management

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DebugSystem.Log("Cannot have more than one bootstrapper!", LogType.Error);
            Destroy(this.gameObject);
        }

        foreach (var manager in gameManagers)
        {
            if (manager == null)
            {
                DebugSystem.Log($"{manager.name}: Manager not found!", LogType.Warn);
                return;
            }

            else
            {
                manager.SetActive(true);
            }
        }

        // called once bootstrapped
        DebugSystem.Log("API and Managers Loaded... OK", LogType.Debug);
        LoadMenu();
    }

    protected private void LoadMenu()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnMenuLoaded;
    }

    protected private void OnMenuLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.SetActiveScene(arg0);
        DebugSystem.Log("Main menu loaded... OK", LogType.Debug);
        return;
    }

    #endregion
}