using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly HashSet<CommandBase> localCommandInstances = new();

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
        public bool RegisterCommand<T>() where T : CommandBase, new()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CommandSystem));
            }

            var command = new T();
            if (SharedServices.CommandManager.Commands.ContainsKey(command.Name))
            {
                return false;
            }

            if (command.Register())
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
        public bool UnregisterCommand<T>() where T : CommandBase
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(CommandSystem));
            }

            var command = this.localCommandInstances.FirstOrDefault(x => x.GetType() == typeof(T));
            if (command is null)
            {
                return false;
            }

            if (command.Unregister())
            {
                this.localCommandInstances.Remove(command);
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
                foreach (var command in this.localCommandInstances)
                {
                    command.Unregister();
                }
                this.localCommandInstances.Clear();
                this.disposedValue = true;
            }
        }
    }
}