using System.Collections;
using TMPro;
using UnityEngine;

public class DebugPrintLog : MonoBehaviour
{
    // Class-only //
    protected private uint qsize = 15;
    protected private Queue myLogQueue = new Queue();
    protected private TextMeshProUGUI textMesh;

    #region Startup

    private void OnEnable()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    #endregion

    protected private void HandleLog(string condition, string stackTrace, UnityEngine.LogType type)
    {
        string warn_type = string.Empty;
        if (type == UnityEngine.LogType.Error ||  type == UnityEngine.LogType.Warning)
        {
            warn_type = $"[{type}]:";
        }
        myLogQueue.Enqueue($"{warn_type}{condition}");
        if (type == UnityEngine.LogType.Exception)
        {
            myLogQueue.Enqueue(stackTrace);
        }
        while (myLogQueue.Count > qsize)
        {
            myLogQueue.Dequeue();
        }
        textMesh.text = "\n" + string.Join("\n", myLogQueue.ToArray());
    }
}