namespace MakeItEasy.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FakeItEasy.Sdk;

    internal class SubjectConstructor<T>
    {
        private const int UNMAPPED = -1;
        private readonly ConstructorInfo constructor;
        private readonly int[] suppliedArgumentToParameterMap;
        private Type[] collaboratorTypes;
        private int[] collaboratorToParameterMap;

        public SubjectConstructor(
            ConstructorInfo constructor,
            int[] suppliedArgumentToParameterMap,
            Type[] collaboratorTypes,
            int[] collaboratorToParameterMap)
        {
            this.constructor = constructor;
            this.suppliedArgumentToParameterMap = suppliedArgumentToParameterMap;
            this.collaboratorTypes = collaboratorTypes;
            this.collaboratorToParameterMap = collaboratorToParameterMap;
        }

        public static SubjectConstructor<T>? TryCreateFrom(ConstructorInfo constructor, Type[] suppliedArgumentTypes, Type[] collaboratorTypes)
        {
            var parameters = constructor.GetParameters().ToList();

            var suppliedArgumentsToParameterMap = MapTypesToParameters(parameters, suppliedArgumentTypes);
            var collaboratorToParameterMap = MapTypesToParameters(parameters, collaboratorTypes);

            return suppliedArgumentsToParameterMap.Concat(collaboratorToParameterMap).Any(b => b == UNMAPPED)
                ? null
                : new SubjectConstructor<T>(constructor, suppliedArgumentsToParameterMap, collaboratorTypes, collaboratorToParameterMap);

            static int[] MapTypesToParameters(IList<ParameterInfo> parameters, Type[] requiredTypes)
            {
                var parameterIndexMap = new int[requiredTypes.Length];
                for (int i = 0; i < parameterIndexMap.Length; ++i)
                {
                    parameterIndexMap[i] = UNMAPPED;
                }

                for (int parameterIndex = 0; parameterIndex < parameters.Count; ++parameterIndex)
                {
                    ParameterInfo parameter = parameters[parameterIndex];

                    for (int typeIndex = 0; typeIndex < requiredTypes.Length; ++typeIndex)
                    {
                        if (parameterIndexMap[typeIndex] != UNMAPPED)
                        {
                            continue;
                        }

                        if (parameter.ParameterType.IsAssignableFrom(requiredTypes[typeIndex]))
                        {
                            parameterIndexMap[typeIndex] = parameter.Position;
                            parameters.RemoveAt(parameterIndex);

                            // We've removed a parameter from the list, so the next parameter is in the same space as the previous one.
                            // Reduce the index so we check it on the next iteration
                            --parameterIndex;
                            break;
                        }
                    }
                }

                return parameterIndexMap;
            }
        }

        internal T Build(object?[] suppliedArgumentValues, object[] collaborators)
        {
            var arguments = new object?[this.constructor.GetParameters().Length];
            var unfilledParameterIndices = new HashSet<int>(Enumerable.Range(0, arguments.Length));

            this.FillSuppliedArguments(suppliedArgumentValues, arguments, unfilledParameterIndices);
            this.FillCollaborators(collaborators, arguments, unfilledParameterIndices);
            this.FillRemainingArguments(arguments, unfilledParameterIndices);
            return (T)this.constructor.Invoke(arguments);
        }

        private void FillSuppliedArguments(object?[] suppliedArgumentValues, object?[] arguments, HashSet<int> unfilledParameterIndices)
        {
            for (int i = 0; i < this.suppliedArgumentToParameterMap.Length; ++i)
            {
                int parameterIndex = this.suppliedArgumentToParameterMap[i];
                arguments[parameterIndex] = suppliedArgumentValues[i];
                unfilledParameterIndices.Remove(parameterIndex);
            }
        }

        private void FillCollaborators(object[] collaborators, object?[] arguments, ISet<int> unfilledParameterIndices)
        {
            for (int i = 0; i < this.collaboratorToParameterMap.Length; ++i)
            {
                int parameterIndex = this.collaboratorToParameterMap[i];
                try
                {
                    arguments[parameterIndex] = collaborators[i] = Create.Fake(this.collaboratorTypes[i]);
                }
                catch (Exception e)
                {
                    throw new CreationException(ExceptionMessages.FailedToCreateCollaborator(typeof(T), this.collaboratorTypes[i]), e);
                }

                unfilledParameterIndices.Remove(parameterIndex);
            }
        }

        private void FillRemainingArguments(object?[] arguments, HashSet<int> unfilledParameterIndices)
        {
            foreach (var parameterIndex in unfilledParameterIndices)
            {
                Type parameterType = this.constructor.GetParameters()[parameterIndex].ParameterType;
                try
                {
                    arguments[parameterIndex] = Create.Dummy(parameterType);
                }
                catch
                {
                    throw new CreationException(ExceptionMessages.FailedToCreateConstructorArgument(typeof(T), parameterType));
                }
            }
        }
    }
}
