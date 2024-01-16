using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    // Class-only //
    private static MainCameraController instance;
    protected private Camera mainCam;
    protected private AudioListener audioListener;

    // Public Static //
    public static MainCameraController Instance
    {
        get => instance;
        private set => instance = value;
    }

    #region Startup/Shutdown

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        mainCam = GetComponent<Camera>();
        audioListener = GetComponent<AudioListener>();
    }

    private void OnDisable()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    #endregion

    /// <summary>
    /// Disables/Enables the static camera, depending on variable input
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleCamera(bool toggle)
    {
        mainCam.enabled = toggle;
        audioListener.enabled = toggle;
        return;
    }
}