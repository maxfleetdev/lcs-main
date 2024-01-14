using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    #region Variables
    // Class Only //
    protected private TMP_InputField cmdInput;
    protected private string previousCmd;
    protected private Dictionary<string, ICommand> commandDictionary;

    // Public //

    #endregion

    #region Command Functions

    // Protected Functions //
    protected private void Awake()
    {
        cmdInput = GetComponent<TMP_InputField>();
        if (cmdInput == null)
        {
            DebugSystem.Log("No input field found! Returning...", LogType.Error);
            this.enabled = false;
            return;
        }

        InitializeCommands();
    }

    private void InitializeCommands()
    {
        // Initialize the command dictionary
        commandDictionary = new Dictionary<string, ICommand>
        {
            { "godmode", new GodMode() },
            {"health", new HealthCommand() }
            // Add more commands as needed
        };
    }

    protected private void OnCommandClear()
    {
        // clears the current input field
        cmdInput.text = string.Empty;
        return;
    }

    // Public Functions //
    public void OnCommandEnter(string cmd)
    {
        previousCmd = cmd;
        OnCommandClear();

        // Split the command into components
        string[] cmdParts = cmd.Split('=');

        if (cmdParts.Length < 2)
        {
            DebugSystem.Log("Invalid command format! Use [command]=[parameters]", LogType.Warn);
            return;
        }

        string command = cmdParts[0].ToLower().Trim();
        string[] parameters = cmdParts.Skip(1).ToArray();

        // Execute the command if it exists in the dictionary
        if (commandDictionary.TryGetValue(command, out ICommand cmdObject))
        {
            cmdObject.Execute(command, parameters);
        }
        else
        {
            DebugSystem.Log($"Unknown command: {command}", LogType.Warn);
        }
    }

    #endregion
}