namespace MakeItEasy.Specs
{
    using System;

    using FluentAssertions;
    using MakeItEasy.Specs.TestTypes;
    using Xbehave;
    using Xunit;

    public static class MakeWithNoArgsOrCollaboratorsSpecs
    {
        [Scenario]
        public static void ClassWithOnlyParameterlessConstructorShouldBeCreated(
            NoCollaboratorsClass subject)
        {
            "Given a class that has only a parameterless constructor"
                .See<NoCollaboratorsClass>();

            "When I make an object of that type"
                .x(() => subject = Make.A<NoCollaboratorsClass>().From());

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<NoCollaboratorsClass>());
        }

        [Scenario]
        public static void ClassWithOneOptionalCollaboratorShouldBeCreated(
            OneOrNoCollaboratorClass subject)
        {
            "Given a class that has a single-collaborator and a parameterless constructor"
                .See<OneOrNoCollaboratorClass>();

            "When I make an object of that type"
                .x(() => subject = Make.A<OneOrNoCollaboratorClass>().From());

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<OneOrNoCollaboratorClass>());

            "And its collaborator should be set"
                .x(() => subject.Collaborator.Should().BeAssignableTo<ICanCollaborate>());
        }

        [Scenario]
        public static void ClassWithOneCollaboratorShouldBeCreated(
            OneCollaboratorClass subject)
        {
            "Given a class that has only a single-collaborator constructor"
                .See<OneCollaboratorClass>();

            "When I make an object of that type"
                .x(() => subject = Make.A<OneCollaboratorClass>().From());

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<OneCollaboratorClass>());

            "And its collaborator should be set"
                .x(() => subject.Collaborator.Should().BeAssignableTo<ICanCollaborate>());
        }

        [Scenario]
        public static void ClassWithOneArgumentShouldBeCreated(
            OneArgumentClass subject)
        {
            "Given a class that has only a single-argument constructor"
                .See<OneArgumentClass>();

            "When I make an object of that type"
                .x(() => subject = Make.A<OneArgumentClass>().From());

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<OneArgumentClass>());
        }

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
    }
}