using System;

namespace Sirensong.IoC.Internal
{
    /// <summary>
    /// Marks a class as a service that can be injected into via the <see cref="SirenServiceContainer" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal sealed class SirenServiceClassAttribute : Attribute
    {

    }
}