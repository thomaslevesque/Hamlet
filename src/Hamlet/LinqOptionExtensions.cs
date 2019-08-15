using Hamlet;

namespace System.Linq
{
    /// <summary>
    /// Provides Linq extension methods for options.
    /// </summary>
    public static class LinqOptionExtensions
    {
        /// <summary>
        /// Filters an option. Same as <see cref="Option{T}.Filter"/>, but usable with the Linq query syntax.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="option">The option on which to apply a filter.</param>
        /// <param name="predicate">The filter predicate.</param>
        /// <returns>A filtered option.</returns>
        public static Option<T> Where<T>(this Option<T> option, Func<T, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return option.Filter(predicate);
        }

        /// <summary>
        /// Maps an option. Same as <see cref="Option{T}.Map{U}"/>, but usable with the Linq query syntax.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <typeparam name="U">The type of the mapped option's value.</typeparam>
        /// <param name="option">The option on which to apply a mapping.</param>
        /// <param name="selector">The mapping function.</param>
        /// <returns>A mapped option.</returns>
        public static Option<U> Select<T, U>(this Option<T> option, Func<T, U> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return option.Map(selector);
        }

        /// <summary>
        /// Binds an option. Same as <see cref="Option{T}.Bind{U}"/>, but usable with the Linq query syntax.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <typeparam name="U">The type of the bound option's value.</typeparam>
        /// <param name="option">The option on which to apply a binder.</param>
        /// <param name="selector">The binder function.</param>
        /// <returns>The result of binding the option.</returns>
        public static Option<U> SelectMany<T, U>(this Option<T> option, Func<T, Option<U>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return option.Bind(selector);
        }

        /// <summary>
        /// Binds an option and applies a mapping on the result. Used in the Linq query syntax when there are multiple <c>from</c> clauses.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <typeparam name="U">The type of the bound option's value.</typeparam>
        /// <typeparam name="V">The type of the mapping result's value.</typeparam>
        /// <param name="option">The option on which to apply a binder and a mapping.</param>
        /// <param name="optionSelector">The binder function.</param>
        /// <param name="resultSelector">The result mapping function.</param>
        /// <returns>The result of binding the option and applying the result mapping.</returns>
        public static Option<V> SelectMany<T, U, V>(this Option<T> option, Func<T, Option<U>> optionSelector, Func<T, U, V> resultSelector)
        {
            if (optionSelector == null)
                throw new ArgumentNullException(nameof(optionSelector));

            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return option.BindMap(optionSelector, resultSelector);
        }
    }
}
