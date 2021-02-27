namespace MakeItEasy.Specs
{
    using System;

    using FakeItEasy;
    using FluentAssertions;
    using MakeItEasy.Specs.TestTypes;
    using Xbehave;
    using Xunit;

    public static class MakeWithSuppliedArgumentAndCollaboratorsSpecs
    {
        [Scenario]
        public static void ClassWithNoMatchingCollaboratorCannotBeCreated(
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

        [Scenario]
        public static void ClassWithMatchingArgumentAndCollaboratorCanBeCreated(
            OneCollaboratorOneArgumentClass subject, ICanCollaborate collaborator)
        {
            "Given a class that has a two-parameter constructor"
                .See<OneCollaboratorOneArgumentClass>();

            "When I make an object of that type, supplying an argument anding ask for a collaborator back"
                .x(() => subject = Make.A<OneCollaboratorOneArgumentClass>()
                    .From(-5, out collaborator));

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<OneCollaboratorOneArgumentClass>());

            "And the subject's constructor used the supplied argument"
                .x(() => subject.Argument.Should().Be(-5));

            "And the subject uses the returned collaborator"
                .x(() =>
                    {
                        A.CallTo(() => collaborator.IntMethod()).Returns(-6);
                        subject.GetIntFromCollaborator().Should().Be(-6);
                    });
        }

        [Scenario]
        public static void ClassWithArgumentAndCollaboratorOffTheSameTypeCanBeCreated(
            TwoOfTheSameCollaboratorsClass subject, ICanCollaborate argument, ICanCollaborate collaborator)
        {
            "Given a class that has a constructor taking two of the same kind of collaborator"
                .See<TwoOfTheSameCollaboratorsClass>();

            "And a preconfigured argument"
                .x(() =>
                    {
                        argument = A.Fake<ICanCollaborate>();
                        A.CallTo(() => argument.IntMethod()).Returns(-8);
                    });

            "When I make an object of that type, supplying one as an argument and asking for the other back"
                .x(() => subject = Make.A<TwoOfTheSameCollaboratorsClass>()
                    .From(argument, out collaborator));

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<TwoOfTheSameCollaboratorsClass>());

            "And the argument and collaborator are distinct"
                .x(() => argument.Should().NotBeSameAs(collaborator));

            "And the subject uses the argument as the first collaborator"
                .x(() => subject.GetIntFromCollaborator1().Should().Be(-8));

            "And the subject uses the returned collaborator as the second collaborator"
                .x(() =>
                    {
                        A.CallTo(() => collaborator.IntMethod()).Returns(-9);
                        subject.GetIntFromCollaborator2().Should().Be(-9);
                    });
        }
    }
}