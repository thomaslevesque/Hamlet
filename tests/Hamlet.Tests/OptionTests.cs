using FluentAssertions;
using Xunit;

namespace Hamlet.Tests
{
    public class OptionTests
    {
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
        public void ToOption_returns_none_for_null()
        {
            int? nullable = null;
            nullable.ToOption().Should().BeNone();
        }

        [Fact]
        public void ToOption_returns_some_for_non_null()
        {
            int? nullable = 42;
            nullable.ToOption().Should().BeSome(42);
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
    }
}
