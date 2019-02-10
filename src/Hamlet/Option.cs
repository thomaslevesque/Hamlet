namespace Hamlet
{
    public static class Option
    {
        public static NoneOption None() => default(NoneOption);
        public static Option<T> None<T>() => default(Option<T>);
        public static Option<T> Some<T>(T value) => new Option<T>(value);
    }
}
