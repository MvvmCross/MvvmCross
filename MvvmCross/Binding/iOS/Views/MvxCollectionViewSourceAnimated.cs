using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Platform;
using UIKit;

namespace MvvmCross.Binding.iOS.Views
{
    public class MvxCollectionViewSourceAnimated : MvxCollectionViewSource
    {
        private readonly object collectionChangedLock = new object();
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

        public MvxCollectionViewSourceAnimated(UICollectionView collectionView) : base(collectionView)
        {
        }

        public MvxCollectionViewSourceAnimated(UICollectionView collectionView, NSString defaultCellIdentifier) : base(collectionView, defaultCellIdentifier)
        {
        }

        protected override void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
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
            Mvx.Trace($"CollectionChanged received action:{args.Action} newItems:{args.NewItems?.Count} oldItems:{args.OldItems?.Count} itemsSourceCount:{itemsSource.Count()}");
            await existingTask;
            Mvx.Trace($"CollectionChanged starting action:{args.Action}");
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
                    var oldCount = args.OldItems.Count;
                    var newCount = args.NewItems.Count;
                    var indexes = new NSIndexPath[oldCount + newCount];

                    var startIndex = args.OldStartingIndex;
                    for (var i = 0; i < oldCount; i++)
                        indexes[i] = NSIndexPath.FromRowSection(startIndex + i, 0);
                    startIndex = args.NewStartingIndex;
                    for (var i = oldCount; i < oldCount + newCount; i++)
                        indexes[i] = NSIndexPath.FromRowSection(startIndex + i, 0);

                    CollectionView.ReloadItems(indexes);
                });
            }
            else if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                await CollectionView.PerformBatchUpdatesAsync(() =>
                {
                    int oldStartingIndex = args.OldStartingIndex;
                    var indexPaths = new NSIndexPath[args.OldItems.Count];
                    for (int index = 0; index < indexPaths.Length; ++index)
                        indexPaths[index] = NSIndexPath.FromRowSection((oldStartingIndex + index), 0);
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
                        indexPaths[index] = NSIndexPath.FromRowSection((newStartingIndex + index), 0);
                    CollectionView.InsertItems(indexPaths);
                });
            }
            else
            {
                await CollectionView.PerformBatchUpdatesAsync(() => { });
                ReloadData();
            }

            itemsSourceBeforeAnimation = null;
            Mvx.Trace($"CollectionChanged done action:{args.Action} newItems:{args.NewItems?.Count} oldItems:{args.OldItems?.Count}");
        }
    }
}