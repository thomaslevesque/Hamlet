using System;
using System.Collections.Generic;

namespace Mayby
{
    public struct Option<T> : IEquatable<Option<T>>
    {
        public Option(T value)
        {
            _value = value;
            HasValue = true;
        }

        private readonly T _value;
        public T Value => HasValue ? _value : throw new InvalidOperationException("The option doesn't have a value");

        public bool HasValue { get; }

        public T GetValueOrDefault(T defaultValue = default) => HasValue ? _value : defaultValue;

        public bool TryGetValue(out T value)
        {
            value = _value;
            return HasValue;
        }

        public static implicit operator Option<T>(T value) => new Option<T>(value);

        public static implicit operator Option<T>(NoneOption value) => default;

        public bool Equals(Option<T> other)
        {
            if (HasValue)
            {
                return other.HasValue && EqualityComparer<T>.Default.Equals(other.Value, Value);
            }
            else
            {
                return !other.HasValue;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Option<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HasValue
                ? _value?.GetHashCode() ?? 0
                : 0;
        }

        public override string ToString()
        {
            return HasValue
                ? $"Some {ValueAsString(_value)}"
                : "None";

            string ValueAsString(T value)
            {
                if (value == null) return "null";
                if (value is string s) return $"\"{s.Replace("\"", "\\\"")}\"";
                return value.ToString();
            }
        }

        public Option<T> Filter(Func<T, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (HasValue && predicate(_value))
                return this;
            return Option.None<T>();
        }

        public Option<U> Map<U>(Func<T, U> mapping)
        {
            if (mapping == null)
                throw new ArgumentNullException(nameof(mapping));

            if (HasValue)
                return Option.Some(mapping(_value));

            return Option.None<U>();
        }

        public Option<U> Bind<U>(Func<T, Option<U>> binder)
        {
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            if (HasValue)
                return binder(_value);

            return Option.None<U>();
        }
    }
}
