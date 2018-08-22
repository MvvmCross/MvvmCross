// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Navigation;
using Xunit;

namespace MvvmCross.UnitTest.Navigation
{
    public class NavigationCacheTests
    {
        [Theory]
        [InlineData("a", 1, 1)]
        [InlineData("a", -1, -1)]
        [InlineData("a", int.MaxValue, int.MaxValue)]
        [InlineData("a", int.MinValue, int.MinValue)]
        [InlineData("a", default(int), default(int))]
        [InlineData("a", 1337, 1337)]
        public void Test_IntsCanBeCached(string key, int value, int expected)
        {
            SimpleTest(new MvxNavigationCache(), key, value, expected);
        }

        [Theory]
        [InlineData("a", 1.0, 1.0)]
        [InlineData("a", -1.0, -1.0)]
        [InlineData("a", double.MaxValue, double.MaxValue)]
        [InlineData("a", double.MinValue, double.MinValue)]
        [InlineData("a", default(double), default(double))]
        [InlineData("a", 1337.0, 1337.0)]
        public void Test_DoublesCanBeCached(string key, double value, double expected)
        {
            SimpleTest(new MvxNavigationCache(), key, value, expected);
        }

        [Theory]
        [InlineData("a", "a", "a")]
        [InlineData("a", "", "")]
        [InlineData("a", null, null)]
        public void Test_StringsCanBeCached(string key, string value, string expected)
        {
            SimpleTest(new MvxNavigationCache(), key, value, expected);
        }

        private static void SimpleTest<T>(IMvxNavigationCache cache, string key, T value, T expected)
        {
            Assert.True(cache.AddValue(key, value));
            var addedValue = cache.GetValueOrDefault<T>(key);
            Assert.Equal(expected, addedValue);
        }

        [Fact]
        public void Test_MixedTypesCanBeCached()
        {
            var cache = new MvxNavigationCache();

            SimpleTest(cache, "a", 1, 1);
            SimpleTest(cache, "b", 1.0, 1.0);
            SimpleTest(cache, "c", 1.0f, 1.0f);
            SimpleTest(cache, "d", "hello", "hello");
            var obj = new TestObject();
            SimpleTest(cache, "e", obj, obj);
        }

        [Fact]
        public void Test_CacheCanClear()
        {
            var cache = new MvxNavigationCache();
            cache.AddValue("a", 0);
            cache.AddValue("b", 0);
            cache.AddValue("c", 0);
            cache.AddValue("d", 0);

            Assert.True(cache.Contains("a"));
            Assert.True(cache.Contains("b"));
            Assert.True(cache.Contains("c"));
            Assert.True(cache.Contains("d"));

            cache.Clear();

            Assert.False(cache.Contains("a"));
            Assert.False(cache.Contains("b"));
            Assert.False(cache.Contains("c"));
            Assert.False(cache.Contains("d"));
        }

        [Fact]
        public void Test_CacheCanRemoveItem()
        {
            var cache = new MvxNavigationCache();
            cache.AddValue("e", 0);
            cache.AddValue("f", 0);
            cache.AddValue("g", 0);
            cache.AddValue("h", 0);

            Assert.True(cache.Contains("e"));
            Assert.True(cache.Contains("f"));
            Assert.True(cache.Contains("g"));
            Assert.True(cache.Contains("h"));

            cache.Remove("g");

            Assert.False(cache.Contains("g"));
            Assert.True(cache.Contains("e"));
            Assert.True(cache.Contains("f"));
            Assert.True(cache.Contains("h"));
        }

        public class TestObject
        {
        }
    }
}
