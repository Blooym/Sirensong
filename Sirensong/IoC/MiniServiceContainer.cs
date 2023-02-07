using System;
using System.Collections.Generic;
using System.Linq;

namespace Sirensong.IoC
{
    /// <summary>
    /// A slimmed down singleton-only service container.
    /// </summary>
    /// <remarks>
    /// Automatically handles the disposal of services that implement <see cref="IDisposable"/>.
    /// You should use <see cref="RemoveService{T}"/> to remove services instead of trying to dispose of them yourself.
    /// </remarks>
    public sealed class MiniServiceContainer : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// The services held by the <see cref="MiniServiceContainer"/>.
        /// </summary>
        private static readonly Lazy<HashSet<object>> ServiceContainer = new(() => new HashSet<object>(), true);

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
                            SirenLog.Debug($"Disposed of SERVICE: {service.GetType().FullName}.");
                        }
                    }
                }
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Checks if a service exists in the service container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>True if the service exists, otherwise false.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public bool ServiceExists<T>() where T : class
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(MiniServiceContainer));
            }

            lock (ServiceContainerLock)
            {
                return ServiceContainer.Value.Any(x => x is T);
            }
        }

        /// <summary>
        /// Gets a service from the service container if it exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The service if it exists, otherwise null.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public T? GetService<T>() where T : class
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(MiniServiceContainer));
            }

            lock (ServiceContainerLock)
            {
                return ServiceContainer.Value.FirstOrDefault(x => x is T) as T;
            }
        }

        /// <summary>
        /// Creates a service in the service container.
        /// </summary>
        /// <remarks>
        /// The service will be disposed of when the <see cref="MiniServiceContainer"/> is disposed of.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void CreateService<T>() where T : class
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(MiniServiceContainer));
            }

            lock (ServiceContainerLock)
            {
                if (this.ServiceExists<T>())
                {
                    throw new InvalidOperationException($"Service of type {typeof(T).FullName} already exists.");
                }

                if (Activator.CreateInstance(typeof(T), true) is not T service)
                {
                    throw new InvalidOperationException($"Could not create service of type {typeof(T).FullName}.");
                }

                SirenLog.Debug($"Added SERVICE to container: {service.GetType().FullName}.");
                ServiceContainer.Value.Add(service);
            }
        }

        /// <summary>
        /// Gets a service from the service container if it exists, otherwise creates it.
        /// </summary>
        /// <remarks>
        /// The service will be disposed of when the <see cref="MiniServiceContainer"/> is disposed of.
        /// </remarks>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <returns>The service that was created or found.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
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

                this.CreateService<T>();
                return this.GetService<T>()!;
            }
        }

        /// <summary>
        /// Removes a service from the service container.
        /// </summary>
        /// <remarks>
        /// The service will be disposed of if it implements <see cref="IDisposable"/>.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <returns>True if removal was successful, otherwise false.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public bool RemoveService<T>() where T : class
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(MiniServiceContainer));
            }

            lock (ServiceContainerLock)
            {
                var service = this.GetService<T>();
                if (service is not null)
                {
                    ServiceContainer.Value.Remove(service);
                    if (service is IDisposable disposable)
                    {
                        disposable.Dispose();
                        SirenLog.Debug($"Disposed of SERVICE: {service.GetType().FullName}.");
                    }
                    SirenLog.Debug($"Removed SERVICE from container: {service.GetType().FullName}.");
                    return true;
                }
                return false;
            }
        }
    }
}