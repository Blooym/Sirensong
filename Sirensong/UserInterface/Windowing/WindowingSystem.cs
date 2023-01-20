using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using Sirensong.IoC.Internal;
using Sirensong.UserInterface.Services;

namespace Sirensong.UserInterface.Windowing
{
    /// <summary>
    ///     Wrapper for <see cref="WindowSystem" /> that adds some additional functionality.
    /// </summary>
    [SirenServiceClass]
    public sealed class WindowingSystem : IDisposable
    {
        /// <summary>
        ///     The linked Dalamud <see cref="WindowSystem" />
        /// </summary>
        private readonly WindowSystem windowSystem;

        /// <summary>
        ///     The namespace of the plugin that owns this <see cref="WindowingSystem"/>.
        /// </summary>
        private readonly string uiNamespace;

        /// <summary>
        ///     The config window to use for <see cref="UiBuilder.OpenConfigUi"/>.
        /// </summary>
        public Window? ConfigWindow { get; private set; }

        /// <inheritdoc cref="Clipboard" />
        private ClipboardService Clipboard { get; } = SirenCore.IoC.GetOrCreateService<ClipboardService>();

        /// <summary>
        ///    Constructs a new <see cref="WindowingSystem"/>.
        /// </summary>
        internal WindowingSystem()
        {
            this.uiNamespace = SirenCore.InitializerName;
            this.windowSystem = new(this.uiNamespace);
            SharedServices.UiBuilder.Draw += this.Draw;
        }

        /// <summary>
        ///     Disposes of the windowing manager and all windows that are bound to it.
        /// </summary>
        public void Dispose()
        {
            SharedServices.UiBuilder.Draw -= this.Draw;
            SharedServices.UiBuilder.OpenConfigUi -= this.ToggleConfigWindow;
            foreach (var disposable in this.windowSystem.Windows.OfType<IDisposable>())
            {
                disposable.Dispose();
                SirenLog.IVerbose($"Disposed of window {disposable}");
            }
            this.windowSystem.RemoveAllWindows();
        }

        /// <summary>
        ///     Draws all windows in the windowing system, with Sirens added providers.
        /// </summary>
        private void Draw()
        {
            if (!this.AnyWindowsOpen())
            {
                return;
            }

            this.windowSystem.Draw();
            this.Clipboard.UseKeyboardShortcuts();
        }

        /// <summary>
        ///     Sets the config window to use for <see cref="UiBuilder.OpenConfigUi"/>.
        /// </summary>
        /// <param name="window">The window to use as the config window.</param>
        private void SetConfigWindow(Window window)
        {
            SirenLog.IVerbose(this.ConfigWindow == window
                ? $"Overwriting existing config window {this.ConfigWindow.WindowName} with {window.WindowName} for {this.uiNamespace}"
                : $"Setting config window for {this.uiNamespace}");

            if (this.ConfigWindow == null)
            {
                SharedServices.UiBuilder.OpenConfigUi += this.ToggleConfigWindow;
            }
            this.ConfigWindow = window;
        }

        /// <summary>
        ///     Unset the config window for <see cref="UiBuilder.OpenConfigUi"/>.
        /// </summary>
        private void UnsetConfigWindow()
        {
            if (this.ConfigWindow != null)
            {
                SharedServices.UiBuilder.OpenConfigUi -= this.ToggleConfigWindow;
                this.ConfigWindow = null;
            }
            SirenLog.IVerbose($"Unset config window for {this.uiNamespace}");
        }

        /// <summary>
        ///    Gets all windows in the windowing system.
        /// </summary>
        /// <returns>An array of all windows in the windowing system.</returns>
        public IReadOnlyList<Window> Windows => this.windowSystem.Windows;

        /// <summary>
        ///     Gets a window by name.
        /// </summary>
        /// <param name="name">The name of the window to get.</param>
        /// <returns>The window with the specified name, or null if it does not exist.</returns>
        public Window? GetWindow(string name) => this.windowSystem.GetWindow(name);

        /// <summary>
        ///     Gets a window by type.
        /// </summary>
        /// <typeparam name="T">The type of window to get.</typeparam>
        /// <returns>The first window with the specified type in the windowing system, or null if it does not exist.</returns>
        public Window? GetWindow<T>() where T : Window => this.Windows.FirstOrDefault(window => window is T);

        /// <summary>
        ///     Tries to get a window by name.
        /// </summary>
        /// <param name="name">The name of the window to get.</param>
        /// <param name="windowOut">The window with the specified name, or null if it does not exist.</param>
        public void TryGetWindow(string name, out Window? windowOut) => windowOut = this.windowSystem.GetWindow(name);

        /// <summary>
        ///     Tries to get a window by type.
        /// </summary>
        /// <typeparam name="T">The type of window to get.</typeparam>
        /// <param name="windowOut">The window with the specified type, or null if it does not exist.</param>
        public void TryGetWindow<T>(out Window? windowOut) where T : Window => windowOut = this.GetWindow<T>();

        /// <summary>
        ///     Adds a window to the windowing system.
        /// </summary>
        /// <remarks>
        ///     If multiple windows are added as config windows, the last one added will be used.
        /// </remarks>
        /// <param name="window">The window to add.</param>
        /// <param name="isConfigWindow">Whether or not the window is the config window.</param>
        public void AddWindow(Window window, bool isConfigWindow = false)
        {
            this.windowSystem.AddWindow(window);

            if (isConfigWindow)
            {
                this.SetConfigWindow(window);
            }

            SirenLog.IVerbose($"Added {window.WindowName} to {this.uiNamespace}");
        }

        /// <summary>
        ///     Adds all passed windows to the windowing system.
        /// </summary>
        /// <remarks>
        ///    If multiple windows are added as config windows, the last one added will be used.
        /// </remarks>
        /// <params name="windows">The windows to add.</params>
        public void AddWindows(Dictionary<Window, bool> windows)
        {
            foreach (var (window, isConfigWindow) in windows)
            {
                this.AddWindow(window, isConfigWindow);
            }
        }

        /// <summary>
        ///     Removes a window from the windowing system and disposes of it if it implements <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="window">The window to remove.</param>
        public void RemoveWindow(Window window)
        {
            if (this.ConfigWindow == window)
            {
                this.UnsetConfigWindow();
            }

            if (window is IDisposable disposable)
            {
                disposable.Dispose();
                SirenLog.IVerbose($"Disposed of {disposable.GetType().Name} for {this.uiNamespace}");
            }

            this.windowSystem.RemoveWindow(window);
            SirenLog.IVerbose($"Removed {window.WindowName} from {this.uiNamespace}");
        }

        /// <summary>
        ///    Checks to see if any windows are open.
        /// </summary>
        /// <returns>True if any windows are open, false otherwise.</returns>
        public bool AnyWindowsOpen()
        {
            foreach (var window in this.windowSystem.Windows)
            {
                if (window.IsOpen)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Toggles the open state of the configuration window.
        /// </summary>
        public void ToggleConfigWindow()
        {
            if (this.ConfigWindow == null)
            {
                return;
            }
            this.ConfigWindow.IsOpen = !this.ConfigWindow.IsOpen;
        }
    }
}
