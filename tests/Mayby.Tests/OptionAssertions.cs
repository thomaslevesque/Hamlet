using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Mayby.Tests
{
    internal class OptionAssertions<T>
    {
        public OptionAssertions(Option<T> option)
        {
            Subject = option;
        }

        public Option<T> Subject { get; }

        public AndConstraint<OptionAssertions<T>> BeNone(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:option} to be None{reason}, but found {0}.", Subject);
            return new AndConstraint<OptionAssertions<T>>(this);
        }

        public AndConstraint<OptionAssertions<T>> BeSome(T value, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && EqualityComparer<T>.Default.Equals(value, Subject.Value))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:option} to be {0}{reason}, but found {1}.", Option.Some(value), Subject);
            return new AndConstraint<OptionAssertions<T>>(this);
        }

        public AndConstraint<OptionAssertions<T>> BeSome(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:option} to be Some{reason}, but found {0}.", Subject);
            return new AndConstraint<OptionAssertions<T>>(this);
        }
    }

    internal static class OptionAssertionExtensions
    {
        public static OptionAssertions<T> Should<T>(this Option<T> option) => new OptionAssertions<T>(option);
    }
}