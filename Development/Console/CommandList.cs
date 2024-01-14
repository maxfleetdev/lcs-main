/*
    This is the script which holds all commands used within the
    debug console. These effect the player and their visuals.
    
    Only for development & testing purposes.
 */

public class GodMode : ICommand
{
    public void Execute(string command, string[] parameters)
    {
        // check format and check for command
        if (command.ToLower() != "godmode" || parameters.Length != 1 || !bool.TryParse(parameters[0], out bool godModeValue))
        {
            DebugSystem.Log("Invalid godmode command format. Use 'godmode=[true/false]'.", LogType.Warn);
            return;
        }

        DebugSystem.Log($"God mode set to {godModeValue}.", LogType.Info);
    }
}

public class HealthCommand : ICommand
{
    public void Execute(string command, string[] parameters)
    {
        // Implement Health command logic
        if (parameters.Length != 1 || !int.TryParse(parameters[0], out int healthValue))
        {
            DebugSystem.Log("Invalid health command format. Use 'health=[value]'.", LogType.Warn);
            return;
        }

        DebugSystem.Log($"Setting health to {healthValue}.", LogType.Info);
    }
}