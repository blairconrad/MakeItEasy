namespace MakeItEasy.Internal
{
    using System;
    using System.Collections.Generic;

    internal static class ExceptionMessages
    {
        public static string ConstructorFailed() =>
            "At least one constructor threw an exception. Constructors with the following signatures failed.";

        public static string ExceptionMessage(Exception exception) =>
            $"Message: {exception.Message}";

        public static string ExceptionType(Exception exception) =>
            $"Exception type: {exception.GetType()}";

        public static string FailedToCreateCollaborator(Type subjectType, Type collaboratorType) =>
            $"Unable to create {subjectType}. Failed to create Fake collaborator of type {collaboratorType}.";

        public static string NoAccessibleConstructor(Type subjectType, IList<Type> requiredTypes) =>
            requiredTypes.Count switch
            {
                0 => $"No accessible constructor for type {subjectType}",
                1 => $"No accessible constructor for type {subjectType} contains a parameter of type {requiredTypes[0]}",
                _ => $"No accessible constructor for type {subjectType} contains all of the following parameter types {string.Join(", ", requiredTypes)}",
            };

        public static string UnableToCreateSubject(Type subjectType) =>
            $"Unable to create {subjectType}.";

        public static string UnableToCreateArgument() =>
            "Failed to create at least one argument needed by a constructor. The following constructors were untried, with unresolvable types marked by *s.";
    }
}
