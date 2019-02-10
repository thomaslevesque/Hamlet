using Mayby;

namespace System.Linq
{
    public static class LinqOptionExtensions
    {
        public static Option<T> Where<T>(this Option<T> option, Func<T, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return option.Filter(predicate);
        }

        public static Option<U> Select<T, U>(this Option<T> option, Func<T, U> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return option.Map(selector);
        }

        public static Option<U> SelectMany<T, U>(this Option<T> option, Func<T, Option<U>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return option.Bind(selector);
        }

        public static Option<V> SelectMany<T, U, V>(this Option<T> option, Func<T, Option<U>> optionSelector, Func<T, U, V> resultSelector)
        {
            if (optionSelector == null)
                throw new ArgumentNullException(nameof(optionSelector));

            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return option.Bind(x => optionSelector(x).Map(u => resultSelector(x, u)));
        }
    }
}
