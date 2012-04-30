using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Phone7.Fx.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Convert an IEnumerable to an observable collection.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        //public static ObservableCollection<TSource> ToObservableCollection<TSource>(this  IEnumerable<TSource> source)
        //{
        //    ObservableCollection<TSource> target = new ObservableCollection<TSource>();

        //    using (IEnumerator<TSource> enumerator = source.GetEnumerator())
        //    {
        //        while (enumerator.MoveNext())
        //        {
        //            target.Add(enumerator.Current);
        //        }
        //    }
        //    return target;
        //}
    }
}