using System;

namespace Sirensong.IoC.Internal
{
    /// <summary>
    ///     Marks a class as a service that can be injected into via the <see cref="ServiceContainer" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class SirenServiceClassAttribute : Attribute
    {
    }
}