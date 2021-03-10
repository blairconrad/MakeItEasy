namespace MakeItEasy.Specs
{
    using System;

    using FakeItEasy;
    using FluentAssertions;
    using MakeItEasy.Specs.TestTypes;
    using Xbehave;
    using Xunit;

    public static class CollaboratorCannotBeFakedSpecs
    {
        [Scenario]
        public static void CollaboratorCannotBeFaked(
            Exception exception)
        {
            "Given a class that has a collaborator"
                .See<ClassWithCollaboratorThatCannotBeFaked>();

            "And the collaborator cannot be faked"
                .See<NoPublicConstructorClass>();

            "When I make an object of the first type, asking for the collaborator"
                .x(() => exception = Record.Exception(() =>
                    Make.A<ClassWithCollaboratorThatCannotBeFaked>().From(out NoPublicConstructorClass collaborator)));

            "Then an exception is thrown"
                .x(() => exception.Should().BeOfType<CreationException>());

            "And the exception indicates why the creation failed"
                .x(() => exception.Message.Should().Be(
                    "Unable to create MakeItEasy.Specs.CollaboratorCannotBeFakedSpecs+ClassWithCollaboratorThatCannotBeFaked. " +
                    "Failed to create Fake collaborator of type MakeItEasy.Specs.TestTypes.NoPublicConstructorClass."));
        }

        public class ClassWithCollaboratorThatCannotBeFaked
        {
#pragma warning disable CA1801 // Parameter is never used
            public ClassWithCollaboratorThatCannotBeFaked(NoPublicConstructorClass collaborator)
            {
            }
#pragma warning restore CA1801 // Parameter is never used
        }
    }
}