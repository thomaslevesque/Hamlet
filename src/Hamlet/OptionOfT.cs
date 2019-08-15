using System;
using System.Collections.Generic;

namespace Hamlet
{
    /// <summary>
    /// Represents an optional value.
    /// </summary>
    /// <typeparam name="T">The type of value that can be contained in the option.</typeparam>
    public struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Option{T}"/> that contains the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public Option(T value)
        {
            _value = value;
            IsSome = true;
        }

        private readonly T _value;

        /// <summary>
        /// Returns the option's value, if any; otherwise, throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The option doesn't have a value.</exception>
        public T Value => IsSome ? _value : throw new InvalidOperationException("The option doesn't have a value");

        /// <summary>
        /// Indicates whether this option is <c>Some</c>, i.e. whether it has a value.
        /// </summary>
        public bool IsSome { get; }

        /// <summary>
        /// Indicates whether this option is <c>None</c>, i.e. whether it has no value.
        /// </summary>
        public bool IsNone => !IsSome;

        /// <summary>
        /// Attempts to get the option's value, if any, or a default value if the option has no value.
        /// </summary>
        /// <param name="defaultValue">The default value to return when the option has no value.</param>
        /// <returns>The option's value, if any, <c>defaultValue</c> otherwise.</returns>
        public T GetValueOrDefault(T defaultValue = default) => IsSome ? _value : defaultValue;

        /// <summary>
        /// Attempts to get the option's value, if any.
        /// </summary>
        /// <param name="value">This output parameter will receive the option's value, if any.</param>
        /// <returns><c>true</c> if a value was obtained, <c>false</c> otherwise.</returns>
        public bool TryGetValue(out T value)
        {
            value = _value;
            return IsSome;
        }

        /// <summary>
        /// Implicitly converts a value into a <c>Some</c> option.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static implicit operator Option<T>(T value) => new Option<T>(value);

        /// <summary>
        /// Implicitly converts a <see cref="NoneOption"/> into a <c>None</c> option.
        /// </summary>
        /// <param name="value">The <see cref="NoneOption"/> to convert.</param>
        public static implicit operator Option<T>(NoneOption value) => default;

        /// <summary>
        /// Determines wether two specified options are equal.
        /// </summary>
        /// <param name="a">The first option to compare.</param>
        /// <param name="b">The second option to compare.</param>
        /// <returns>true if the options are equal, false otherwise.</returns>
        /// <remarks>The equality semantics are the same as for <see cref="Equals(Option{T})"/></remarks>
        public static bool operator ==(Option<T> a, Option<T> b) => a.Equals(b);

        /// <summary>
        /// Determines wether two specified options are different.
        /// </summary>
        /// <param name="a">The first option to compare.</param>
        /// <param name="b">The second option to compare.</param>
        /// <returns>true if the options are different, false otherwise.</returns>
        /// <remarks>The equality semantics are the same as for <see cref="Equals(Option{T})"/></remarks>
        public static bool operator !=(Option<T> a, Option<T> b) => !(a == b);

        /// <summary>
        /// Determines wether the specified option and value are equal.
        /// </summary>
        /// <param name="option">The option to compare.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>true if <c>option</c> has a value equal to <c>value</c>, false otherwise.</returns>
        /// <remarks>Always returns false if the option is <c>None</c>.</remarks>
        public static bool operator ==(Option<T> option, T value) => option.Equals(value);

        /// <summary>
        /// Determines wether the specified option and value are different.
        /// </summary>
        /// <param name="option">The option to compare.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>true if <c>option</c> is <c>None</c> or has a value different from <c>value</c>, false otherwise.</returns>
        /// <remarks>Always returns true if the option is <c>None</c>.</remarks>
        public static bool operator !=(Option<T> option, T value) => !(option == value);

        /// <summary>
        /// Determines wether the specified value and option are equal.
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <param name="option">The option to compare.</param>
        /// <returns>true if <c>option</c> has a value equal to <c>value</c>, false otherwise.</returns>
        /// <remarks>Always returns false if the option is <c>None</c>.</remarks>
        public static bool operator ==(T value, Option<T> option) => option == value;

        /// <summary>
        /// Determines wether the specified value and option are different.
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <param name="option">The option to compare.</param>
        /// <returns>true if <c>option</c> is <c>None</c> or has a value different from <c>value</c>, false otherwise.</returns>
        /// <remarks>Always returns true if the option is <c>None</c>.</remarks>
        public static bool operator !=(T value, Option<T> option) => option != value;

        /// <summary>
        /// Determines whether the specified <see cref="Option{T}"/> is equal to the current option.
        /// </summary>
        /// <param name="other">The <see cref="Option{T}"/> to compare with the current option.</param>
        /// <returns><c>true</c> if the other option is equal to the current option, false otherwise.</returns>
        /// <remarks>
        /// Two options are considered equal if one of these conditions is met:
        /// <list type="bullet">
        ///   <item>
        ///     <description>Both options are <c>None</c></description>
        ///   </item>
        ///   <item>
        ///     <description>Both options are <c>Some</c> and there value are equal</description>
        ///   </item>
        /// </list>
        /// </remarks>
        public bool Equals(Option<T> other)
        {
            if (IsSome)
            {
                return other.IsSome && EqualityComparer<T>.Default.Equals(other.Value, Value);
            }
            else
            {
                return !other.IsSome;
            }
        }

        /// <summary>
        /// Determines whether the specified value is equal to the current option.
        /// </summary>
        /// <param name="value">The value to compare with the current option.</param>
        /// <returns>True if the current option is <c>Some</c> and has a value equal to <c>value</c>, false otherwise.</returns>
        public bool Equals(T value)
        {
            return IsSome && EqualityComparer<T>.Default.Equals(value, Value);
        }

        /// <inheritdoc cref="Object" />
        public override bool Equals(object obj)
        {
            return obj is Option<T> other && Equals(other);
        }

        /// <inheritdoc cref="Object" />
        public override int GetHashCode()
        {
            return IsSome
                ? _value?.GetHashCode() ?? 0
                : 0;
        }

        /// <inheritdoc cref="Object" />
        public override string ToString()
        {
            return IsSome
                ? $"Some {ValueAsString(_value)}"
                : "None";

            string ValueAsString(T value)
            {
                if (value == null) return "null";
                if (value is string s) return $"\"{s.Replace("\"", "\\\"")}\"";
                return value.ToString();
            }
        }
    }
}
