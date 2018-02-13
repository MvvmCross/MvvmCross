using System;
using System.Collections.Specialized;
using MvvmCross.Base;
using MvvmCross.Test;
using MvvmCross.ViewModels;
using Xunit;

namespace MvvmCross.UnitTest.ViewModels
{
    [Collection("MvxTest")]
    public class MvxObservableCollectionTest
    {
        MvxTestFixture _fixture;

        public MvxObservableCollectionTest(MvxTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(new DummyDispatcher());
        }

        public class DummyDispatcher : MvxSingleton<IMvxMainThreadDispatcher>, IMvxMainThreadDispatcher
        {
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
    }
}
