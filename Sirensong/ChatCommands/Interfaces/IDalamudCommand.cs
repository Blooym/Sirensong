namespace Sirensong.ChatCommands.Interfaces
{
    /// <summary>
    /// Represents a command for Dalamud.
    /// </summary>
    public interface IDalamudCommand
    {
        /// <summary>
        /// The name of the command.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The help message for the command when "/<see cref="Name"/> help" is executed.
        /// </summary>
        string HelpMessage { get; }

        /// <summary>
        /// The code to execute when the command is invoked without a subcommand.
        /// </summary>
        void ExecuteCommand(string arguments);
    }
}