namespace MakeItEasy.Internal
{
    using System;
    using System.Collections.Generic;

    internal static class ExceptionMessages
    {
        public static string NoAccessibleConstructor(Type subjectType, IList<Type> requiredTypes) =>
            requiredTypes.Count switch
            {
                0 => $"No accessible constructor for type {subjectType}",
                1 => $"No accessible constructor for type {subjectType} contains a parameter of type {requiredTypes[0]}",
                _ => $"No accessible constructor for type {subjectType} contains all of the following parameter types {string.Join(", ", requiredTypes)}",
            };
    }
}
