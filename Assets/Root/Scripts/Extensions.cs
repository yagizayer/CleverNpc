// Extensions.cs

using System.Collections.Generic;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts
{
    public static class Extensions
    {
        public static List<T> Clone<T>(this IEnumerable<T> list) => new(list);
        
        public static IPassableData ToPassableData<T>(this T value) => new GenericPassableData<T>(value);
    }
}