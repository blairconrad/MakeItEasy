namespace MakeItEasy.Specs
{
    using System;

    using FakeItEasy;
    using FluentAssertions;
    using MakeItEasy.Specs.TestTypes;
    using Xbehave;
    using Xunit;

    public static class SubjectConstructorFailsAndDefaultArgumentCannotBeDummiedSpecs
    {
        [Scenario]
        public static void TwoConstructorsFail(
            Exception exception)
        {
            "Given a class that has multiple constructors"
                .See<OneConstructorFailsAnotherHasUnresolvableArgument>();

            "And one constructor fails"
                .See<OneConstructorFailsAnotherHasUnresolvableArgument>();

            "And another has an unresolvable argument"
                .See<OneConstructorFailsAnotherHasUnresolvableArgument>();

            "When I try to make an object of the type"
                .x(() => exception = Record.Exception(() => Make.A<OneConstructorFailsAnotherHasUnresolvableArgument>().FromDefaults()));

            "Then an exception is thrown"
                .x(() => exception.Should().BeOfType<CreationException>());

            "And the exception indicates why the creation failed"
                .x(() => exception.Message.Should().Be(
                    @"
Unable to create MakeItEasy.Specs.SubjectConstructorFailsAndDefaultArgumentCannotBeDummiedSpecs+OneConstructorFailsAnotherHasUnresolvableArgument.

  Failed to create at least one argument needed by a constructor. The following constructors were untried, with unresolvable types marked by *s.
    (*MakeItEasy.Specs.TestTypes.ICannotBeDummied)

  At least one constructor threw an exception. Constructors with the following signatures failed.
    (System.Int32)
      Exception type: System.InvalidOperationException
      Message: I'm not ready
".TrimStart()));
        }

#pragma warning disable CA1801 // Parameter is never used
        public class OneConstructorFailsAnotherHasUnresolvableArgument
        {
            public OneConstructorFailsAnotherHasUnresolvableArgument(int i)
            {
                throw new InvalidOperationException("I'm not ready");
            }

            public OneConstructorFailsAnotherHasUnresolvableArgument(ICannotBeDummied collaborator)
            {
            }
        }
#pragma warning restore CA1801 // Parameter is never used
    }
}
