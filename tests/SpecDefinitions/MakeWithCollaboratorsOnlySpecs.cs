namespace MakeItEasy.Specs
{
    using System;

    using FakeItEasy;
    using FluentAssertions;
    using MakeItEasy.Specs.TestTypes;
    using Xbehave;
    using Xunit;

    public static class MakeWithCollaboratorsOnlySpecs
    {
        [Scenario]
        public static void ClassWithNoMatchingCollaboratorCannotBeCreated(
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
        public static void ClassWithMatchingCollaboratorCanBeCreated(
            OneCollaboratorClass subject, ICanCollaborate collaborator)
        {
            "Given a class that has a one-parameter constructor"
                .See<OneCollaboratorClass>();

            "When I make an object of that type and ask for a collaborator back"
                .x(() => subject = Make.A<OneCollaboratorClass>().From(out collaborator));

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<OneCollaboratorClass>());

            "And the subject uses the returned collaborator"
                .x(() =>
                    {
                        A.CallTo(() => collaborator.IntMethod()).Returns(3);
                        subject.GetIntFromCollaborator().Should().Be(3);
                    });
        }

        [Scenario]
        public static void ClassWithTwoCollaboratorsCanBeCreated(
            TwoCollaboratorClass subject, ICanCollaborate collaborator1, ICanCollaborateToo collaborator2)
        {
            "Given a class that has a two-parameter constructor"
                .See<TwoCollaboratorClass>();

            "When I make an object of that type and ask for both collaborators back"
                .x(() => subject = Make.A<TwoCollaboratorClass>().From(out collaborator1, out collaborator2));

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<TwoCollaboratorClass>());

            "And the subject uses the first returned collaborator"
                .x(() =>
                    {
                        A.CallTo(() => collaborator1.IntMethod()).Returns(5);
                        subject.GetIntFromCollaborator1().Should().Be(5);
                    });

            "And the subject uses the second returned collaborator"
                .x(() =>
                    {
                        A.CallTo(() => collaborator2.StringMethod()).Returns("five");
                        subject.GetStringFromCollaborator2().Should().Be("five");
                    });
        }

        [Scenario]
        public static void ClassWithTwoCollaboratorsInTheWrongOrderCanBeCreated(
            TwoCollaboratorClass subject, ICanCollaborate collaborator1, ICanCollaborateToo collaborator2)
        {
            "Given a class that has a two-parameter constructor"
                .See<TwoCollaboratorClass>();

            "When I make an object of that type and ask for both collaborators back, but in the wrong order"
                .x(() => subject = Make.A<TwoCollaboratorClass>().From(out collaborator2, out collaborator1));

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<TwoCollaboratorClass>());

            "And the subject uses the first returned collaborator"
                .x(() =>
                    {
                        A.CallTo(() => collaborator2.StringMethod()).Returns("fivebackwards");
                        subject.GetStringFromCollaborator2().Should().Be("fivebackwards");
                    });

            "And the subject uses the second returned collaborator"
                .x(() =>
                    {
                        A.CallTo(() => collaborator1.IntMethod()).Returns(50);
                        subject.GetIntFromCollaborator1().Should().Be(50);
                    });
        }

        [Scenario]
        public static void ClassWithTwoOfTheSameCollaboratorsCanBeCreated(
            TwoOfTheSameCollaboratorsClass subject, ICanCollaborate collaborator1, ICanCollaborate collaborator2)
        {
            "Given a class that has a constructor taking two of the same kind of collaborator"
                .See<TwoOfTheSameCollaboratorsClass>();

            "When I make an object of that type and ask for both collaborators back"
                .x(() => subject = Make.A<TwoOfTheSameCollaboratorsClass>().From(out collaborator1, out collaborator2));

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<TwoOfTheSameCollaboratorsClass>());

            "And the collaborators are distinct"
                .x(() => collaborator1.Should().NotBeSameAs(collaborator2));

            "And the subject uses the first returned collaborator"
                .x(() =>
                    {
                        A.CallTo(() => collaborator1.IntMethod()).Returns(6);
                        subject.GetIntFromCollaborator1().Should().Be(6);
                    });

            "And the subject uses the second returned collaborator"
                .x(() =>
                    {
                        A.CallTo(() => collaborator2.IntMethod()).Returns(7);
                        subject.GetIntFromCollaborator2().Should().Be(7);
                    });
        }

        [Scenario]
        public static void FakesNotCreatedForNonMatchingConstructors()
        {
            "Given a class that has multiple constructors"
                .See<ClassWithMultipleConstructors>();

            "When I make an object of that type and ask for a collaborator that is not used in the longer constructor"
                .x(() => Make.A<ClassWithMultipleConstructors>().From(out ConstructorCountingClass countingCollaborator, out ICanCollaborate choosyCollaborator));

            "Then the collaborator class common to the constructors is faked only once"
                .x(() => ConstructorCountingClass.IntancesCreated.Should().Be(1));
        }

#pragma warning disable CA1034 // Do not nest type
#pragma warning disable CA1052 // Type is a static holder type but is neither static nor NotInheritable
        public class ClassWithMultipleConstructors
        {
#pragma warning disable CA1801 // Parameter is never used
            public ClassWithMultipleConstructors(ConstructorCountingClass collaborator1, int signatureLengthener1, int signatureLengthener2)
            {
            }

            public ClassWithMultipleConstructors(ConstructorCountingClass collaborator1, ICanCollaborate collaborator2)
            {
            }
#pragma warning restore CA1801 // Parameter is never used
        }

        public class ConstructorCountingClass
        {
            public ConstructorCountingClass()
            {
                ++IntancesCreated;
            }

            internal static int IntancesCreated { get; private set; }
        }
#pragma warning restore CA1052 // Type is a static holder type but is neither static nor NotInheritable
#pragma warning restore CA1034 // Do not nest type
    }
}