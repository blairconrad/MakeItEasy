namespace MakeItEasy.Specs
{
    using System;

    using FakeItEasy;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class SubjectConstructorFailsSpecs
    {
        [Scenario]
        public static void SomeConstructorsFail(
            SomeConstructorsFailClass subject)
        {
            "Given a class that has multiple constructors"
                .See<SomeConstructorsFailClass>();

            "And the short constructor fails"
                .See<SomeConstructorsFailClass>();

            "When I make an object of the type"
                .x(() => subject = Make.A<SomeConstructorsFailClass>().From());

            "Then creation succeeds"
                .x(() => subject.Should().NotBeNull());
        }

        [Scenario]
        public static void OnlyConstructorFails(
            Exception exception)
        {
            "Given a class that has a single constructor"
                .See<OnlyConstructorFailsClass>();

            "And the constructor fails"
                .See<OnlyConstructorFailsClass>();

            "When I try to make an object of the type"
                .x(() => exception = Record.Exception(() => Make.A<OnlyConstructorFailsClass>().From()));

            "Then an exception is thrown"
                .x(() => exception.Should().BeOfType<CreationException>());

            "And the exception indicates why the creation failed"
                .x(() => exception.Message.Should().Be(
                    @"
Unable to create MakeItEasy.Specs.SubjectConstructorFailsSpecs+OnlyConstructorFailsClass.

  At least one constructor threw an exception. Constructors with the following signatures failed.
    (System.Int32)
      Exception type: System.InvalidOperationException
      Message: a message
".TrimStart()));
        }

        [Scenario]
        public static void TwoConstructorsFail(
            Exception exception)
        {
            "Given a class that has multiple constructors"
                .See<BothConstructorsFailClass>();

            "And the constructor fails"
                .See<BothConstructorsFailClass>();

            "When I try to make an object of the type"
                .x(() => exception = Record.Exception(() => Make.A<BothConstructorsFailClass>().From()));

            "Then an exception is thrown"
                .x(() => exception.Should().BeOfType<CreationException>());

            "And the exception indicates why the creation failed"
                .x(() => exception.Message.Should().Match(
                    @"
Unable to create MakeItEasy.Specs.SubjectConstructorFailsSpecs+BothConstructorsFailClass.

  At least one constructor threw an exception. Constructors with the following signatures failed.
    (System.Int32)
      Exception type: System.ArgumentOutOfRangeException
      Message: Specified argument was out of the range of valid values.*
    ()
      Exception type: System.InvalidOperationException
      Message: a message
".TrimStart()));
        }

#pragma warning disable CA1801 // Parameter is never used
        public class OnlyConstructorFailsClass
        {
            public OnlyConstructorFailsClass(int i)
            {
                throw new InvalidOperationException("a message");
            }
        }

        public class BothConstructorsFailClass
        {
            public BothConstructorsFailClass()
            {
                throw new InvalidOperationException("a message");
            }

            public BothConstructorsFailClass(int i)
            {
                throw new ArgumentOutOfRangeException(nameof(i));
            }
        }

        public class SomeConstructorsFailClass
        {
            public SomeConstructorsFailClass()
            {
            }

            public SomeConstructorsFailClass(int i)
            {
                throw new ArgumentOutOfRangeException(nameof(i));
            }
        }
#pragma warning restore CA1801 // Parameter is never used
    }
}
