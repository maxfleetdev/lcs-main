using UnityEngine;

namespace LCS
{
    namespace GUI
    {
        /// <summary>
        /// Acts as a wrapper for all GUI elements in-game. Simply toggles certain GUI's when subscribed to a GUIHandler.
        /// Can be called at any point in game. Loaded from Bootstrapper.
        /// </summary>
        public class GUIManager : MonoBehaviour
        {
            [SerializeField] private GameObject mainGUI;
            [Header("In-Game GUI")]
            [SerializeField] private GameObject saveScreenGUI;
            [SerializeField] private GameObject debugGUI;
            [SerializeField] private GameObject pickupGUI;
            [SerializeField] private GameObject settingsGUI;

            private GameObject enabledGUI = null;

            #region Start/Stop

            private void Awake()
            {
                GUIHandler.OnGUIToggle += ShowGUI;
                GUIHandler.OnGUIHide += HideGUI;
            }

            private void OnDestroy()
            {
                GUIHandler.OnGUIToggle -= ShowGUI;
                GUIHandler.OnGUIHide -= HideGUI;
            }

            #endregion

            #region GUI Logic

            private void ShowGUI(GUIType type)
            {
                // Start Core_GUI
                //mainGUI.SetActive(true);

                switch (type)
                {
                    case GUIType.GUI_SAVE_GAME:
                        EnableGUIElement(saveScreenGUI);
                        break;
                    case GUIType.GUI_DEBUG:
                        EnableGUIElement(debugGUI);
                        break;
                    case GUIType.GUI_PICKUP:
                        EnableGUIElement(pickupGUI);
                        break;
                    case GUIType.GUI_SETTINGS:
                        EnableGUIElement(settingsGUI);
                        break;
                    default:
                        return;
                }
            }

            private void EnableGUIElement(GameObject gui)
            {
                if (gui == null)
                    return;

                // Enable GUI
                if (enabledGUI != null)
                    enabledGUI.GetComponent<IGUIObject>().DisableGUI();
                enabledGUI = gui;
                enabledGUI.GetComponent<IGUIObject>().EnableGUI();
            }

            private void HideGUI()
            {
                // Reset GUI
                enabledGUI.GetComponent<IGUIObject>().DisableGUI();
            }

            #endregion
        }
    }
}