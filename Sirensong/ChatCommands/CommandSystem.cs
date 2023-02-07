using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Command;
using Sirensong.ChatCommands.Interfaces;
using Sirensong.IoC.Internal;

namespace Sirensong.ChatCommands
{
    /// <summary>
    /// Wrapper for <see cref="CommandManager"/> that adds some additional functionality.
    /// </summary>
    [SirenServiceClass]
    public sealed class CommandSystem
    {
        private bool disposedValue;

        /// <summary>
        /// The commands registered to the <see cref="CommandManager"/> from this <see cref="CommandSystem"/>.
        /// </summary>
        private readonly HashSet<IDalamudCommand> localCommandInstances = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandSystem"/> class.
        /// </summary>
        private CommandSystem()
        {

        }

        /// <summary>
        /// Registers the type of the command to the command system if it is not already registered.
        /// </summary>
        /// <typeparam name="T">The type of command to register.</typeparam>
        /// <returns>If the command was successfully registered.</returns>
        public bool RegisterCommand<T>() where T : IDalamudCommand, new()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CommandSystem));
            }

            // Check if the command is already registered.
            var command = new T();
            if (SharedServices.CommandManager.Commands.ContainsKey(command.Name))
            {
                return false;
            }

            // If not, register it.
            if (SharedServices.CommandManager.AddHandler(command.Name, new CommandInfo(this.OnCommand)
            {
                HelpMessage = command.HelpMessage,
            }))
            {
                this.localCommandInstances.Add(command);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Unregisters the type of the command from the command system if it is registered.
        /// </summary>
        /// <typeparam name="T">The type of command to unregister.</typeparam>
        /// <returns>If the command was successfully unregistered.</returns>
        public bool UnregisterCommand<T>() where T : IDalamudCommand
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CommandSystem));
            }

            // Check if the command is registered.
            var command = this.localCommandInstances.FirstOrDefault(x => x.GetType() == typeof(T));
            if (command is null)
            {
                return false;
            }

            // If it is, unregister it.
            if (SharedServices.CommandManager.RemoveHandler(command.Name))
            {
                this.localCommandInstances.Remove(command);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Unregisters all commands registered to the command system.
        /// </summary>
        public void UnregisterAllCommands()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CommandSystem));
            }

            foreach (var command in this.localCommandInstances)
            {
                SharedServices.CommandManager.RemoveHandler(command.Name);
            }
            this.localCommandInstances.Clear();
        }

        /// <summary>
        /// Disposes of the command manager.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                foreach (var command in this.localCommandInstances)
                {
                    SharedServices.CommandManager.RemoveHandler(command.Name);
                }
                this.localCommandInstances.Clear();
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// The command handler for the <see cref="CommandManager"/>.
        /// </summary>
        /// <param name="command">The command that was executed.</param>
        /// <param name="arguments">The arguments passed to the command.</param>
        private void OnCommand(string command, string arguments)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CommandSystem));
            }

            this.localCommandInstances.FirstOrDefault(x => x.Name == command)?.ExecuteCommand(arguments);
        }
    }
}