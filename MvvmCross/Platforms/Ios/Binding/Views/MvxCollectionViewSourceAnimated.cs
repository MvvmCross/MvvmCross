// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.Extensions;
using MvvmCross.Logging;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Views
{
    public class MvxCollectionViewSourceAnimated : MvxCollectionViewSource
    {
        private readonly object collectionChangedLock = new object();
        private readonly ILogger<MvxCollectionViewSourceAnimated> _logger;

        private Task runningChangeTask = Task.FromResult(true);

        /// <summary>
        /// UICollectionView animations must be synchronized: the itemsSource content must not change until the animation ends.
        /// As we can not assume that the backing collection is thread safe and waits for us, we keep a copy of its items,
        /// hoping they won't be disposed explicitely before the next UICollectionView animation ends.
        /// The best would be a new NotifyCollectionChangedEventArgs with support for multiple changes, and a new async (awaitable) event for changes.
        /// </summary>
        private IEnumerable itemsSourceBeforeAnimation;

        /// <summary>
        /// When a collectionchanged event is received, if the number of changed items is over MaxAnimatedItems, the collection will not animate changes.
        /// This is a guard to prevent creating a large array of changed indexes, when the collection is large.
        /// </summary>
        public int MaxAnimatedItems { get; set; } = 10;

        public MvxCollectionViewSourceAnimated(UICollectionView collectionView)
            : base(collectionView)
        {
            _logger = MvxLogHost.GetLog<MvxCollectionViewSourceAnimated>();
        }

        public MvxCollectionViewSourceAnimated(UICollectionView collectionView, NSString defaultCellIdentifier)
            : base(collectionView, defaultCellIdentifier)
        {
            _logger = MvxLogHost.GetLog<MvxCollectionViewSourceAnimated>();
        }

        protected override void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (!NSThread.IsMain)
            {
                BeginInvokeOnMainThread(() => CollectionChangedOnCollectionChanged(sender, args));
                return;
            }

            var itemsSource = (ItemsSource as IEnumerable<object>)?.ToList();
            if (itemsSource == null)
                throw new ArgumentException("ItemsSource must be convertible to IEnumerable<object>, as this code needs to take a snapshot of the list in order to be thread safe for the ios animations");

            lock (collectionChangedLock)
            {
                var existingTask = runningChangeTask;
                runningChangeTask = CollectionChangedOnCollectionChangedAsync(args, existingTask, itemsSource);
            }
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            var itemsSource = itemsSourceBeforeAnimation ?? ItemsSource;
            return itemsSource?.ElementAt(indexPath.Row);
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            var itemsSource = itemsSourceBeforeAnimation ?? ItemsSource;
            return itemsSource?.Count() ?? 0;
        }

        private async Task CollectionChangedOnCollectionChangedAsync(NotifyCollectionChangedEventArgs args, Task existingTask, IEnumerable itemsSource)
        {
            _logger?.LogTrace(
                "CollectionChanged received action:{action} newItems:{newItemsCount} oldItems:{oldItemsCount} itemsSourceCount:{itemsSourceCount}",
                args.Action, args.NewItems?.Count ?? 0, args.OldItems?.Count ?? 0, itemsSource.Count());
            await existingTask;
            _logger?.LogTrace("CollectionChanged starting action:{action}", args.Action);
            itemsSourceBeforeAnimation = itemsSource;

            if (args.NewItems?.Count > MaxAnimatedItems || args.OldItems?.Count > MaxAnimatedItems)
            {
                //No animation change
                await CollectionView.PerformBatchUpdatesAsync(() => { });
                ReloadData();
            }
            else if (args.Action == NotifyCollectionChangedAction.Move)
            {
                await CollectionView.PerformBatchUpdatesAsync(() =>
                {
                    if (args.NewItems.Count != 1 && args.OldItems.Count != 1)
                    {
                        _logger?.LogTrace("CollectionChanged {action} action called with more than one movement. All data will be reloaded", args.Action);
                        CollectionView.ReloadData();
                        return;
                    }

                    var oldIndexPath = NSIndexPath.FromRowSection(args.OldStartingIndex, 0);
                    var newIndexPath = NSIndexPath.FromRowSection(args.NewStartingIndex, 0);
                    CollectionView.MoveItem(oldIndexPath, newIndexPath);
                });
            }
            else if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                await CollectionView.PerformBatchUpdatesAsync(() =>
                {
                    int oldStartingIndex = args.OldStartingIndex;
                    var indexPaths = new NSIndexPath[args.OldItems.Count];
                    for (int index = 0; index < indexPaths.Length; ++index)
                        indexPaths[index] = NSIndexPath.FromRowSection(oldStartingIndex + index, 0);
                    CollectionView.DeleteItems(indexPaths);
                });
            }
            else if (args.Action == NotifyCollectionChangedAction.Add)
            {
                await CollectionView.PerformBatchUpdatesAsync(() =>
                {
                    int newStartingIndex = args.NewStartingIndex;
                    var indexPaths = new NSIndexPath[args.NewItems.Count];
                    for (int index = 0; index < indexPaths.Length; ++index)
                        indexPaths[index] = NSIndexPath.FromRowSection(newStartingIndex + index, 0);
                    CollectionView.InsertItems(indexPaths);
                });
            }
            else
            {
                await CollectionView.PerformBatchUpdatesAsync(() => { });
                ReloadData();
            }

            itemsSourceBeforeAnimation = null;
            _logger?.LogTrace("CollectionChanged done action:{action} newItems:{newItemsCount} oldItems:{oldItemsCount}",
                args.Action, args.NewItems?.Count ?? 0, args.OldItems?.Count ?? 0);
        }
    }
}
