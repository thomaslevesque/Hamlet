using FakeItEasy;
using FluentAssertions;
using System;
using Xunit;
using static Hamlet.Tests.TestHelpers;

namespace Hamlet.Tests
{
    public class OptionTests
    {
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
        public void Bind_on_some_returns_binder_result()
        {
            var option = Option.Some(41).Bind(x => Option.Some(x + 1));
            option.Should().BeSome(42);
        }

        [Fact]
        public void BindMap_throws_if_argument_is_null()
        {
            var option = Option.Some(42);
            AssertThrowsWhenArgumentNull(() => option.BindMap(
                x => Option.Some(x + 1),
                (x, y) => x + y),
                "binder",
                "mapping");
        }

        [Fact]
        public void BindMap_returns_none_if_first_is_none()
        {
            var option = Option.None<int>().BindMap(
                x => Option.Some(x + 1),
                (x, y) => x + y);
            option.Should().BeNone();
        }

        [Fact]
        public void BindMap_returns_none_if_second_is_none()
        {
            var option = Option.Some(41).BindMap(
                x => Option.None<int>(),
                (x, y) => x + y);
            option.Should().BeNone();
        }

        [Fact]
        public void BindMap_returns_some_if_both_are_some()
        {
            var option = Option.Some(20).BindMap(
                x => Option.Some(x + 2),
                (x, y) => x + y);
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

        [Fact]
        public void ToNullable_returns_null_for_none()
        {
            var option = Option.None<int>();
            option.ToNullable().Should().BeNull();
        }

        [Fact]
        public void ToNullable_returns_value_for_some()
        {
            var option = Option.Some(42);
            option.ToNullable().Should().Be(42);
        }

        [Fact]
        public void SomeIfNotNull_nullable_returns_none_for_null()
        {
            int? nullable = null;
            nullable.SomeIfNotNull().Should().BeNone();
        }

        [Fact]
        public void SomeIfNotNull_nullable_returns_some_for_non_null()
        {
            int? nullable = 42;
            nullable.SomeIfNotNull().Should().BeSome(42);
        }

        [Fact]
        public void SomeIfNotNull_reftype_returns_none_for_null()
        {
            string? value = null;
            value.SomeIfNotNull().Should().BeNone();
        }

        [Fact]
        public void SomeIfNotNull_reftype_returns_some_for_non_null()
        {
            string value = "hello";
            value.SomeIfNotNull().Should().BeSome("hello");
        }

        [Fact]
        public void ToArray_returns_empty_array_for_none()
        {
            var option = Option.None<int>();
            option.ToArray().Should().BeEmpty();
        }

        [Fact]
        public void ToArray_returns_array_of_length_one_for_some()
        {
            var option = Option.Some(42);
            option.ToArray().Should().Equal(42);
        }

        [Fact]
        public void ToList_returns_empty_list_for_none()
        {
            var option = Option.None<int>();
            option.ToList().Should().BeEmpty();
        }

        [Fact]
        public void ToList_returns_list_of_length_one_for_some()
        {
            var option = Option.Some(42);
            option.ToList().Should().Equal(42);
        }

        [Fact]
        public void Do_throws_if_argument_is_null()
        {
            var option = Option.Some(42);
            AssertThrowsWhenArgumentNull(() => option.Do(value => value.ToString()), "action");
        }

        [Fact]
        public void Do_does_nothing_for_none()
        {
            var option = Option.None<int>();
            var action = A.Fake<Action<int>>();
            option.Do(action);
            A.CallTo(action).MustNotHaveHappened();
        }

        [Fact]
        public void Do_calls_action_for_some()
        {
            var option = Option.Some(42);
            var action = A.Fake<Action<int>>();
            option.Do(action);
            A.CallTo(() => action(42)).MustHaveHappenedOnceExactly();
        }
    }
}
