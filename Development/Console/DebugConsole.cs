using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    #region Variables

    // Public //
    [SerializeField] private Canvas consoleGUI;

    // Class Only //
    private static DebugConsole instance;

    // Private //
    private InputManager inputManager;
    private bool debugMode = false;

    // Exposed Properties //
    public static DebugConsole Instance
    {
        get => instance;
        private set => instance = value;
    }

    #endregion

    private void OnEnable()
    {
        inputManager = InputManager.Instance;
        if (inputManager == null)
        {
            this.enabled = false;
        }
        inputManager.OnDebugConsole += OnDebugPressed;
        consoleGUI.enabled = false;
    }

    private void OnDisable()
    {
        inputManager.OnDebugConsole -= OnDebugPressed;
    }

    private void OnDebugPressed()
    {
        debugMode = !debugMode;
        if (debugMode)
        {
            consoleGUI.enabled = true;
        }

        else
        {
            consoleGUI.enabled = false;
        }

        DebugSystem.Log($"Debug Console: {debugMode}", LogType.Debug);
    }
}