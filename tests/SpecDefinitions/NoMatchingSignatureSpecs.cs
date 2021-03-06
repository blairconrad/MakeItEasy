namespace MakeItEasy.Specs
{
    using System;

    using FakeItEasy;
    using FluentAssertions;
    using MakeItEasy.Specs.TestTypes;
    using Xbehave;
    using Xunit;

    public static class NoMatchingSignatureSpecs
    {
        [Scenario]
        public static void ClassWithNoPublicConstructorCannotBeCreated(
            Exception exception)
        {
            "Given a class that has no public constructor"
                .See<NoPublicConstructorClass>();

            "When I make an object of that type"
                .x(() => exception = Record.Exception(() => Make.A<NoPublicConstructorClass>().From()));

            "Then an exception is thrown"
                .x(() => exception.Should().BeOfType<CreationException>());

            "And the exception indicates why the creation failed"
                .x(() => exception.Message.Should().Be("No accessible constructor for type MakeItEasy.Specs.TestTypes.NoPublicConstructorClass"));
        }

        [Scenario]
        public static void ClassWithConstructorWhoseArgumentTypeDoesNotMatchCannotBeConstructed(
            Exception exception)
        {
            "Given a class that has only one constructor"
                .See<OneArgumentClass>();

            "When I make an object of that type and supply a non-matching constructor argument"
                .x(() => exception = Record.Exception(() =>
                    Make.A<OneArgumentClass>().From("hippo")));

            "Then an exception is thrown"
                .x(() => exception.Should().BeOfType<CreationException>());

            "And the exception indicates why the creation failed"
                .x(() => exception.Message.Should().Be("No accessible constructor for type MakeItEasy.Specs.TestTypes.OneArgumentClass contains a parameter of type System.String"));
        }

        [Scenario]
        public static void ClassWithOnlyParameterlessConstructorCannotSupplyCollaborator(
            Exception exception)
        {
            "Given a class that has only a parameterless constructor"
                .See<NoCollaboratorsClass>();

            "When I make an object of that type and ask for a collaborator back"
                .x(() => exception = Record.Exception(() =>
                    Make.A<NoCollaboratorsClass>()
                        .From(out ICanCollaborate collaborator)));

            "Then an exception is thrown"
                .x(() => exception.Should().BeOfType<CreationException>());

            "And the exception indicates why the creation failed"
                .x(() => exception.Message.Should().Be("No accessible constructor for type MakeItEasy.Specs.TestTypes.NoCollaboratorsClass contains a parameter of type MakeItEasy.Specs.TestTypes.ICanCollaborate"));
        }

        [Scenario]
        public static void ClassWithConstructorsThatTakeArgumentOrCollaboratorCannotBeCreatedFromArgumentAndSupplyCollaborator(
            Exception exception)
        {
            "Given a class that has one constructor taking an argument and one a collaborator"
                .See<OneCollaboratorOrOneArgumentClass>();

            "When I make an object of that type, supplying an argument and asking for a collaborator back"
                .x(() => exception = Record.Exception(() =>
                    Make.A<OneCollaboratorOrOneArgumentClass>()
                        .From(-4, out ICanCollaborate collaborator)));

            "Then an exception is thrown"
                .x(() => exception.Should().BeOfType<CreationException>());

            "And the exception indicates why the creation failed"
                .x(() => exception.Message.Should().Be("No accessible constructor for type MakeItEasy.Specs.TestTypes.OneCollaboratorOrOneArgumentClass contains all of the following parameter types System.Int32, MakeItEasy.Specs.TestTypes.ICanCollaborate"));
        }
    }
}