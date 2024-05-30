using TMPro;
using UnityEngine;

namespace LCS
{
    namespace GUI
    {
        public class ConsoleDebug : MonoBehaviour, IGUIObject
        {
            [SerializeField] private InputData inputData;
            [SerializeField] private TextMeshProUGUI logText;
            [SerializeField] private GameObject consoleUI;

            private bool consoleEnabled = false;

            #region Start/Stop

            private void Awake()
            {
                Application.logMessageReceived += MessageRecieved;
            }

            private void OnEnable()
            {
                inputData.DebugEvent += ToggleConsole;
            }

            private void OnDisable()
            {
                inputData.DebugEvent -= ToggleConsole;
            }

            public void EnableGUI()
            {
                consoleEnabled = true;
                consoleUI.SetActive(true);
            }

            public void DisableGUI()
            {
                consoleEnabled = false;
                consoleUI.SetActive(false);
            }

            #endregion

            private void ToggleConsole()
            {
                consoleEnabled = !consoleEnabled;
                if (consoleEnabled)
                {
                    GUIHandler.ShowGUI(GUIType.GUI_DEBUG);
                }
                else
                {
                    GUIHandler.HideGUI();
                }
                string msg = consoleEnabled ? "ENTERING DEBUG MODE" : "EXITING DEBUG MODE";
                Debugger.LogConsole(msg, 0);
            }

            private void MessageRecieved(string log, string stack, LogType type)
            {
                switch (type)
                {
                    case LogType.Log:
                        logText.text += '\n' + $"<color=white>{log}</color>";
                        break;
                    case LogType.Warning:
                        logText.text += '\n' + $"<color=yellow>{log}</color>";
                        break;
                    case LogType.Error:
                        logText.text += '\n' + $"<color=red>{log}</color>";
                        break;
                    case LogType.Exception:
                        logText.text += '\n' + $"<color=red>{log}</color>";
                        break;
                }
            }
        }
    }
}