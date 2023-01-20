using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirensong.Extensions;

namespace Sirensong.IoC.Internal
{
    /// <summary>
    ///     Handles the creation and management of services.
    /// </summary>
    internal sealed class ServiceContainer : IServiceProvider, IDisposable
    {
        /// <summary>
        ///     Creates a new instance of the <see cref="ServiceContainer"/> class.
        /// </summary>
        internal ServiceContainer() { }

        /// <summary>
        ///     All services that have been added to the <see cref="ServiceContainer"/>.
        /// </summary>
        private readonly Lazy<List<object>> services = new(() => new List<object>(), true);

        /// <summary>
        ///     Whether or not the <see cref="ServiceContainer"/> has been disposed of.
        /// </summary>
        internal bool Disposed { get; private set; }

        /// <summary>
        ///     Disposes of the <see cref="ServiceContainer"/> and all services contained within it that implement <see cref="IDisposable"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the caller is not the Sirensong assembly.</exception>
        public void Dispose()
        {
            if (!Assembly.GetCallingAssembly().IsExecutingAssembly())
            {
                throw new InvalidOperationException("The service container can only be disposed of by the Sirensong assembly.");
            }

            if (this.Disposed)
            {
                return;
            }

            // Dispose of contained IDisposable services.
            foreach (var service in this.services.Value)
            {
                if (service is IDisposable disposableService)
                {
                    SirenLog.IVerbose($"Disposing of service {disposableService.GetType().FullName}.");
                    disposableService.Dispose();
                }
            }

            SirenLog.IVerbose("Disposed of service container.");
            this.Disposed = true;
        }

        /// <summary>
        ///     A boolean value indicating whether or not the caller is allowed to create the given service.
        /// </summary>
        /// <param name="type">The type of the service to create.</param>
        /// <returns>True if the caller is allowed to create the service, false otherwise.</returns>
        private static bool CanInjectService(Type type)
        {
            var attribute = type.GetCustomAttribute<SirenServiceClassAttribute>();
            if (attribute == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///    Creates a new instance of the given type and adds it to the service container.
        /// </summary>
        /// <param name="type">The type of the service to add.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the service container has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the caller is not allowed to create the service.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
        internal object CreateService(Type type)
        {
            if (this.Disposed)
            {
                throw new ObjectDisposedException(nameof(ServiceContainer));
            }

            if (!CanInjectService(type))
            {
                throw new InvalidOperationException($"Cannot create service of type {type.FullName} from other assemblies.");
            }

            var service = Activator.CreateInstance(type, true);
            if (service == null)
            {
                throw new ArgumentNullException(service?.GetType().FullName);
            }

            this.services.Value.Add(service);
            SirenLog.IVerbose($"Created service of type {service.GetType().FullName}.");
            return service;
        }

        /// <inheritdoc cref="CreateService(Type)"/>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        internal T CreateService<T>() => (T)this.CreateService(typeof(T));

        /// <summary>
        ///     Gets a service from the service container.
        /// </summary>
        /// <param name="type">The type of the service to get.</param>
        /// <returns>The service, or null if it was not found.</returns>
        /// <exception cref="ObjectDisposedException">Thrown if the service container has been disposed.</exception>
        public object? GetService(Type type)
        {
            if (this.Disposed)
            {
                throw new ObjectDisposedException(nameof(ServiceContainer));
            }
            return this.services.Value.FirstOrDefault(service => service.GetType() == type);
        }

        /// <inheritdoc cref="GetService(Type)"/>
        /// <typeparam name="T">The type of the service to get.</typeparam>
        public T? GetService<T>() => (T?)this.GetService(typeof(T));

        /// <summary>
        ///     Gets a service from the service container, or throws an exception if it was not found.
        /// </summary>
        /// <param name="type">The type of the service to get.</param>
        /// <returns>The service if it was found.</returns>
        /// <exception cref="ObjectDisposedException">Thrown if the service container has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the service was not found.</exception>
        internal object MustGetService(Type type)
        {
            if (this.Disposed)
            {
                throw new ObjectDisposedException(nameof(ServiceContainer));
            }

            var service = this.GetService(type);
            if (service == null)
            {
                throw new InvalidOperationException($"Service of type {type.FullName} was not found.");
            }
            return service;
        }

        /// <inheritdoc cref="MustGetService(Type)"/>
        /// <typeparam name="T">The type of the service to get.</typeparam>
        internal T MustGetService<T>() => (T)this.MustGetService(typeof(T));

        /// <summary>
        ///    Gets or creates a service from the service container.
        /// </summary>
        /// <param name="type">The type of the service to get.</param>
        /// <returns>The service.</returns>
        /// <exception cref="ObjectDisposedException">Thrown if the service container has been disposed.</exception>
        internal object GetOrCreateService(Type type) => this.GetService(type) ?? this.CreateService(type);

        /// <inheritdoc cref="GetOrCreateService(Type)"/>
        /// <typeparam name="T">The type of the service to get.</typeparam>
        internal T GetOrCreateService<T>() => (T)this.GetOrCreateService(typeof(T));

        /// <summary>
        ///     Removes a service from the service container and disposes it if it implements <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="service">The (this) object to remove.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the service container has been disposed.</exception>
        internal void RemoveService(object service)
        {
            if (this.Disposed)
            {
                throw new ObjectDisposedException(nameof(ServiceContainer));
            }

            if (service is IDisposable disposable)
            {
                disposable.Dispose();
            }

            this.services.Value.Remove(service);
        }

        /// <summary>
        ///     Injects services into a class.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This method will inject services into any static properties of the class that are marked with the <see cref="SirenServiceAttribute"/> attribute
        ///         and are internally marked with <see cref="SirenServiceClassAttribute"/> with "Externally Accessible" set to true.
        ///     </para>
        /// </remarks>
        /// <typeparam name="T">The type of the class to inject services into.</typeparam>
        /// <param name="assembly">The assembly to inject services from.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the service container has been disposed.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the class to inject services into is null.</exception>
        internal void InjectServices<T>(Assembly assembly)
        {
            if (this.Disposed)
            {
                throw new ObjectDisposedException(nameof(ServiceContainer));
            }

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<SirenServiceAttribute>();
                if (attribute == null)
                {
                    continue;
                }

                if (!CanInjectService(property.PropertyType))
                {
                    continue;
                }

                var service = this.GetOrCreateService(property.PropertyType);
                property.SetValue(null, service);
                SirenLog.IVerbose($"Injected service {property.PropertyType.FullName} into {property.Name}.");
            }
        }

        /// <summary>
        ///     Returns all services in the service container.
        /// </summary>
        /// <returns>All services in the service container.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        internal IEnumerable<object> GetServices()
        {
            if (this.Disposed)
            {
                throw new ObjectDisposedException(nameof(ServiceContainer));
            }
            return this.services.Value;
        }
    }
}
