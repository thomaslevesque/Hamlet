using System;
using System.Collections.Generic;

namespace Hamlet
{
    /// <summary>
    /// Provides basic operations on <see cref="Option{T}"/>.
    /// </summary>
    public static class Option
    {
        /// <summary>
        /// Returns a <c>Some</c> <see cref="Option{T}"/> with the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="value">The value of the option to create.</param>
        /// <returns>A <c>Some</c> <see cref="Option{T}"/> with the specified value.</returns>
        public static Option<T> Some<T>(T value) => new Option<T>(value);

        /// <summary>
        /// Returns a <c>None</c> <see cref="Option{T}"/> of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <returns>A <c>None</c> <see cref="Option{T}"/>.</returns>
        public static Option<T> None<T>() => default;

        /// <summary>
        /// Returns an object representing a <c>None</c> option of any type, convertible to <see cref="Option{T}"/> for any <c>T</c>.
        /// </summary>
        /// <returns>An object representing a <c>None</c> option of any type.</returns>
        /// <remarks>This method is provided as a convenience to avoid having to specify the type when it's known from the context.</remarks>
        public static NoneOption None() => default;

        /// <summary>
        /// Evaluates whether the value contained in the option should remain, or be filtered out.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="option">The option to filter.</param>
        /// <param name="predicate">A function that evaluates whether the value contained in the option should remain, or be filtered out.</param>
        /// <returns>The option, if it is <c>Some</c> and its value matches the predicate; otherwise, <c>None</c>.</returns>
        public static Option<T> Filter<T>(this Option<T> option, Func<T, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (option.TryGetValue(out var value) && predicate(value))
                return option;

            return None<T>();
        }

        /// <summary>
        /// Transforms an option value by using a specified mapping function.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <typeparam name="U">The type of the mapping result.</typeparam>
        /// <param name="option">The option to map.</param>
        /// <param name="mapping">A function to apply to the option value.</param>
        /// <returns>A <c>Some</c> option with the mapping result, if the option is <c>Some</c>; otherwise, <c>None</c>.</returns>
        public static Option<U> Map<T, U>(this Option<T> option, Func<T, U> mapping)
        {
            if (mapping == null)
                throw new ArgumentNullException(nameof(mapping));

            if (option.TryGetValue(out var value))
                return Some(mapping(value));

            return None<U>();
        }

        /// <summary>
        /// Invokes a function on an optional value that itself yields an option.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <typeparam name="U">The type of the binding result's value.</typeparam>
        /// <param name="option">The option to bind.</param>
        /// <param name="binder">A function that takes the value of type <c>T</c> from the option and transforms it into an option containing a value of type <c>U</c>.</param>
        /// <returns>An option with the result of the binder, if the option is <c>Some</c>; otherwise, <c>None</c>.</returns>
        public static Option<U> Bind<T, U>(this Option<T> option, Func<T, Option<U>> binder)
        {
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            if (option.TryGetValue(out var value))
                return binder(value);

            return None<U>();
        }

        /// <summary>
        /// Invokes a function on an optional value that itself yields an option, then applies a mapping on the original value and the binding's result.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <typeparam name="U">The type of the binding result's value.</typeparam>
        /// <typeparam name="V">The type of the mapping result's value.</typeparam>
        /// <param name="option">The option to bind.</param>
        /// <param name="binder">A function that takes the value of type <c>T</c> from the option and transforms it into an option containing a value of type <c>U</c>.</param>
        /// <param name="mapping">A function that takes the value of type <c>T</c> from the option and the value of type <c>U</c> from the binding's result, and returns a value of type <c>V</c>.</param>
        /// <returns>An option with the result of the binding and mapping, if the option and the binding's result are both <c>Some</c>; otherwise, <c>None</c>.</returns>
        public static Option<V> BindMap<T, U, V>(this Option<T> option, Func<T, Option<U>> binder, Func<T, U, V> mapping)
        {
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            if (mapping == null)
                throw new ArgumentNullException(nameof(mapping));

            return option.Bind(x => binder(x).Map(u => mapping(x, u)));
        }

        /// <summary>
        /// Invokes one of the specified functions, depending on whether the option is <c>Some</c> or <c>None</c>.
        /// </summary>
        /// <typeparam name="U">The type of the mapping result.</typeparam>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="option">The option to match.</param>
        /// <param name="some">A mapping function invoked on the value if the option is <c>Some</c>.</param>
        /// <param name="none">A function invoked if the option is <c>None</c>.</param>
        /// <returns>The result of one of the specified functions, depending on whether the option is <c>Some</c> or <c>None</c>.</returns>
        public static U Match<T, U>(this Option<T> option, Func<T, U> some, Func<U> none)
        {
            if (some == null)
                throw new ArgumentNullException(nameof(some));
            if (none == null)
                throw new ArgumentNullException(nameof(none));

            return option.TryGetValue(out var value)
                ? some(value)
                : none();
        }

        /// <summary>
        /// Converts an <see cref="Option{T}"/> to a <see cref="Nullable{T}"/>, based on whether the option is <c>Some</c> or <c>None</c>.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="option">The option to convert.</param>
        /// <returns>A <see cref="Nullable{T}"/> with the option's value, if the option is <c>Some</c>; otherwise, <c>null</c>.</returns>
        public static T? ToNullable<T>(this Option<T> option)
            where T : struct
        {
            return option.IsSome
                ? option.Value
                : default(T?);
        }

        /// <summary>
        /// Returns the value of an <see cref="Option{T}"/> if the option is <c>Some</c>, or <c>null</c> if it's <c>None</c>.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="option">The option to get the value from.</param>
        /// <returns>The option's value if it's <c>Some</c>, or <c>null</c> if it's <c>None</c>.</returns>
        public static T? GetValueOrNull<T>(this Option<T> option)
            where T : class
        {
            return option.IsSome
                ? option.Value
                : null;
        }

        /// <summary>
        /// Returns the value of an <see cref="Option{T}"/> if the option is <c>Some</c>, or <c>default(T)</c> if it's <c>None</c>.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="option">The option to get the value from.</param>
        /// <returns>The option's value if it's <c>Some</c>, or <c>default(T)</c> if it's <c>None</c>.</returns>
        public static T GetValueOrDefault<T>(this Option<T> option)
            where T : struct
        {
            return option.IsSome
                ? option.Value
                : default;
        }

        /// <summary>
        /// Returns the value of an <see cref="Option{T}"/> if the option is <c>Some</c>, or <c>defaultValue</c> if it's <c>None</c>.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="option">The option to get the value from.</param>
        /// <param name="defaultValue">The value to return if the option is <c>None</c>.</param>
        /// <returns>The option's value if it's <c>Some</c>, or <c>defaultValue</c> if it's <c>None</c>.</returns>
        public static T GetValueOr<T>(this Option<T> option, T defaultValue)
            where T : struct
        {
            return option.IsSome
                ? option.Value
                : defaultValue;
        }

        /// <summary>
        /// Converts a <see cref="Nullable{T}"/> to an <see cref="Option{T}"/>, based on whether the nullable has a value.
        /// </summary>
        /// <typeparam name="T">The type of the nullable's value.</typeparam>
        /// <param name="nullable">The nullable to convert.</param>
        /// <returns>A <c>Some</c> <see cref="Option{T}"/> with the nullable's value, if any; otherwise, <c>None</c>.</returns>
        public static Option<T> SomeIfNotNull<T>(this T? nullable)
            where T : struct
        {
            return nullable.HasValue
                ? Some(nullable.Value)
                : None<T>();
        }

        /// <summary>
        /// Converts a <c>T</c> to an <see cref="Option{T}"/>, based on whether it's null or not.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <c>Some</c> <see cref="Option{T}"/> with the value, if it's not null; otherwise, <c>None</c>.</returns>
        public static Option<T> SomeIfNotNull<T>(this T? value)
            where T : class
        {
            return value != null
                ? Some(value)
                : None<T>();
        }

        /// <summary>
        /// Converts the option to an array of length 0 or 1.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="option">The option to convert.</param>
        /// <returns>An array containing the option's value, if the option is <c>Some</c>; otherwise, an empty array.</returns>
        public static T[] ToArray<T>(this Option<T> option)
        {
            return option.Match(value => new[] { value }, Array.Empty<T>);
        }

        /// <summary>
        /// Converts the option to an list of length 0 or 1.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="option">The option to convert.</param>
        /// <returns>A list containing the option's value, if the option is <c>Some</c>; otherwise, an empty list.</returns>
        public static List<T> ToList<T>(this Option<T> option)
        {
            return option.Match(value => new List<T> { value }, () => new List<T>());
        }

        /// <summary>
        /// Executes an action of the option's value, if it exists.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="option">The option on which to execute an action.</param>
        /// <param name="action">The action to execute on the option's value.</param>
        public static void Do<T>(this Option<T> option, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (option.TryGetValue(out var value))
                action(value);
        }
    }
}
