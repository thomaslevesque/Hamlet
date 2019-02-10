using Mayby;

namespace System.Linq
{
    public static class LinqOptionExtensions
    {
        public static Option<T> Where<T>(this Option<T> option, Func<T, bool> predicate) =>
            option.Filter(predicate);

        public static Option<U> Select<T, U>(this Option<T> option, Func<T, U> projection) =>
            option.Map(projection);

        public static Option<U> SelectMany<T, U>(this Option<T> option, Func<T, Option<U>> projection) =>
            option.Bind(projection);

        public static Option<V> SelectMany<T, U, V>(this Option<T> option, Func<T, Option<U>> projection, Func<T, U, V> resultProjection)
        {
            if (projection == null)
                throw new ArgumentNullException(nameof(projection));

            if (resultProjection == null)
                throw new ArgumentNullException(nameof(resultProjection));

            return option.Bind(x => projection(x).Map(u => resultProjection(x, u)));
        }
    }
}
