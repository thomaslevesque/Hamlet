#if !FEATURE_ARRAY_EMPTY
namespace Hamlet.Compatibility
{
    internal static class ArrayEx
    {
        public static T[] Empty<T>() => ArrayCache<T>.Empty;

        private static class ArrayCache<T>
        {
            public static T[] Empty = new T[0];
        }
    }
}
#endif