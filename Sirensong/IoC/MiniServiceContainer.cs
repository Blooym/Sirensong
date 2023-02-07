using System;
using System.Collections.Generic;

namespace Sirensong.IoC
{
    /// <summary>
    /// The slimmed down singleton-only service container.
    /// </summary>
    public sealed class MiniServiceContainer : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// The services held by the <see cref="MiniServiceContainer"/>.
        /// </summary>
        private static readonly Lazy<List<object>> ServiceContainer = new(() => new List<object>(), true);

        /// <summary>
        /// The lock object for the <see cref="ServiceContainer"/>.
        /// </summary>
        private static readonly object ServiceContainerLock = new();

        /// <summary>
        /// Disposes of the <see cref="MiniServiceContainer"/> and all services contained within it that implement <see cref="IDisposable"/>.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                lock (ServiceContainerLock)
                {
                    foreach (var service in ServiceContainer.Value)
                    {
                        if (service is IDisposable disposableService)
                        {
                            disposableService.Dispose();
                        }
                    }
                }
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Gets a service from the service container if it exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The service if found, otherwise <see langword="null"/>.</returns>
        public T? GetService<T>() where T : class
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(MiniServiceContainer));
            }

            lock (ServiceContainerLock)
            {
                return ServiceContainer.Value.Find(x => x is T) as T;
            }
        }

        /// <summary>
        /// Checks if a service exists in the service container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>True if the service exists, otherwise false.</returns>
        public bool ServiceExists<T>() where T : class
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(MiniServiceContainer));
            }

            lock (ServiceContainerLock)
            {
                return ServiceContainer.Value.Find(x => x is T) is not null;
            }
        }


        /// <summary>
        /// Creates the service if it does not exist, returns the service either way.
        /// </summary>
        /// <remarks>
        /// If you do not dispose of the service yourself, it will be disposed of when the plugin is unloaded.
        /// </remarks>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <returns>The service that was created or found.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the service could not be created.</exception>
        public T GetOrCreateService<T>() where T : class
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(MiniServiceContainer));
            }

            lock (ServiceContainerLock)
            {
                var existingService = this.GetService<T>();
                if (existingService is not null)
                {
                    return existingService;
                }

                SirenLog.Debug($"Creating service: {typeof(T).FullName}");

                if (Activator.CreateInstance(typeof(T), true) is not T service)
                {
                    throw new InvalidOperationException($"Could not create service of type {typeof(T).FullName}.");
                }

                ServiceContainer.Value.Add(service);
                SirenLog.Debug($"Service created: {service.GetType().Name}");
                return service;
            }
        }

        /// <summary>
        /// Removes a service from the service container if it exists and disposes of it if it implements <see cref="IDisposable"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>True if removal was successful, otherwise false.</returns>
        public bool RemoveService<T>() where T : class
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(MiniServiceContainer));
            }

            lock (ServiceContainerLock)
            {
                var service = ServiceContainer.Value.Find(x => x is T);
                if (service is not null)
                {
                    ServiceContainer.Value.Remove(service);

                    if (service is IDisposable disposable)
                    {
                        SirenLog.Debug($"Disposing service: {service.GetType().Name}");
                        disposable.Dispose();
                    }

                    SirenLog.Debug($"Unregistered service: {service.GetType().Name}");
                    return true;
                }
                return false;
            }
        }
    }
}