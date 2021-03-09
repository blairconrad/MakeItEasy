namespace MakeItEasy.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal class SubjectBuilder
    {
        public static T BuildSubject<T>(Type[] argumentTypes, object?[] argumentValues, Type[] collaboratorTypes, out object[] collaboratatorValues)
        {
            var exceptions = new List<ArgumentCreationException>();

            foreach (var candidateConstructor in GetAllConstructors(typeof(T)).Select(c => SubjectConstructor<T>.TryCreateFrom(c, argumentTypes, collaboratorTypes)).Where(c => c is not null))
            {
                collaboratatorValues = new object[collaboratorTypes.Length];
                try
                {
                    return candidateConstructor!.Build(argumentValues, collaboratatorValues);
                }
                catch (ArgumentCreationException e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Count == 0)
            {
                var requiredTypes = argumentTypes.Concat(collaboratorTypes).ToList();
                throw new CreationException(ExceptionMessages.NoAccessibleConstructor(typeof(T), requiredTypes));
            }

            var message = new StringBuilder()
                .AppendLine(ExceptionMessages.UnableToCreateSubject(typeof(T)))
                .AppendLine()
                .AppendLine("  " + ExceptionMessages.UnableToCreateArgument());
            foreach (var exception in exceptions)
            {
                message
                    .Append("    (")
                    .Append(string.Join(", ", exception.Constructor.GetParameters().Select((p, i) => i == exception.ParameterIndex ? "*" + p.ParameterType : p.ParameterType.ToString())))
                    .AppendLine(")");
            }

            throw new CreationException(message.ToString());
        }

        private static IEnumerable<ConstructorInfo> GetAllConstructors(Type type)
        {
            return type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length);
        }
    }
}