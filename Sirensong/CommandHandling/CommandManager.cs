using System;
using System.Collections.Generic;
using Sirensong.IoC.Internal;

namespace Sirensong.CommandHandling
{
    /// <summary>
    /// Wrapper for <see cref="Dalamud.Game.Command.CommandManager"/> that adds some additional functionality.
    /// </summary>
    [SirenServiceClass]
    public sealed class CommandSystem
    {
        private bool disposedValue;

        /// <summary>
        /// the commands registered to the <see cref="Dalamud.Game.Command.CommandManager"/> from this <see cref="CommandSystem"/>.
        /// </summary>
        private readonly HashSet<CommandBase> localRegisteredCommands = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandSystem"/> class.
        /// </summary>
        private CommandSystem()
        {

        }

        /// <summary>
        /// Registers a command to the command system if it is not already registered.
        /// </summary>
        /// <param name="command">The command to register.</param>
        /// <returns>If the command was successfully registered.</returns>
        public bool RegisterCommand(CommandBase command)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CommandSystem));
            }

            if (SharedServices.CommandManager.Commands.ContainsKey(command.Name))
            {
                return false;
            }

            if (command.Register())
            {
                this.localRegisteredCommands.Add(command);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Unregisters a command from the command system if it is registered.
        /// </summary>
        /// <param name="command">The command to unregister.</param>
        /// <returns>If the command was successfully unregistered.</returns>
        public bool UnregisterCommand(CommandBase command)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CommandSystem));
            }

            if (!SharedServices.CommandManager.Commands.ContainsKey(command.Name))
            {
                return false;
            }

            if (command.Unregister())
            {
                this.localRegisteredCommands.Remove(command);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Disposes of the command manager.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                foreach (var command in this.localRegisteredCommands)
                {
                    command.Unregister();
                }
                this.localRegisteredCommands.Clear();
                this.disposedValue = true;
            }
        }
    }
}