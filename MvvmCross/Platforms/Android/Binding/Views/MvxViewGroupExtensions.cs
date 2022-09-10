// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Specialized;
using Android.Views;
using Android.Widget;

namespace MvvmCross.Platforms.Android.Binding.Views
{
    public static class MvxViewGroupExtensions
    {
        public static void UpdateDataSetFromChange<T>(this T viewGroup, object sender,
                                                      NotifyCollectionChangedEventArgs eventArgs)
            where T : ViewGroup, IMvxWithChangeAdapter
        {
            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    viewGroup.Add(viewGroup.Adapter, eventArgs.NewStartingIndex, eventArgs.NewItems.Count);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    viewGroup.Remove(viewGroup.Adapter, eventArgs.OldStartingIndex, eventArgs.OldItems.Count);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (eventArgs.NewItems.Count != eventArgs.OldItems.Count)
                    {
                        viewGroup.Refill(viewGroup.Adapter);
                    }
                    else
                    {
                        viewGroup.Replace(viewGroup.Adapter, eventArgs.NewStartingIndex, eventArgs.NewItems.Count);
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    // move is not implemented - so we call Refill instead
                    viewGroup.Refill(viewGroup.Adapter);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    viewGroup.Refill(viewGroup.Adapter);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void Refill(this ViewGroup viewGroup, IAdapter adapter)
        {
            viewGroup.RemoveAllViews();
            var count = adapter.Count;
            for (var i = 0; i < count; i++)
            {
                viewGroup.AddView(adapter.GetView(i, null, viewGroup));
            }
        }

        private static void Add(this ViewGroup viewGroup, IAdapter adapter, int insertionIndex, int count)
        {
            for (var i = 0; i < count; i++)
            {
                viewGroup.AddView(adapter.GetView(insertionIndex + i, null, viewGroup), insertionIndex + i);
            }
        }

        private static void Remove(this ViewGroup viewGroup, IAdapter adapter, int removalIndex, int count)
        {
            for (var i = 0; i < count; i++)
            {
                viewGroup.RemoveViewAt(removalIndex + i);
            }
        }

        private static void Replace(this ViewGroup viewGroup, IAdapter adapter, int startIndex, int count)
        {
            for (var i = 0; i < count; i++)
            {
                viewGroup.RemoveViewAt(startIndex + i);
                viewGroup.AddView(adapter.GetView(startIndex + i, null, viewGroup), startIndex + i);
            }
        }
    }
}
