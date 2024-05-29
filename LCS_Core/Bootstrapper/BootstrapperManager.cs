using HeathenEngineering.SteamworksIntegration;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapperManager : MonoBehaviour
{
    [SerializeField] private bool enableSteam = true;
    [SerializeField] private InputData input;

    private SteamworksBehaviour steamworks;

    private void Awake()
    {
        if (enableSteam)
        {
            steamworks = GetComponent<SteamworksBehaviour>();
            steamworks.enabled = true;
        }
        else
        {
            Boostrapped();
        }        
    }

    // Bootstrap LCS_Core
    public void Boostrapped()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        SceneManager.LoadScene("Data_Scene_Additive", LoadSceneMode.Additive);
        SceneManager.LoadScene("GUI_Scene_Additive", LoadSceneMode.Additive);
        ItemCache.CacheItems();
        DifficultyCache.CacheDifficulty();
        input.Construct();
    }
}