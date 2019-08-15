using Hamlet;
using System.Collections.Generic;

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

        /// <summary>
        /// Returns the values of the options that are <c>Some</c>.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="source">The input option list.</param>
        /// <returns>The values of the options that are <c>Some</c>.</returns>
        public static IEnumerable<T> Choose<T>(this IEnumerable<Option<T>> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.ChooseImpl();
        }

        private static IEnumerable<T> ChooseImpl<T>(this IEnumerable<Option<T>> source)
        {
            foreach (var option in source)
            {
                if (option.TryGetValue(out var value))
                    yield return value;
            }
        }

        /// <summary>
        /// Maps the elements from the list to options, and returns the values of the options that are <c>Some</c>.
        /// </summary>
        /// <typeparam name="T">The type of the list's elements.</typeparam>
        /// <typeparam name="U">The type of the projected option's value.</typeparam>
        /// <param name="source">The input list.</param>
        /// <param name="chooser">The function to generate options from the elements.</param>
        /// <returns>The values of the options that are <c>Some</c>.</returns>
        public static IEnumerable<U> Choose<T, U>(this IEnumerable<T> source, Func<T, Option<U>> chooser)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (chooser == null)
                throw new ArgumentNullException(nameof(chooser));

            return source.ChooseImpl(chooser);
        }

        private static IEnumerable<U> ChooseImpl<T, U>(this IEnumerable<T> source, Func<T, Option<U>> chooser)
        {
            foreach (var item in source)
            {
                var option = chooser(item);
                if (option.TryGetValue(out var value))
                    yield return value;
            }
        }

        /// <summary>
        /// Returns the value of the first option that is <c>Some</c>.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="source">The input option list.</param>
        /// <returns>The value of the first option that is <c>Some</c>.</returns>
        /// <exception cref="InvalidOperationException">The list doesn't contain any option that is <c>Some</c>.</exception>
        public static T Pick<T>(this IEnumerable<Option<T>> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.ChooseImpl().First();
        }

        /// <summary>
        /// Maps the elements from the list to options, and returns the value of the first option that is <c>Some</c>.
        /// </summary>
        /// <typeparam name="T">The type of the list's elements.</typeparam>
        /// <typeparam name="U">The type of the projected option's value.</typeparam>
        /// <param name="source">The input list.</param>
        /// <param name="chooser">The function to generate options from the elements.</param>
        /// <returns>The value of the first option that is <c>Some</c>.</returns>
        /// <exception cref="InvalidOperationException">The list doesn't contain any element which generates an option that is <c>Some</c>.</exception>
        public static U Pick<T, U>(this IEnumerable<T> source, Func<T, Option<U>> chooser)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (chooser == null)
                throw new ArgumentNullException(nameof(chooser));

            return source.ChooseImpl(chooser).First();
        }

        /// <summary>
        /// Returns the first option that is <c>Some</c>, if any, or <c>None</c>.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="source">The input option list.</param>
        /// <returns>The first option that is <c>Some</c>, if any; otherwise, <c>None</c>.</returns>
        public static Option<T> TryPick<T>(this IEnumerable<Option<T>> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.FirstOrDefault(o => o.IsSome);
        }

        /// <summary>
        /// Maps the elements from the list to options, and returns the first option that is <c>Some</c>, if any, or <c>None</c>.
        /// </summary>
        /// <typeparam name="T">The type of the list's elements.</typeparam>
        /// <typeparam name="U">The type of the projected option's value.</typeparam>
        /// <param name="source">The input list.</param>
        /// <param name="chooser">The function to generate options from the elements.</param>
        /// <returns>The value of the first option that is <c>Some</c>, if any; otherwise, <c>None</c>.</returns>
        public static Option<U> TryPick<T, U>(this IEnumerable<T> source, Func<T, Option<U>> chooser)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (chooser == null)
                throw new ArgumentNullException(nameof(chooser));

            return source.Select(chooser).FirstOrDefault(o => o.IsSome);
        }
    }
}
