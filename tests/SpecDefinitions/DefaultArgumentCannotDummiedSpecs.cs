namespace MakeItEasy.Specs
{
    using System;

    using FakeItEasy;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class DefaultArgumentCannotDummiedSpecs
    {
        public interface ICannotBeDummied
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
                    "Unable to create MakeItEasy.Specs.DefaultArgumentCannotDummiedSpecs+RequiredParameterCannotBeDummiedClass. " +
                    "Failed to create Dummy argument of type MakeItEasy.Specs.DefaultArgumentCannotDummiedSpecs+ICannotBeDummied."));
        }

        public class CannotBeDummiedDummyFactory : DummyFactory<ICannotBeDummied>
        {
            protected override ICannotBeDummied Create()
            {
                throw new InvalidOperationException();
            }
        }

        public class RequiredParameterCannotBeDummiedClass
        {
#pragma warning disable CA1801 // Parameter is never used
            public RequiredParameterCannotBeDummiedClass(ICannotBeDummied collaborator)
            {
            }
#pragma warning restore CA1801 // Parameter is never used
        }
    }
}