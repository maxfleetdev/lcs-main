/*
    This is the script which holds all commands used within the
    debug console. These effect the player and their visuals.
    
    Only for development & testing purposes.
 */

/// <summary>
/// Sets infinite health for player
/// </summary>
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

/// <summary>
/// Changes the health of the current Player
/// </summary>
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

/// <summary>
/// Changes the view of the player from first to third person
/// True = First Person View
/// False = Third Person View
/// </summary>
public class ViewCommand : ICommand
{
    public void Execute(string command, string[] parameters)
    {
        // check format and check for command
        if (command.ToLower() != "fpview" || parameters.Length != 1 || !bool.TryParse(parameters[0], out bool isFirstPerson))
        {
            DebugSystem.Log("Invalid firstperson command format. Use 'firstperson=[true/false]'.", LogType.Warn);
            return;
        }

        DebugSystem.Log($"FirstPerson set to {isFirstPerson}.", LogType.Info);
        InstanceFinder.Player_Movement().ChangeView(isFirstPerson);
    }
}