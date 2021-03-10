namespace MakeItEasy.Specs
{
    using System;

    using FluentAssertions;
    using MakeItEasy.Specs.TestTypes;
    using Xbehave;
    using Xunit;

    public static class MakeWithSuppliedArgumentsOnlySpecs
    {
        [Scenario]
        public static void ClassWithMatchingArgumentCanBeCreated(
            OneCollaboratorOneArgumentClass subject)
        {
            "Given a class that has a two-parameter constructor"
                .See<OneCollaboratorOneArgumentClass>();

            "When I make an object of that type and supply an argument"
                .x(() => subject = Make.A<OneCollaboratorOneArgumentClass>().From(7));

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<OneCollaboratorOneArgumentClass>());

            "And the supplied argument is used in the constructor"
                .x(() => subject.Argument.Should().Be(7));
        }

        [Scenario]
        public static void ClassWithTwoArgumentsCanBeCreated(
            TwoArgumentClass subject)
        {
            "Given a class that has a two-parameter constructor"
                .See<TwoArgumentClass>();

            "When I make an object of that type and supply both arguments"
                .x(() => subject = Make.A<TwoArgumentClass>().From(8, "eight"));

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<TwoArgumentClass>());

            "And the first supplied argument is used in the constructor"
                .x(() => subject.Argument1.Should().Be(8));

            "And the second supplied argument is used in the constructor"
                .x(() => subject.Argument2.Should().Be("eight"));
        }

        [Scenario]
        public static void ClassWithTwoArgumentsInTheWrongOrderCanBeCreated(
            TwoArgumentClass subject)
        {
            "Given a class that has a two-parameter constructor"
                .See<TwoArgumentClass>();

            "When I make an object of that type and supply both arguments, but in the wrong order"
                .x(() => subject = Make.A<TwoArgumentClass>().From("nine", 9));

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<TwoArgumentClass>());

            "And the first supplied argument is used in the constructor"
                .x(() => subject.Argument2.Should().Be("nine"));

            "And the second supplied argument is used in the constructor"
                .x(() => subject.Argument1.Should().Be(9));
        }

        [Scenario]
        public static void ClassWithTwoOfTheSameArgumentsCanBeCreated(
            TwoOfTheSameArgumentsClass subject)
        {
            "Given a class that has a constructor taking two of the same kind of argument"
                .See<TwoOfTheSameArgumentsClass>();

            "When I make an object of that type and ask for both arguments back"
                .x(() => subject = Make.A<TwoOfTheSameArgumentsClass>().From("argument1", "argument2"));

            "Then the subject should be of the desired type"
                .x(() => subject.Should().BeOfType<TwoOfTheSameArgumentsClass>());

            "And the first supplied argument is used as the first parameter"
                .x(() => subject.Argument1.Should().Be("argument1"));

            "And the second supplied argument is used as the second parameter"
                .x(() => subject.Argument2.Should().Be("argument2"));
        }
    }
}