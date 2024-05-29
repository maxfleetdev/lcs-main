using TMPro;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public void CommandSent(string cmd)
    {
        if (cmd == string.Empty)
            return;
        GetComponent<TMP_InputField>().text = string.Empty;
        Command.ExecuteCommand(cmd);
    }
}