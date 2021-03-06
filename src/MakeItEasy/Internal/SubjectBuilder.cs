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
            throw new CreationException(ExceptionMessages.NoAccessibleConstructor(typeof(T), requiredTypes));
        }

        private static IEnumerable<ConstructorInfo> GetAllConstructors(Type type)
        {
            return type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length);
        }
    }
}