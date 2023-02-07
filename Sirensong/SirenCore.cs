using System;
using System.Reflection;
using Dalamud.Plugin;
using Sirensong.IoC.Internal;

namespace Sirensong
{
    /// <summary>
    /// Contains core methods for interacting with Sirensong.
    /// </summary>
    public static class SirenCore
    {

        /// <inheritdoc cref="SirenServiceContainer"/>
        internal static readonly SirenServiceContainer IoC = new();

        /// <summary>
        /// Whether or not Sirensong has been disposed.
        /// </summary>
        private static bool disposedValue;

        /// <summary>
        /// The initializing assembly.
        /// </summary>
        internal static Assembly InitializerAssembly { get; private set; } = null!;

        /// <summary>
        /// The name of the assembly/plugin that initialized Sirensong.
        /// </summary>
        internal static string InitializerName { get; private set; } = null!;

        /// <summary>
        /// Initializes the Sirensong library, using the provided <see cref="DalamudPluginInterface"/> to access Dalamud services.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     Initialize is required to be called before accessing any Sirensong services, as it is responsible for creating
        ///     Both a <see cref="SirenServiceContainer"/> and a <see cref="SharedServices"/> instance.
        /// </para>
        /// </remarks>
        /// <param name="pluginInterface">Your plugin's <see cref="DalamudPluginInterface"/>.</param>
        /// <param name="pluginName"></param>
        /// <exception cref="InvalidOperationException">Thrown if Sirensong has already been initialized.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if Sirensong has been disposed.</exception>
        public static void Initialize(DalamudPluginInterface pluginInterface, string pluginName)
        {
            // Set initializer information.
            InitializerAssembly = Assembly.GetCallingAssembly();
            InitializerName = pluginName;

            // Create Dalamud services.
            SharedServices.Initialize(pluginInterface);

            // Log initialization.
            SirenLog.Information($"Initialized Sirensong for {pluginName}.");
        }

        /// <summary>
        /// Disposes of Sirensong resources.
        /// </summary>
        public static void Dispose()
        {
            if (!disposedValue)
            {
                IoC.Dispose();

                SirenLog.Information($"Disposed of Sirensong for {InitializerName}.");

                disposedValue = true;
            }
        }

        /// <inheritdoc cref="SirenServiceContainer.InjectServices{T}"/>
        public static void InjectServices<T>() where T : class => IoC.InjectServices<T>();

        /// <inheritdoc cref="SirenServiceContainer.GetService{T}"/>
        public static T? GetService<T>() where T : class => IoC.GetService<T>();

        /// <inheritdoc cref="SirenServiceContainer.GetOrCreateService{T}"/>
        public static T GetOrCreateService<T>() where T : class => IoC.GetOrCreateService<T>();
    }
}
