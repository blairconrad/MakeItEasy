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
                .x(() => subject = Make.A<NoCollaboratorsClass>().FromDefaults());

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
                .x(() => subject = Make.A<OneOrNoCollaboratorClass>().FromDefaults());

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
                .x(() => subject = Make.A<OneCollaboratorClass>().FromDefaults());

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
                .x(() => subject = Make.A<OneArgumentClass>().FromDefaults());

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<OneArgumentClass>());
        }
    }
}