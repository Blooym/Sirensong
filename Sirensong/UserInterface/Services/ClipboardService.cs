using System;
using ImGuiNET;
using Sirensong.IoC.Internal;

namespace Sirensong.UserInterface.Services
{
    /// <summary>
    ///     Wrapper built on top of ImGui's clipboard functions that allow for finer control over the clipboard and clipboard events.
    /// </summary>
    [SirenServiceClass]
    public sealed class ClipboardService
    {
        /// <summary>
        ///     Creates a new instance of the <see cref="ClipboardService"/> class.
        /// </summary>
        internal ClipboardService() { }

        /// <summary>
        ///     How long should the clipboard be considered copied or pasted for?
        /// </summary>
        /// <remarks>
        ///     Default value: 1000ms
        /// </remarks>
        public int Timeout { get; set; } = 1250;

        /// <summary>
        ///     The last time the clipboard was copied using the <see cref="Copy"/> method.
        /// </summary>
        public DateTime LastCopyTime { get; private set; }

        /// <summary>
        ///     The last time the clipboard was pasted to using the <see cref="Paste"/> method.
        /// </summary>
        public DateTime LastPasteTime { get; private set; }

        /// <summary>
        ///     Fired when the clipboard is copied using the Clipboard <see cref="Copy"/> method.
        /// </summary>
        public event Action<object, string> Copied = null!;

        /// <summary>
        ///     Fired when the clipboard is pasted from using the Clipboard <see cref="Paste"/> method.
        /// </summary>
        public event Action<object, string> Pasted = null!;

        /// <summary>
        ///     Was the clipboard copied within the timeout?
        /// </summary>
        public bool WasCopied => this.LastCopyTime.AddMilliseconds(this.Timeout) > DateTime.Now;

        /// <summary>
        ///     Was the clipboard pasted within the timeout?
        /// </summary>
        public bool WasPasted => this.LastPasteTime.AddMilliseconds(this.Timeout) > DateTime.Now;

        /// <summary>
        ///     Copies text to the clipboard.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="silent">If true, suppresses any events from being fired, does not log anything and does not update the <see cref="LastCopyTime"/> property.</param>
        public void Copy(string text, bool silent = false)
        {
            if (text == string.Empty)
            {
                return;
            }

            if (!silent)
            {
                SirenLog.IVerbose($"Copying text to clipboard: {text}");
                this.LastCopyTime = DateTime.Now;
                this.Copied?.Invoke(this, text);
            }

            ImGui.SetClipboardText(text);
        }

        /// <summary>
        ///     Pastes text from the clipboard.
        /// </summary>
        /// <param name="silent">If true, suppresses any events from being fired, does not log anything and does not update the <see cref="LastPasteTime"/> property.</param>
        /// <returns></returns>
        public string Paste(bool silent = false)
        {
            var text = ImGui.GetClipboardText();

            if (!silent && text != string.Empty)
            {
                SirenLog.IVerbose($"Pasting text from clipboard: {text}");
                this.LastPasteTime = DateTime.Now;
                this.Pasted?.Invoke(this, text);
            }

            return text;
        }

        /// <summary>
        ///     Binds typical keyboard shortcuts for copy, cut, and paste to the clipboard Copy and Paste methods.
        /// </summary>
        /// <remarks>
        ///    This method should be called in at the end of a window's draw method.
        /// </remarks>
        public void UseKeyboardShortcuts()
        {
            // If not inside of an Input or a window is not focused, don't do anything.
            if (!ImGui.IsAnyItemActive())

            {
                return;
            }

            // Override copy shortcut.
            if (ImGui.IsKeyPressed(ImGuiKey.C) && ImGui.GetIO().KeyCtrl)
            {
                this.Copy(ImGui.GetClipboardText());
            }

            // Override cut shortcut.
            if (ImGui.IsKeyPressed(ImGuiKey.X) && ImGui.GetIO().KeyCtrl)
            {
                this.Copy(ImGui.GetClipboardText());
            }

            // Override paste shortcut.
            if (ImGui.IsKeyPressed(ImGuiKey.V) && ImGui.GetIO().KeyCtrl)
            {
                this.Paste();
            }
        }
    }
}
