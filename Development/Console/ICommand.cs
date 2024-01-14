public interface ICommand
{
    void Execute(string command, string[] parameters);
}