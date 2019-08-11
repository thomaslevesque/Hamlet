using FluentAssertions;
using System;
using System.Linq;
using Xunit;
using static Hamlet.Tests.TestHelpers;

namespace Hamlet.Tests
{
    public class LinqOptionExtensionsTests
    {
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
            AssertThrowsWhenArgumentNull(() => option.Select(x => "hello " + x), "selector");
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
            AssertThrowsWhenArgumentNull(() => option.SelectMany(x => Option.Some(x + 1)), "selector");
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
            AssertThrowsWhenArgumentNull(() => option.SelectMany(x => Option.Some(123), (x, y) => x + y), "optionSelector", "resultSelector");
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

        [Fact]
        public void ChooseWithChooser_throws_if_argument_is_null()
        {
            var source = Enumerable.Empty<int>();
            AssertThrowsWhenArgumentNull(
                () => source.Choose(x => x % 2 == 0 ? Option.Some(x) : Option.None()),
                "source", "chooser");
        }

        [Fact]
        public void ChooseWithChooser_returns_empty_if_all_are_none()
        {
            var values = new[] { 1, 3, 5 };
            var result = values.Choose(x => x % 2 == 0 ? Option.Some(x) : Option.None());
            result.Should().BeEmpty();
        }

        [Fact]
        public void ChooseWithChooser_returns_all_if_all_are_some()
        {
            var values = new[] { 2, 4, 6 };
            var result = values.Choose(x => x % 2 == 0 ? Option.Some(x) : Option.None());
            result.Should().Equal(values);
        }

        [Fact]
        public void ChooseWithChooser_returns_only_somes()
        {
            var values = new[] { 1, 2, 3, 4 };
            var result = values.Choose(x => x % 2 == 0 ? Option.Some(x) : Option.None());
            result.Should().Equal(2, 4);
        }

        [Fact]
        public void Choose_throws_if_argument_is_null()
        {
            var source = Enumerable.Empty<Option<int>>();
            AssertThrowsWhenArgumentNull(() => source.Choose(), "source");
        }

        [Fact]
        public void Choose_returns_empty_if_all_are_none()
        {
            var options = new[] { Option.None<int>(), Option.None<int>(), Option.None<int>() };
            var result = options.Choose();
            result.Should().BeEmpty();
        }

        [Fact]
        public void Choose_returns_all_if_all_are_some()
        {
            var options = new[] { Option.Some(1), Option.Some(2), Option.Some(3) };
            var result = options.Choose();
            result.Should().Equal(1, 2, 3);
        }

        [Fact]
        public void Choose_returns_only_somes()
        {
            var options = new[] { Option.None<int>(), Option.Some(2), Option.None<int>(), Option.Some(4) };
            var result = options.Choose();
            result.Should().Equal(2, 4);
        }

        [Fact]
        public void PickWithChooser_throws_if_argument_is_null()
        {
            var source = Enumerable.Empty<int>();
            AssertThrowsWhenArgumentNull(
                () => source.Pick(x => x % 2 == 0 ? Option.Some(x) : Option.None()),
                "source", "chooser");
        }

        [Fact]
        public void PickWithChooser_throws_if_all_are_none()
        {
            var options = new[] { 1, 3, 5 };
            var exception = Record.Exception(() => options.Pick(x => x % 2 == 0 ? Option.Some(x) : Option.None()));
            exception.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public void PickWithChooser_returns_first_if_all_are_some()
        {
            var options = new[] { 2, 4, 6 };
            var result = options.Pick(x => x % 2 == 0 ? Option.Some(x) : Option.None());
            result.Should().Be(2);
        }

        [Fact]
        public void PickWithChooser_returns_first_some()
        {
            var options = new[] { 1, 2, 3, 4 };
            var result = options.Pick(x => x % 2 == 0 ? Option.Some(x) : Option.None());
            result.Should().Be(2);
        }

        [Fact]
        public void Pick_throws_if_argument_is_null()
        {
            var source = Enumerable.Empty<Option<int>>();
            AssertThrowsWhenArgumentNull(() => source.Pick(), "source");
        }

        [Fact]
        public void Pick_throws_if_all_are_none()
        {
            var options = new[] { Option.None<int>(), Option.None<int>(), Option.None<int>() };
            var exception = Record.Exception(() => options.Pick());
            exception.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public void Pick_returns_first_if_all_are_some()
        {
            var options = new[] { Option.Some(1), Option.Some(2), Option.Some(3) };
            var result = options.Pick();
            result.Should().Be(1);
        }

        [Fact]
        public void Pick_returns_first_some()
        {
            var options = new[] { Option.None<int>(), Option.Some(2), Option.None<int>(), Option.Some(4) };
            var result = options.Pick();
            result.Should().Be(2);
        }

        [Fact]
        public void TryPickWithChooser_throws_if_argument_is_null()
        {
            var source = Enumerable.Empty<int>();
            AssertThrowsWhenArgumentNull(
                () => source.TryPick(x => x % 2 == 0 ? Option.Some(x) : Option.None()),
                "source", "chooser");
        }

        [Fact]
        public void TryPickWithChooser_returns_none_if_all_are_none()
        {
            var options = new[] { 1, 3, 5 };
            var result = options.TryPick(x => x % 2 == 0 ? Option.Some(x) : Option.None());
            result.Should().BeNone();
        }

        [Fact]
        public void TryPickWithChooser_returns_first_if_all_are_some()
        {
            var options = new[] { 2, 4, 6 };
            var result = options.TryPick(x => x % 2 == 0 ? Option.Some(x) : Option.None());
            result.Should().BeSome(2);
        }

        [Fact]
        public void TryPickWithChooser_returns_first_some()
        {
            var options = new[] { 1, 2, 3, 4 };
            var result = options.TryPick(x => x % 2 == 0 ? Option.Some(x) : Option.None());
            result.Should().BeSome(2);
        }

        [Fact]
        public void TryPick_throws_if_argument_is_null()
        {
            var source = Enumerable.Empty<Option<int>>();
            AssertThrowsWhenArgumentNull(() => source.TryPick(), "source");
        }

        [Fact]
        public void TryPick_returns_none_if_all_are_none()
        {
            var options = new[] { Option.None<int>(), Option.None<int>(), Option.None<int>() };
            var result = options.TryPick();
            result.Should().BeNone();
        }

        [Fact]
        public void TryPick_returns_first_if_all_are_some()
        {
            var options = new[] { Option.Some(1), Option.Some(2), Option.Some(3) };
            var result = options.TryPick();
            result.Should().BeSome(1);
        }

        [Fact]
        public void TryPick_returns_first_some()
        {
            var options = new[] { Option.None<int>(), Option.Some(2), Option.None<int>(), Option.Some(4) };
            var result = options.TryPick();
            result.Should().BeSome(2);
        }
    }
}
