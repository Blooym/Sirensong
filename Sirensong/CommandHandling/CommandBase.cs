using Dalamud.Game.Command;

namespace Sirensong.CommandHandling
{
    /// <summary>
    /// The base class for commands.
    /// </summary>
    public abstract class CommandBase
    {
        /// <summary>
        /// The command with the leading slash (/).
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The help message for the command.
        /// </summary>
        public virtual string HelpMessage { get; } = string.Empty;

        /// <summary>
        /// Whether the command should be shown in the help menu.
        /// </summary>
        public virtual bool ShowInHelp { get; } = true;

        /// <summary>
        /// The <see cref="Dalamud.Game.Command.CommandInfo"/> for the command.
        /// </summary>
        protected CommandInfo CommandInfo => new(this.OnCommandInvoke)
        {
            HelpMessage = this.HelpMessage,
            ShowInHelp = this.ShowInHelp,
        };

        /// <summary>
        /// The code to execute when the command is invoked.
        /// </summary>
        protected abstract CommandInfo.HandlerDelegate Execute { get; }

        /// <summary>
        /// The handler for when commands are invoked, which will call the <see cref="Execute"/> method if the command matches.
        /// </summary>
        protected CommandInfo.HandlerDelegate OnCommandInvoke => (command, args) =>
        {
            if (command == this.Name)
            {
                this.Execute(command, args);
            }
        };

        /// <summary>
        /// Registers the command to the <see cref="CommandSystem"/>.
        /// </summary>
        /// <returns>If the command was successfully registered.</returns>
        public bool Register() => SharedServices.CommandManager.AddHandler(this.Name, this.CommandInfo);

        /// <summary>
        /// Unregisters the command from the <see cref="CommandSystem"/>.
        /// </summary>
        /// <returns>If the command was successfully unregistered.</returns>
        public bool Unregister() => SharedServices.CommandManager.RemoveHandler(this.Name);

        /// <inheritdoc />
        public override string ToString() => $"{this.Name} - {this.HelpMessage}";
    }
}