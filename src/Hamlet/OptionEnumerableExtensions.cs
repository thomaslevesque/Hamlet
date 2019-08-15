using System;
using System.Collections.Generic;
using System.Linq;

namespace Hamlet
{
    /// <summary>
    /// Provides extension methods for sequences of options.
    /// </summary>
    public static class OptionEnumerableExtensions
    {
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

        /// <summary>
        /// Returns the first element for which the given predicate returns true, or <c>None</c> if no such element exists.
        /// </summary>
        /// <typeparam name="T">The type of the list's elements.</typeparam>
        /// <param name="source">The input list.</param>
        /// <param name="predicate">A function that evaluates to a Boolean when given an item in the sequence.</param>
        /// <returns>A <c>Some</c> option with the first element for which the predicate returns true, if any; otherwise, <c>None</c>.</returns>
        public static Option<T> TryFind<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (var item in source)
            {
                if (predicate(item))
                    return Option.Some(item);
            }

            return Option.None<T>();
        }

        /// <summary>
        /// Returns the index of the first element for which the given predicate returns true, or <c>None</c> if no such element exists.
        /// </summary>
        /// <typeparam name="T">The type of the list's elements.</typeparam>
        /// <param name="source">The input list.</param>
        /// <param name="predicate">A function that evaluates to a Boolean when given an item in the sequence.</param>
        /// <returns>A <c>Some</c> option with the index of the first element for which the predicate returns true, if any; otherwise, <c>None</c>.</returns>
        public static Option<int> TryFindIndex<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            int index = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                    return Option.Some(index);
                index++;
            }

            return Option.None<int>();
        }
    }
}
