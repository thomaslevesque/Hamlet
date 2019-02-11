using System;
using Xunit;
using FluentAssertions;
using static Hamlet.Tests.TestHelpers;

namespace Hamlet.Tests
{
    public class OptionOfTTests
    {
        [Fact]
        public void IsSome_returns_true_for_some()
        {
            var option = Option.Some(42);
            option.IsSome.Should().BeTrue();
        }

        [Fact]
        public void IsNone_returns_false_for_some()
        {
            var option = Option.Some(42);
            option.IsNone.Should().BeFalse();
        }

        [Fact]
        public void IsSome_returns_false_for_none()
        {
            var option = Option.None<int>();
            option.IsSome.Should().BeFalse();
        }

        [Fact]
        public void IsNone_returns_true_for_none()
        {
            var option = Option.None<int>();
            option.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Value_returns_value_for_some()
        {
            var option = Option.Some(42);
            option.Value.Should().Be(42);
        }

        [Fact]
        public void Value_throws_for_none()
        {
            var option = Option.None<int>();
            var exception = Record.Exception(() => option.Value);
            exception.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public void GetValueOrDefault_returns_value_for_some()
        {
            var option = Option.Some(42);
            option.GetValueOrDefault().Should().Be(42);
        }

        [Fact]
        public void GetValueOrDefault_returns_default_for_none()
        {
            var option = Option.None<int>();
            option.GetValueOrDefault().Should().Be(0);
        }

        [Fact]
        public void GetValueOrDefault_returns_specified_default_for_none()
        {
            var option = Option.None<int>();
            option.GetValueOrDefault(3).Should().Be(3);
        }

        [Fact]
        public void TryGetValue_returns_true_and_sets_value_for_some()
        {
            var option = Option.Some(42);
            option.TryGetValue(out var value).Should().BeTrue();
            value.Should().Be(42);
        }

        [Fact]
        public void TryGetValue_returns_false_for_none()
        {
            var option = Option.None<int>();
            option.TryGetValue(out _).Should().BeFalse();
        }

        [Fact]
        public void Equals_object_returns_false_if_argument_is_not_an_option()
        {
            var option = Option.Some(42);
            object arg = "hello";
            option.Equals(arg).Should().BeFalse();
        }

        [Fact]
        public void Equals_object_returns_false_if_argument_is_a_different_option()
        {
            var option = Option.Some(42);
            object arg = Option.Some(123);
            option.Equals(arg).Should().BeFalse();
        }

        [Fact]
        public void Equals_object_returns_true_if_argument_is_an_identical_option()
        {
            var option = Option.Some(42);
            object arg = Option.Some(42);
            option.Equals(arg).Should().BeTrue();
        }

        [Fact]
        public void Equals_returns_false_for_some_and_none()
        {
            var a = Option.Some(42);
            var b = Option.None<int>();
            a.Equals(b).Should().BeFalse();
        }

        [Fact]
        public void Equals_returns_false_for_none_and_some()
        {
            var a = Option.None<int>();
            var b = Option.Some(42);
            a.Equals(b).Should().BeFalse();
        }

        [Fact]
        public void Equals_returns_false_for_different_values()
        {
            var a = Option.Some(42);
            var b = Option.Some(123);
            a.Equals(b).Should().BeFalse();
        }

        [Fact]
        public void Equals_returns_true_for_none_and_none()
        {
            var a = Option.None<int>();
            var b = Option.None<int>();
            a.Equals(b).Should().BeTrue();
        }

        [Fact]
        public void Equals_returns_true_for_same_values()
        {
            var a = Option.Some(42);
            var b = Option.Some(42);
            a.Equals(b).Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_returns_zero_for_none()
        {
            var option = Option.None<int>();
            option.GetHashCode().Should().Be(0);
        }

        [Fact]
        public void GetHashCode_returns_zero_for_some_null()
        {
            var option = Option.Some(default(string));
            option.GetHashCode().Should().Be(0);
        }

        [Fact]
        public void GetHashCode_returns_value_hashcode_for_some()
        {
            var option = Option.Some(42);
            option.GetHashCode().Should().Be(42.GetHashCode());
        }

        [Fact]
        public void ToString_returns_none_for_none()
        {
            var option = Option.None<int>();
            option.ToString().Should().Be("None");
        }

        [Fact]
        public void ToString_returns_some_value_for_some_int()
        {
            var option = Option.Some(42);
            option.ToString().Should().Be("Some 42");
        }

        [Fact]
        public void ToString_returns_some_value_for_some_string()
        {
            var option = Option.Some("test");
            option.ToString().Should().Be("Some \"test\"");
        }

        [Fact]
        public void ToString_returns_some_value_for_some_null()
        {
            var option = Option.Some(default(string));
            option.ToString().Should().Be("Some null");
        }

        [Fact]
        public void Implicit_conversion_from_value_returns_some()
        {
            Option<int> option = 42;
            option.Should().BeSome(42);
        }

        [Fact]
        public void Implicit_conversion_from_none_returns_none()
        {
            Option<int> option = Option.None();
            option.Should().BeNone();
        }

        [Fact]
        public void Filter_throws_if_argument_is_null()
        {
            var option = Option.Some(42);
            AssertThrowsWhenArgumentNull(() => option.Filter(x => true), "predicate");
        }

        [Fact]
        public void Filter_on_none_returns_none()
        {
            var option = Option.None<int>().Filter(x => true);
            option.Should().BeNone();
        }

        [Fact]
        public void Filter_on_some_with_false_predicate_returns_none()
        {
            var option = Option.Some(42).Filter(x => x % 2 == 1);
            option.Should().BeNone();
        }

        [Fact]
        public void Filter_on_some_with_true_predicate_returns_some()
        {
            var option = Option.Some(42).Filter(x => x % 2 == 0);
            option.Should().BeSome(42);
        }

        [Fact]
        public void Map_throws_if_argument_is_null()
        {
            var option = Option.Some(42);
            AssertThrowsWhenArgumentNull(() => option.Map(x => "hello " + x), "mapping");
        }

        [Fact]
        public void Map_on_none_returns_none()
        {
            var option = Option.None<int>().Map(x => "hello " + x);
            option.Should().BeNone();
        }

        [Fact]
        public void Map_on_some_returns_some()
        {
            var option = Option.Some(42).Map(x => "hello " + x);
            option.Should().BeSome("hello 42");
        }

        [Fact]
        public void Bind_throws_if_argument_is_null()
        {
            var option = Option.Some(42);
            AssertThrowsWhenArgumentNull(() => option.Bind(x => Option.Some(x + 1)), "binder");
        }

        [Fact]
        public void Bind_on_none_returns_none()
        {
            var option = Option.None<int>().Bind(x => Option.Some(x + 1));
            option.Should().BeNone();
        }

        [Fact]
        public void Bind_on_some_returns_projection_result()
        {
            var option = Option.Some(41).Bind(x => Option.Some(x + 1));
            option.Should().BeSome(42);
        }

        [Fact]
        public void Match_throws_if_argument_is_null()
        {
            var option = Option.Some(42);
            AssertThrowsWhenArgumentNull(() => option.Match(x => x, () => 0), "some", "none");
        }

        [Fact]
        public void Match_evaluates_the_some_projection_for_some()
        {
            var option = Option.Some(21);
            option.Match(some: x => x * 2, none: () => 0).Should().Be(42);
        }

        [Fact]
        public void Match_evaluates_the_non_projection_for_none()
        {
            var option = Option.None<int>();
            option.Match(some: x => 0, none: () => 42).Should().Be(42);
        }
    }
}
