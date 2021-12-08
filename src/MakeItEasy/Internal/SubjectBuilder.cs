namespace MakeItEasy.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal class SubjectBuilder
    {
        public static T BuildSubject<T>(Type[] argumentTypes, object?[] argumentValues, Type[] collaboratorTypes, out object[] collaboratorValues)
        {
            var argumentExceptions = new List<ArgumentCreationException>();
            var constructorExceptions = new List<ConstructorFailedException>();

            foreach (var candidateConstructor in GetAllConstructors(typeof(T)).Select(c => SubjectConstructor<T>.TryCreateFrom(c, argumentTypes, collaboratorTypes)).Where(c => c is not null))
            {
                collaboratorValues = new object[collaboratorTypes.Length];
                try
                {
                    return candidateConstructor!.Build(argumentValues, collaboratorValues);
                }
                catch (ArgumentCreationException e)
                {
                    argumentExceptions.Add(e);
                }
                catch (ConstructorFailedException e)
                {
                    constructorExceptions.Add(e);
                }
            }

            if (argumentExceptions.Count == 0 && constructorExceptions.Count == 0)
            {
                var requiredTypes = argumentTypes.Concat(collaboratorTypes).ToList();
                throw new CreationException(ExceptionMessages.NoAccessibleConstructor(typeof(T), requiredTypes));
            }

            var message = new StringBuilder()
                .AppendLine(ExceptionMessages.UnableToCreateSubject(typeof(T)));

            if (argumentExceptions.Count > 0)
            {
                message
                    .AppendLine()
                    .AppendLine("  " + ExceptionMessages.UnableToCreateArgument());
                foreach (var exception in argumentExceptions)
                {
                    message
                        .Append("    (")
                        .Append(string.Join(", ", exception.Constructor.GetParameters().Select((p, i) => i == exception.ParameterIndex ? "*" + p.ParameterType : p.ParameterType.ToString())))
                        .AppendLine(")");
                }
            }

            if (constructorExceptions.Count > 0)
            {
                message
                    .AppendLine()
                    .AppendLine("  " + ExceptionMessages.ConstructorFailed());
                foreach (var exception in constructorExceptions)
                {
                    message
                        .Append("    (")
                        .Append(string.Join(", ", exception.Constructor.GetParameters().Select(p => p.ParameterType.ToString())))
                        .AppendLine(")")
                        .AppendLine("      " + ExceptionMessages.ExceptionType(exception.InnerException!))
                        .AppendLine("      " + ExceptionMessages.ExceptionMessage(exception.InnerException!));
                }
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
