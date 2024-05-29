using UnityEngine;
using UnityEditor;

public class MenuItems : MonoBehaviour
{
    [MenuItem("Tools/Data Management/Save Game")]
    private static void MenuSaveGame()
    {
        Debug.Log("Manually Saving");
        GameDataHandler.SaveData(0);
    }

    [MenuItem("Tools/Data Management/Load Game")]
    private static void MenuLoadGame()
    {
        Debug.Log("Manually Loading");
        GameDataHandler.LoadData(0);
    }

    // Menu Verification //

    [MenuItem("Tools/Data Management/Save Game", true)]
    private static bool MenuSaveGameVerify()
    {
        return Application.isPlaying;
    }

    [MenuItem("Tools/Data Management/Load Game", true)]
    private static bool MenuLoadGameVerify()
    {
        return Application.isPlaying;
    }
}