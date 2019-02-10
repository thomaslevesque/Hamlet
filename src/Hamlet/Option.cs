namespace Hamlet
{
    public static class Option
    {
        public static NoneOption None() => default(NoneOption);
        public static Option<T> None<T>() => default(Option<T>);
        public static Option<T> Some<T>(T value) => new Option<T>(value);

        public static T? ToNullable<T>(this Option<T> option)
            where T : struct
        {
            return option.IsSome
                ? option.Value
                : default(T?);
        }

        public static Option<T> ToOption<T>(this T? nullable)
            where T : struct
        {
            return nullable.HasValue
                ? Some(nullable.Value)
                : None<T>();
        }
    }
}
