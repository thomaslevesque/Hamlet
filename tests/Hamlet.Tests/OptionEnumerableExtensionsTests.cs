using FluentAssertions;
using System;
using System.Linq;
using Xunit;
using static Hamlet.Tests.TestHelpers;

namespace Hamlet.Tests
{
    public class OptionEnumerableExtensionsTests
    {
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
