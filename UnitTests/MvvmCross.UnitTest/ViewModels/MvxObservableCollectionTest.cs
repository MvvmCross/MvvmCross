// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Tests;
using MvvmCross.ViewModels;

namespace MvvmCross.UnitTest.ViewModels
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
    public class MvxObservableCollectionTest
    {
        readonly NavigationTestFixture _fixture;

        public MvxObservableCollectionTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadAsyncDispatcher>(new DummyDispatcher());
        }

        public class DummyDispatcher : IMvxMainThreadAsyncDispatcher
        {
            public bool IsOnMainThread => true;

            public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
            {
                action?.Invoke();
                return Task.CompletedTask;
            }

            public Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true)
            {
                return action?.Invoke();
            }
        }

        [Test]
        public async Task AddRangeSuppressesChangesTest()
        {
            var collection = new MvxObservableCollection<string>();
            var invokeCount = 0;
            collection.CollectionChanged += (s, e) =>
            {
                invokeCount++;
            };

            collection.Add("Foo");

            await Assert.That(invokeCount).IsEqualTo(1);
            await Assert.That(collection).Contains("Foo");

            var newItems = new[] { "Bar", "Baz", "Herp", "Derp" };
            collection.AddRange(newItems);

            await Assert.That(invokeCount).IsEqualTo(2);

            foreach (var item in newItems)
                await Assert.That(collection).Contains(item);
        }

        [Test]
        public async Task AddRangeIsAddActionTest()
        {
            var collection = new MvxObservableCollection<string>();
            var events = new List<NotifyCollectionChangedEventArgs>();
            collection.CollectionChanged += (s, e) => events.Add(e);
            collection.AddRange(new[] { "Bar", "Baz", "Herp", "Derp" });
            var change = await Assert.That(events).HasSingleItem();
            await Assert.That(change.Action).IsEqualTo(NotifyCollectionChangedAction.Add);
        }

        [Test]
        public async Task ValidateStartingIndexOnAddRangeTest()
        {
            var collection = new MvxObservableCollection<string>();
            var newItems = new[] { "Bar", "Baz", "Herp", "Derp" };

            var events = new List<NotifyCollectionChangedEventArgs>();
            collection.CollectionChanged += (s, e) => events.Add(e);

            collection.AddRange(newItems);
            var firstChange = await Assert.That(events).HasSingleItem();
            await Assert.That(firstChange.NewStartingIndex).IsEqualTo(0);
            await Assert.That(firstChange.NewItems.Count).IsEqualTo(newItems.Length);
            await Assert.That(firstChange.OldItems).IsNull();
            var newStartIndex = collection.Count;

            events.Clear();
            collection.AddRange(newItems);
            var secondChange = await Assert.That(events).HasSingleItem();
            await Assert.That(secondChange.NewStartingIndex).IsEqualTo(newStartIndex);
            await Assert.That(secondChange.NewItems.Count).IsEqualTo(newItems.Length);
        }

        [Test]
        public void AddRangeThrowsArgumentNullExceptionOnNullInput()
        {
            var collection = new MvxObservableCollection<string>();
            Assert.Throws<ArgumentNullException>(() => collection.AddRange(null));
        }

        [Test]
        [Arguments(-1, 0, new[] { "foo" })]
        [Arguments(0, 4, new[] { "foo", "bar", "baz" })]
        [Arguments(0, 0, new[] { "foo" })]
        [Arguments(0, -1, new[] { "foo" })]
        public void RemoveRangeThrowsArgumentOutOfRangeTest(int start, int count, string[] items)
        {
            var collection = new MvxObservableCollection<string>(items);

            Assert.Throws<ArgumentOutOfRangeException>(() => collection.RemoveRange(start, count));
        }

        [Test]
        public void RemoveItemThrowsArgumentNullTest()
        {
            var collection = new MvxObservableCollection<string>(new[] { "foo" });

            Assert.Throws<ArgumentNullException>(() => collection.RemoveItems(null));
        }
    }
}
