using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class PlaytestMenu : MonoBehaviour
{
    [MenuItem("Tools/Debug/Playtest _F5")]
    private static void Play()
    {
        if (!Application.isPlaying)
        {
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            EditorSceneManager.OpenScene("Assets/Scenes/Boostrapper_Additive.unity");
            EditorApplication.EnterPlaymode();
        }

        else
        {
            EditorApplication.ExitPlaymode();
            EditorApplication.playModeStateChanged += LoadPreviousScene;
        }
    }

    private static void LoadPreviousScene(PlayModeStateChange change)
    {
        if (change == PlayModeStateChange.EnteredEditMode)
        {
            EditorApplication.playModeStateChanged -= LoadPreviousScene;
            EditorSceneManager.OpenScene($"Assets/Scenes/SampleScene.unity");
        }
    }
}