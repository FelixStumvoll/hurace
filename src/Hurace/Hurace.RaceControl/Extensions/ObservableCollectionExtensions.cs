using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hurace.RaceControl.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> enumerable)
        {
            foreach (var obj in enumerable) collection.Add(obj);
        }

        public static void Repopulate<T>(this ObservableCollection<T> collection, IEnumerable<T> enumerable)
        {
            collection.Clear();
            collection.AddRange(enumerable);
        }
    }
}