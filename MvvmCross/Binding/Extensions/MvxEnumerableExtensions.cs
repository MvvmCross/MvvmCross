// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace MvvmCross.Binding.Extensions
{
    public static class MvxEnumerableExtensions
    {
        public static int Count(this IEnumerable enumerable)
        {
            if (enumerable == null)
                return 0;

            var itemsList = enumerable as ICollection;
            if (itemsList != null)
            {
                return itemsList.Count;
            }

            var enumerator = enumerable.GetEnumerator();
            var count = 0;
            while (enumerator.MoveNext())
            {
                count++;
            }

            return count;
        }

        public static int GetPosition(this IEnumerable items, object item)
        {
            if (items == null)
            {
                return -1;
            }

            var itemsList = items as IList;
            if (itemsList != null)
            {
                {
                    return itemsList.IndexOf(item);
                }
            }

            var enumerator = items.GetEnumerator();
            for (var i = 0; ; i++)
            {
                if (!enumerator.MoveNext())
                {
                    return -1;
                }

                if (enumerator.Current == null)
                {
                    if (item == null)
                        return i;
                }
                // Note: do *not* use == here - see https://github.com/slodge/MvvmCross/issues/309
                else if (enumerator.Current.Equals(item))
                {
                    return i;
                }
            }
        }

        public static object ElementAt(this IEnumerable items, int position)
        {
            if (items == null)
                return null;

            var itemsList = items as IList;
            if (itemsList != null)
            {
                return itemsList[position];
            }

            var enumerator = items.GetEnumerator();
            for (var i = 0; i <= position; i++)
            {
                enumerator.MoveNext();
            }

            return enumerator.Current;
        }

        public static IEnumerable Filter(this IEnumerable items, Func<object, bool> predicate)
        {
            if (items == null)
                return null;

            var matchList = new List<object>();
            foreach (var item in items)
            {
                var match = predicate(item);
                if (match)
                    matchList.Add(item);
            }

            if (matchList.Count == 0)
                return null;

            return matchList;
        }
    }
}
