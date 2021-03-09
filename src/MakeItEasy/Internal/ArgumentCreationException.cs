#pragma warning disable CA1032 // Add the following Exception constructor
#pragma warning disable CA1064 // Exceptions should be public
namespace MakeItEasy.Internal
{
    using System;
    using System.Reflection;

    /// <summary>
    /// An exception that is thrown when there was an error creating an argument for a
    /// subject's exception.
    /// </summary>
    internal class ArgumentCreationException : Exception
    {
        public ArgumentCreationException(ConstructorInfo constructor, int parameterIndex)
        {
            this.Constructor = constructor;
            this.ParameterIndex = parameterIndex;
        }

        public int ParameterIndex { get; }

        public ConstructorInfo Constructor { get; }
    }
}
#pragma warning restore CA1064 // Exceptions should be public
#pragma warning restore CA1032 // Add the following Exception constructor
