using System;
using Xunit;
using FluentAssertions;
using static Mayby.Tests.TestHelpers;

namespace Mayby.Tests
{
    public class OptionOfTTests
    {
        [Fact]
        public void HasValue_returns_true_for_some()
        {
            var option = Option.Some(42);
            option.HasValue.Should().BeTrue();
        }

        [Fact]
        public void HasValue_returns_false_for_none()
        {
            var option = Option.None<int>();
            option.HasValue.Should().BeFalse();
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
        public void Where_throws_if_argument_is_null()
        {
            var option = Option.Some(42);
            AssertThrowsWhenArgumentNull(() => option.Where(x => true), "predicate");
        }

        [Fact]
        public void Where_on_none_returns_none()
        {
            var option = Option.None<int>().Where(x => true);
            option.Should().BeNone();
        }

        [Fact]
        public void Where_on_some_with_false_predicate_returns_none()
        {
            var option = Option.Some(42).Where(x => x % 2 == 1);
            option.Should().BeNone();
        }

        [Fact]
        public void Where_on_some_with_true_predicate_returns_some()
        {
            var option = Option.Some(42).Where(x => x % 2 == 0);
            option.Should().BeSome(42);
        }

        [Fact]
        public void Select_throws_if_argument_is_null()
        {
            var option = Option.Some(42);
            AssertThrowsWhenArgumentNull(() => option.Select(x => "hello " + x), "projection");
        }

        [Fact]
        public void Select_on_none_returns_none()
        {
            var option = Option.None<int>().Select(x => "hello " + x);
            option.Should().BeNone();
        }

        [Fact]
        public void Select_on_some_returns_some()
        {
            var option = Option.Some(42).Select(x => "hello " + x);
            option.Should().BeSome("hello 42");
        }

        [Fact]
        public void SelectMany_throws_if_argument_is_null()
        {
            var option = Option.Some(42);
            AssertThrowsWhenArgumentNull(() => option.SelectMany(x => Option.Some(x + 1)), "projection");
        }

        [Fact]
        public void SelectMany_on_none_returns_none()
        {
            var option = Option.None<int>().SelectMany(x => Option.Some(x + 1));
            option.Should().BeNone();
        }

        [Fact]
        public void SelectMany_on_some_returns_projection_result()
        {
            var option = Option.Some(41).SelectMany(x => Option.Some(x + 1));
            option.Should().BeSome(42);
        }

        [Fact]
        public void SelectManyWithResultProjection_throws_if_argument_is_null()
        {
            var option = Option.Some(42);
            AssertThrowsWhenArgumentNull(() => option.SelectMany(x => Option.Some(123), (x, y) => x + y), "projection", "resultProjection");
        }

        [Fact]
        public void SelectManyWithResultProjection_returns_none_if_both_are_none()
        {
            var q =
                from a in Option.None<int>()
                from b in Option.None<int>()
                select a + b;

            q.Should().BeNone();
        }

        [Fact]
        public void SelectManyWithResultProjection_returns_some_if_both_are_some()
        {
            var q =
                from a in Option.Some(1)
                from b in Option.Some(2)
                select a + b;

            q.Should().BeSome(3);
        }

        [Fact]
        public void SelectManyWithResultProjection_returns_none_if_first_is_none()
        {
            var q =
                from a in Option.None<int>()
                from b in Option.Some(2)
                select a + b;

            q.Should().BeNone();
        }

        [Fact]
        public void SelectManyWithResultProjection_returns_none_if_second_is_none()
        {
            var q =
                from a in Option.Some(1)
                from b in Option.None<int>()
                select a + b;

            q.Should().BeNone();
        }
    }
}
