namespace MakeItEasy.Specs
{
    using System;

    using FakeItEasy;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class DefaultArgumentCannotBeDummiedSpecs
    {
        public interface ICannotBeDummied
        {
        }

        public interface ICannotBeDummiedEither
        {
        }

        [Scenario]
        public static void CannotMakeDefaultArgumentForOnlyConstructor(
            Exception exception)
        {
            "Given a class that has a single constructor"
                .See<RequiredParameterCannotBeDummiedClass>();

            "And the constructor's argument cannot be dummied"
                .See<RequiredParameterCannotBeDummiedClass>();

            "When I try to make an object of the type"
                .x(() => exception = Record.Exception(() => Make.A<RequiredParameterCannotBeDummiedClass>().From()));

            "Then an exception is thrown"
                .x(() => exception.Should().BeOfType<CreationException>());

            "And the exception indicates why the creation failed"
                .x(() => exception.Message.Should().Be(
                    @"
Unable to create MakeItEasy.Specs.DefaultArgumentCannotBeDummiedSpecs+RequiredParameterCannotBeDummiedClass.

  Failed to create at least one argument needed by a constructor. The following constructors were untried, with unresolvable types marked by *s.
    (*MakeItEasy.Specs.DefaultArgumentCannotBeDummiedSpecs+ICannotBeDummied)
".TrimStart()));
        }

        [Scenario]
        public static void CannotMakeDefaultArgumentForAnyConstructor(
            Exception exception)
        {
            "Given a class that has multiple constructors"
                .See<NeitherCollaboratorCanBeDummiedClass>();

            "And neither constructor's argument can be dummied"
                .See<NeitherCollaboratorCanBeDummiedClass>();

            "When I make an object of the type"
                .x(() => exception = Record.Exception(() => Make.A<NeitherCollaboratorCanBeDummiedClass>().From()));

            "Then an exception is thrown"
                .x(() => exception.Should().BeOfType<CreationException>());

            "And the exception indicates why the creation failed"
                .x(() => exception.Message.Should().Be(
                    @"
Unable to create MakeItEasy.Specs.DefaultArgumentCannotBeDummiedSpecs+NeitherCollaboratorCanBeDummiedClass.

  Failed to create at least one argument needed by a constructor. The following constructors were untried, with unresolvable types marked by *s.
    (System.Int32, *MakeItEasy.Specs.DefaultArgumentCannotBeDummiedSpecs+ICannotBeDummiedEither)
    (*MakeItEasy.Specs.DefaultArgumentCannotBeDummiedSpecs+ICannotBeDummied)
".TrimStart()));
        }

        [Scenario]
        public static void CannotMakeDefaultArgumentsForSomeConstructors(
            OptionalCollaboratorsCannotBeDummiedClass subject)
        {
            "Given a class that has multiple constructors"
                .See<OptionalCollaboratorsCannotBeDummiedClass>();

            "And the long constructor's argument cannot be dummied"
                .See<OptionalCollaboratorsCannotBeDummiedClass>();

            "When I make an object of the type"
                .x(() => subject = Make.A<OptionalCollaboratorsCannotBeDummiedClass>().From());

            "Then the object is created"
                .x(() => subject.Should().NotBeNull());
        }

        public class CannotBeDummiedDummyFactory : DummyFactory<ICannotBeDummied>
        {
            protected override ICannotBeDummied Create()
            {
                throw new InvalidOperationException();
            }
        }

        public class CannotBeDummiedEitherDummyFactory : DummyFactory<ICannotBeDummiedEither>
        {
            protected override ICannotBeDummiedEither Create()
            {
                throw new InvalidOperationException();
            }
        }

#pragma warning disable CA1801 // Parameter is never used
        public class RequiredParameterCannotBeDummiedClass
        {
            public RequiredParameterCannotBeDummiedClass(ICannotBeDummied collaborator)
            {
            }
        }

        public class NeitherCollaboratorCanBeDummiedClass
        {
            public NeitherCollaboratorCanBeDummiedClass(ICannotBeDummied collaborator)
            {
            }

            public NeitherCollaboratorCanBeDummiedClass(int i, ICannotBeDummiedEither collaborator)
            {
            }
        }

        public class OptionalCollaboratorsCannotBeDummiedClass
        {
            public OptionalCollaboratorsCannotBeDummiedClass()
            {
            }

            public OptionalCollaboratorsCannotBeDummiedClass(ICannotBeDummied collaborator)
            {
            }

            public OptionalCollaboratorsCannotBeDummiedClass(ICannotBeDummiedEither collaborator)
            {
            }
        }
    }
#pragma warning restore CA1801 // Parameter is never used
}