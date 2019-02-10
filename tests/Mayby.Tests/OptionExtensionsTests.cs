using System.Linq;
using Xunit;
using static Mayby.Tests.TestHelpers;

namespace Mayby.Tests
{
    public class OptionExtensionsTests
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
    }
}
