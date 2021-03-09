#pragma warning disable CA1032 // Add the following Exception constructor
#pragma warning disable CA1064 // Exceptions should be public
namespace MakeItEasy.Internal
{
    using System;
    using System.Reflection;

    /// <summary>
    /// An exception that is thrown when there was an error calling a subject's constructor.
    /// </summary>
    internal class ConstructorFailedException : Exception
    {
        public ConstructorFailedException(ConstructorInfo constructor, Exception? innerException)
            : base("Constructor " + constructor + " failed", innerException)
        {
            this.Constructor = constructor;
        }

        public ConstructorInfo Constructor { get; }
    }
}
#pragma warning restore CA1064 // Exceptions should be public
#pragma warning restore CA1032 // Add the following Exception constructor
