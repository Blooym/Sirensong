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
    /// Wrapper for <see cref="WindowSystem" /> that adds some additional functionality.
    /// </summary>
    [SirenServiceClass]
    public sealed class WindowingSystem : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// The linked Dalamud <see cref="WindowSystem" />
        /// </summary>
        private readonly WindowSystem windowSystem;

        /// <summary>
        /// The namespace of the plugin that owns this <see cref="WindowingSystem"/>.
        /// </summary>
        private readonly string uiNamespace;

        /// <summary>
        /// The config window to use for <see cref="UiBuilder.OpenConfigUi"/>.
        /// </summary>
        public Window? ConfigWindow { get; private set; }

        /// <inheritdoc cref="Clipboard" />
        private ClipboardService Clipboard { get; } = SirenCore.IoC.GetOrCreateService<ClipboardService>();

        /// <summary>
        /// Constructs a new <see cref="WindowingSystem"/>.
        /// </summary>
        private WindowingSystem()
        {
            this.uiNamespace = SirenCore.InitializerName;
            this.windowSystem = new(this.uiNamespace);
            SharedServices.UiBuilder.Draw += this.Draw;
        }

        /// <summary>
        /// Disposes of the windowing manager and all windows that are bound to it.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                SharedServices.UiBuilder.Draw -= this.Draw;
                SharedServices.UiBuilder.OpenConfigUi -= this.ToggleConfigWindow;
                foreach (var disposable in this.windowSystem.Windows.OfType<IDisposable>())
                {
                    disposable.Dispose();
                    SirenLog.Debug($"Disposed of window {disposable}");
                }
                this.windowSystem.RemoveAllWindows();
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Draws all windows in the windowing system, with Sirens added providers.
        /// </summary>
        private void Draw()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(WindowingSystem));
            }

            if (!this.AnyWindowsOpen())
            {
                return;
            }

            this.windowSystem.Draw();
            this.Clipboard.UseKeyboardShortcuts();
        }

        /// <summary>
        /// Sets the config window to use for <see cref="UiBuilder.OpenConfigUi"/>.
        /// </summary>
        /// <param name="window">The window to use as the config window.</param>
        public void SetConfigWindow(Window window)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(WindowingSystem));
            }

            if (this.ConfigWindow == null)
            {
                SharedServices.UiBuilder.OpenConfigUi += this.ToggleConfigWindow;
            }
            this.ConfigWindow = window;
        }

        /// <summary>
        /// Unset the config window for <see cref="UiBuilder.OpenConfigUi"/>.
        /// </summary>
        public void UnsetConfigWindow()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(WindowingSystem));
            }

            if (this.ConfigWindow != null)
            {
                SharedServices.UiBuilder.OpenConfigUi -= this.ToggleConfigWindow;
                this.ConfigWindow = null;
            }
        }

        /// <summary>
        ///Gets all windows in the windowing system.
        /// </summary>
        /// <returns>An array of all windows in the windowing system.</returns>
        public IReadOnlyList<Window> Windows => this.windowSystem.Windows;

        /// <summary>
        /// Gets a window by type.
        /// </summary>
        /// <typeparam name="T">The type of window to get.</typeparam>
        /// <returns>The first window with the specified type in the windowing system, or null if it does not exist.</returns>
        public Window? GetWindow<T>() where T : Window
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(WindowingSystem));
            }

            return this.Windows.FirstOrDefault(window => window is T);
        }

        /// <summary>
        /// Tries to get a window by type.
        /// </summary>
        /// <typeparam name="T">The type of window to get.</typeparam>
        /// <param name="windowOut">The window with the specified type, or null if it does not exist.</param>
        public bool TryGetWindow<T>(out Window windowOut) where T : Window
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(WindowingSystem));
            }

            windowOut = this.GetWindow<T>()!;
            return windowOut != null;
        }

        /// <summary>
        /// Adds a window to the windowing system.
        /// </summary>
        /// <remarks>
        /// If multiple windows are added as config windows, the last one added will be used.
        /// </remarks>
        /// <param name="window">The window to add.</param>
        /// <param name="isConfigWindow">Whether or not the window is the config window.</param>
        public void AddWindow(Window window, bool isConfigWindow = false)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(WindowingSystem));
            }

            this.windowSystem.AddWindow(window);

            if (isConfigWindow)
            {
                this.SetConfigWindow(window);
            }

            SirenLog.Debug($"Added window {window} to windowing system.");
        }

        /// <summary>
        /// Adds all passed windows to the windowing system.
        /// </summary>
        /// <remarks>
        ///If multiple windows are added as config windows, the last one added will be used.
        /// </remarks>
        /// <params name="windows">The windows to add.</params>
        public void AddWindows(Dictionary<Window, bool> windows)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(WindowingSystem));
            }

            foreach (var (window, isConfigWindow) in windows)
            {
                this.AddWindow(window, isConfigWindow);
            }
        }

        /// <summary>
        /// Removes a window from the windowing system and disposes of it if it implements <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="window">The window to remove.</param>
        public void RemoveWindow(Window window)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(WindowingSystem));
            }

            if (this.ConfigWindow == window)
            {
                this.UnsetConfigWindow();
            }

            if (window is IDisposable disposable)
            {
                disposable.Dispose();
                SirenLog.Verbose($"Disposed of {window.WindowName} from windowing system.");
            }

            SirenLog.Debug($"Removed {window.WindowName} from windowing system.");
            this.windowSystem.RemoveWindow(window);
        }

        /// <summary>
        ///Checks to see if any windows are open.
        /// </summary>
        /// <returns>True if any windows are open, false otherwise.</returns>
        internal bool AnyWindowsOpen()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(WindowingSystem));
            }

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
        /// Toggles the open state of the configuration window.
        /// </summary>
        internal void ToggleConfigWindow()
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(WindowingSystem));
            }

            if (this.ConfigWindow == null)
            {
                return;
            }
            this.ConfigWindow.IsOpen = !this.ConfigWindow.IsOpen;
        }
    }
}
