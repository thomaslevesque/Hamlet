using System;

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
        /// Converts a <see cref="Nullable{T}"/> to an <see cref="Option{T}"/>, based on whether the nullable has a value.
        /// </summary>
        /// <typeparam name="T">The type of the option's value.</typeparam>
        /// <param name="nullable">The nullabe to convert.</param>
        /// <returns>A <c>Some</c> <see cref="Option{T}"/> with the nullable's value, if any; otherwise, <c>None</c>.</returns>
        public static Option<T> ToOption<T>(this T? nullable)
            where T : struct
        {
            return nullable.HasValue
                ? Some(nullable.Value)
                : None<T>();
        }
    }
}
