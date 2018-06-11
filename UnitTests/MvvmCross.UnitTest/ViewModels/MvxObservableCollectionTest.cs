// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Specialized;
using MvvmCross.Base;
using MvvmCross.Tests;
using MvvmCross.ViewModels;
using Xunit;

namespace MvvmCross.UnitTest.ViewModels
{
    [Collection("MvxTest")]
    public class MvxObservableCollectionTest
    {
        NavigationTestFixture _fixture;

        public MvxObservableCollectionTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearAll();
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(new DummyDispatcher());
        }

        public class DummyDispatcher : MvxSingleton<IMvxMainThreadDispatcher>, IMvxMainThreadDispatcher
        {
            public bool IsOnMainThread => true;

            public bool RequestMainThreadAction(Action action, bool maskExceptions = true)
            {
                action?.Invoke();
                return true;
            }
        }

        [Fact]
        public void AddRangeSuppressesChangesTest()
        {
            var collection = new MvxObservableCollection<string>();
            var invokeCount = 0;
            collection.CollectionChanged += (s, e) =>
            {
                invokeCount++;
            };

            collection.Add("Foo");

            Assert.Equal(1, invokeCount);
            Assert.Contains("Foo", collection);

            var newItems = new[] { "Bar", "Baz", "Herp", "Derp" };
            collection.AddRange(newItems);

            Assert.Equal(2, invokeCount);

            foreach (var item in newItems)
                Assert.Contains(item, collection);
        }

        [Fact]
        public void AddRangeIsAddActionTest()
        {
            var collection = new MvxObservableCollection<string>();
            collection.CollectionChanged += (s, e) => {
                Assert.Equal(NotifyCollectionChangedAction.Add, e.Action);
            };
            collection.AddRange(new[] { "Bar", "Baz", "Herp", "Derp" });
        }

        [Fact]
        public void ValidateStartingIndexOnAddRangeTest()
        {
            var collection = new MvxObservableCollection<string>();
            var newItems = new[] { "Bar", "Baz", "Herp", "Derp" };

            NotifyCollectionChangedEventHandler handler = (s, a) =>
            {
                Assert.Equal(0, a.NewStartingIndex);
                Assert.Equal(newItems.Length, a.NewItems.Count);
                Assert.Null(a.OldItems);
            };

            collection.CollectionChanged += handler;

            collection.AddRange(newItems);
            var newStartIndex = collection.Count;

            collection.CollectionChanged -= handler;

            handler = (s, a) => {
                Assert.Equal(newStartIndex, a.NewStartingIndex);
                Assert.Equal(newItems.Length, a.NewItems.Count);
            };

            collection.CollectionChanged += handler;

            collection.AddRange(newItems);
        }

        [Fact]
        public void AddRangeThrowsArgumentNullExceptionOnNullInput()
        {
            var collection = new MvxObservableCollection<string>();
            Assert.Throws<ArgumentNullException>(() => collection.AddRange(null));
        }

        [Theory]
        [InlineData(-1, 0, new [] { "foo" })]
        [InlineData(0, 4, new[] { "foo", "bar", "baz" })]
        [InlineData(0, 0, new[] { "foo" })]
        [InlineData(0, -1, new[] { "foo" })]
        public void RemoveRangeThrowsArgumentOutOfRangeTest(int start, int count, string[] items)
        {
            var collection = new MvxObservableCollection<string>(items);

            Assert.Throws<ArgumentOutOfRangeException>(() => collection.RemoveRange(start, count));
        }

        [Fact]
        public void RemoveItemThrowsArgumentNullTest()
        {
            var collection = new MvxObservableCollection<string>(new[] { "foo" });

            Assert.Throws<ArgumentNullException>(() => collection.RemoveItems(null));
        }
    }
}
