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
        public void Equals_with_value_returns_false_for_none()
        {
            var a = Option.None<int>();
            var b = 42;
            a.Equals(b).Should().BeFalse();
        }

        [Fact]
        public void Equals_with_value_returns_false_some_different_value()
        {
            var a = Option.Some(42);
            var b = 123;
            a.Equals(b).Should().BeFalse();
        }

        [Fact]
        public void Equals_with_value_returns_true_for_same_values()
        {
            var a = Option.Some(42);
            var b = 42;
            a.Equals(b).Should().BeTrue();
        }

        [Fact]
        public void Equality_operator_returns_false_for_some_and_none()
        {
            var a = Option.Some(42);
            var b = Option.None<int>();
            (a == b).Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_returns_false_for_none_and_some()
        {
            var a = Option.None<int>();
            var b = Option.Some(42);
            (a == b).Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_returns_false_for_different_values()
        {
            var a = Option.Some(42);
            var b = Option.Some(123);
            (a == b).Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_returns_true_for_none_and_none()
        {
            var a = Option.None<int>();
            var b = Option.None<int>();
            (a == b).Should().BeTrue();
        }

        [Fact]
        public void Equality_operator_returns_true_for_same_values()
        {
            var a = Option.Some(42);
            var b = Option.Some(42);
            (a == b).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_returns_true_for_some_and_none()
        {
            var a = Option.Some(42);
            var b = Option.None<int>();
            (a != b).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_returns_true_for_none_and_some()
        {
            var a = Option.None<int>();
            var b = Option.Some(42);
            (a != b).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_returns_true_for_different_values()
        {
            var a = Option.Some(42);
            var b = Option.Some(123);
            (a != b).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_returns_false_for_none_and_none()
        {
            var a = Option.None<int>();
            var b = Option.None<int>();
            (a != b).Should().BeFalse();
        }

        [Fact]
        public void Inequality_operator_returns_false_for_same_values()
        {
            var a = Option.Some(42);
            var b = Option.Some(42);
            (a != b).Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_with_option_and_value_returns_false_for_none()
        {
            var option = Option.None<int>();
            var value = 42;
            (option == value).Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_with_option_and_value_returns_false_for_some_different_value()
        {
            var option = Option.Some(42);
            var value = 123;
            (option == value).Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_with_option_and_value_returns_true_for_same_values()
        {
            var option = Option.Some(42);
            var value = 42;
            (option == value).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_with_option_and_value_returns_true_for_none()
        {
            var option = Option.None<int>();
            var value = 42;
            (option != value).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_with_option_and_value_returns_true_for_some_different_value()
        {
            var option = Option.Some(42);
            var value = 123;
            (option != value).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_with_option_and_value_returns_false_for_same_values()
        {
            var option = Option.Some(42);
            var value = 42;
            (option != value).Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_with_value_and_option_returns_false_for_none()
        {
            var option = Option.None<int>();
            var value = 42;
            (value == option).Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_with_value_and_option_returns_false_for_some_different_value()
        {
            var option = Option.Some(42);
            var value = 123;
            (value == option).Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_with_value_and_option_returns_true_for_same_values()
        {
            var option = Option.Some(42);
            var value = 42;
            (value == option).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_with_value_and_option_returns_true_for_none()
        {
            var option = Option.None<int>();
            var value = 42;
            (value != option).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_with_value_and_option_returns_true_for_some_different_value()
        {
            var option = Option.Some(42);
            var value = 123;
            (value != option).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_with_value_and_option_returns_false_for_same_values()
        {
            var option = Option.Some(42);
            var value = 42;
            (value != option).Should().BeFalse();
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
    }
}
