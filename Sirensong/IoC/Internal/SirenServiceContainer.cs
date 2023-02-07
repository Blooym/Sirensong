using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sirensong.IoC.Internal
{
    /// <summary>
    /// Handles the creation and management of services for Sirensong.
    /// </summary>
    internal sealed class SirenServiceContainer : IServiceProvider, IDisposable
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SirenServiceContainer"/> class.
        /// </summary>
        internal SirenServiceContainer() { }

        /// <summary>
        /// The services held by the <see cref="SirenServiceContainer"/>.
        /// </summary>
        private readonly Lazy<List<object>> services = new(() => new List<object>(), true);

        /// <summary>
        /// Whether or not the <see cref="SirenServiceContainer"/> has been disposed of.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Disposes of the <see cref="SirenServiceContainer"/> and all services contained within it that implement <see cref="IDisposable"/>.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposedValue)
            {
                SirenLog.Verbose("Disposing of service container.");
                foreach (var service in this.services.Value)
                {
                    if (service is IDisposable disposableService)
                    {
                        SirenLog.Verbose($"Disposing of service {service.GetType().FullName}.");
                        disposableService.Dispose();
                    }
                }
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// A boolean value indicating if the given type is a valid service.
        /// </summary>
        private static bool IsValidService(Type type)
        {
            var attribute = type.GetCustomAttribute<SirenServiceClassAttribute>();
            if (attribute == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a new instance of the given service type and adds it to the service container if it is valid.
        /// </summary>
        /// <param name="type">The type of the service to add.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the service container has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the service type is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the service type already exists.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the service type does not have a parameterless constructor.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
        /// <returns>The newly created service.</returns>
        internal object CreateService(Type type)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(SirenServiceContainer));
            }

            if (!IsValidService(type))
            {
                throw new InvalidOperationException($"Cannot create service of type {type.FullName} because it is not a valid service.");
            }

            var existingService = this.GetService(type);
            if (existingService != null)
            {
                throw new InvalidOperationException($"Cannot create service of type {type.FullName} because it already exists.");
            }

            var service = Activator.CreateInstance(type, true);
            if (service == null)
            {
                throw new ArgumentNullException(service?.GetType().Name);
            }

            this.services.Value.Add(service);
            SirenLog.Verbose($"Service {service.GetType().FullName} created and added to service container.");
            return service;
        }

        /// <inheritdoc cref="CreateService(Type)"/>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        internal T CreateService<T>() where T : class => (T)this.CreateService(typeof(T));

        /// <summary>
        /// Gets a service from the service container.
        /// </summary>
        /// <param name="type">The type of the service to get.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the service container has been disposed.</exception>
        /// <returns>The service, or null if it was not found.</returns>
        public object? GetService(Type type)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(SirenServiceContainer));
            }
            return this.services.Value.FirstOrDefault(service => service.GetType() == type);
        }

        /// <inheritdoc cref="GetService(Type)"/>
        /// <typeparam name="T">The type of the service to get.</typeparam>
        public T? GetService<T>() where T : class => (T?)this.GetService(typeof(T));

        /// <summary>
        /// Gets or creates a service from the service container.
        /// </summary>
        /// <param name="type">The type of the service to get.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the service container has been disposed.</exception>
        /// <returns>The service.</returns>
        internal object GetOrCreateService(Type type) => this.GetService(type) ?? this.CreateService(type);

        /// <inheritdoc cref="GetOrCreateService(Type)"/>
        /// <typeparam name="T">The type of the service to get.</typeparam>
        internal T GetOrCreateService<T>() where T : class => (T)this.GetOrCreateService(typeof(T));

        /// <summary>
        /// Removes a service from the service container and disposes it if it implements <see cref="IDisposable"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if the service container has been disposed.</exception>
        /// <param name="service">The type of the service to remove.</param>
        internal void RemoveService(Type service)
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(SirenServiceContainer));
            }

            if (service is IDisposable disposable)
            {
                disposable.Dispose();
            }

            SirenLog.Verbose($"Service {service.FullName} removed from service container.");
            this.services.Value.Remove(service);
        }

        /// <inheritdoc cref="RemoveService(Type)"/>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        internal void RemoveService<T>() => this.RemoveService(typeof(T));

        /// <summary>
        /// Injects services into a class.
        /// </summary>
        /// <typeparam name="T">The type of the class to inject services into.</typeparam>
        /// <remarks>
        /// <para>
        ///  This method will inject services into any static properties of the class that are marked with the <see cref="SirenServiceAttribute"/> attribute
        ///  and are internally marked with <see cref="SirenServiceClassAttribute"/> .
        /// </para>
        /// <para>
        ///  If something goes wrong while injecting a service, an error will be thrown.
        /// </para>
        /// </remarks>
        /// <exception cref="ObjectDisposedException">Thrown if the service container has been disposed.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the class to inject services into is null.</exception>
        internal void InjectServices<T>() where T : class
        {
            if (this.disposedValue)
            {
                throw new ObjectDisposedException(nameof(SirenServiceContainer));
            }

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<SirenServiceAttribute>();
                if (attribute == null)
                {
                    continue;
                }

                if (!IsValidService(property.PropertyType))
                {
                    throw new InvalidOperationException($"Cannot inject service of type {property.PropertyType.FullName} into class {typeof(T).FullName} because it is not a valid service.");
                }

                var service = this.GetOrCreateService(property.PropertyType);
                property.SetValue(null, service);
                SirenLog.Verbose($"Injected service {service.GetType().FullName} into class {typeof(T).FullName}.");
            }
        }
    }
}
