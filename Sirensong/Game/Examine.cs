using System;
using System.Runtime.InteropServices;
using Dalamud.Game.ClientState.Objects.Types;
using Framework = FFXIVClientStructs.FFXIV.Client.System.Framework.Framework;

// From XivCommon, licensed under the MIT license
// https://git.anna.lgbt/ascclemens/XivCommon/src/branch/main/LICENCE
// https://git.anna.lgbt/ascclemens/XivCommon/src/branch/main/XivCommon/Functions/Examine.cs

namespace Sirensong.Game
{
    /// <summary>
    ///     Class containing examine functions
    /// </summary>
    public static class Examine
    {
        private static class Signatures
        {
            internal const string RequestCharacterInfo = "40 53 48 83 EC 40 48 8B D9 48 8B 49 10 48 8B 01 FF 90 ?? ?? ?? ?? BA";
        }

        private delegate long RequestCharInfoDelegate(IntPtr ptr);

        private static RequestCharInfoDelegate invoke = null!;

        /// <summary>
        ///     If this is true, the Examine functions will have been disabled.
        /// </summary>
        public static bool Disabled { get; private set; }

        /// <summary>
        ///     Initializes the RequestCharInfo delegate.
        /// </summary>
        /// <returns>True if the delegate was initialized successfully, false otherwise</returns>
        private static bool Initialize()
        {
            try
            {
                if (Disabled)
                {
                    return false;
                }

                if (invoke != null)
                {
                    return true;
                }

                var foundExamine = SharedServices.SigScanner.TryScanText(Signatures.RequestCharacterInfo, out var rciData);
                if (!foundExamine || rciData == IntPtr.Zero)
                {
                    SirenLog.IWarning("Could not find signature for RequestCharacterInfo, examine functions will be disabled.");
                    Disabled = true;
                }
                invoke = Marshal.GetDelegateForFunctionPointer<RequestCharInfoDelegate>(rciData);
                return true;
            }
            catch (Exception e)
            {
                SirenLog.IWarning($"Failed to initialize Examine functions, they will be disabled. Error: {e}");
                Disabled = true;
                return false;
            }
        }

        /// <summary>
        ///     Opens the Examine window for the specified object.
        /// </summary>
        /// <param name="obj">Object to open window for</param>
        /// <exception cref="InvalidOperationException">If the signature for this function could not be found</exception>
        public static void OpenExamineWindow(GameObject obj) => OpenExamineWindow(obj.ObjectId);

        /// <summary>
        ///     Opens the Examine window for the object with the specified ID.
        /// </summary>
        /// <param name="objectId">Object ID to open window for</param>
        /// <exception cref="InvalidOperationException">If the signature for this function could not be found</exception>

        public static unsafe void OpenExamineWindow(uint objectId)
        {
            if (!Initialize())
            {
                return;
            }

            var agentModule = (IntPtr)Framework.Instance()->GetUiModule()->GetAgentModule();
            var rciData = Marshal.ReadIntPtr(agentModule + 0x1A8);

            var raw = (uint*)rciData;
            *(raw + 10) = objectId;
            *(raw + 11) = objectId;
            *(raw + 12) = objectId;
            *(raw + 13) = 0xE0000000;
            *(raw + 301) = 0;

            invoke(rciData);
        }
    }
}
