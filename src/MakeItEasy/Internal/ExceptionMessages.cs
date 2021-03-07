namespace MakeItEasy.Internal
{
    using System;
    using System.Collections.Generic;

    internal static class ExceptionMessages
    {
        public static string FailedToCreateCollaborator(Type subjectType, Type collaboratorType) =>
            $"Unable to create {subjectType}. Failed to create Fake collaborator of type {collaboratorType}.";

        public static string FailedToCreateConstructorArgument(Type subjectType, Type parameterType) =>
            $"Unable to create {subjectType}. Failed to create Dummy argument of type {parameterType}.";

        public static string NoAccessibleConstructor(Type subjectType, IList<Type> requiredTypes) =>
            requiredTypes.Count switch
            {
                0 => $"No accessible constructor for type {subjectType}",
                1 => $"No accessible constructor for type {subjectType} contains a parameter of type {requiredTypes[0]}",
                _ => $"No accessible constructor for type {subjectType} contains all of the following parameter types {string.Join(", ", requiredTypes)}",
            };
    }
}
