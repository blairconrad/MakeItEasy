namespace MakeItEasy.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class SubjectBuilder
    {
        public static T BuildSubject<T>(Type[] argumentTypes, object?[] argumentValues, Type[] collaboratorTypes, out object[] collaboratatorValues)
        {
            foreach (var candidateConstructor in GetAllConstructors(typeof(T)).Select(c => SubjectConstructor<T>.TryCreateFrom(c, argumentTypes, collaboratorTypes)).Where(c => c is not null))
            {
                collaboratatorValues = new object[collaboratorTypes.Length];
                return candidateConstructor!.Build(argumentValues, collaboratatorValues);
            }

            var requiredTypes = argumentTypes.Concat(collaboratorTypes).ToList();
            var message = requiredTypes.Count switch
            {
                0 => $"No accessible constructor for type {typeof(T)}",
                1 => $"No accessible constructor for type {typeof(T)} contains a parameter of type {requiredTypes[0]}",
                _ => $"No accessible constructor for type {typeof(T)} contains all of the following parameter types {string.Join(", ", requiredTypes)}",
            };

            throw new CreationException(message);
        }

        private static IEnumerable<ConstructorInfo> GetAllConstructors(Type type)
        {
            return type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length);
        }
    }
}