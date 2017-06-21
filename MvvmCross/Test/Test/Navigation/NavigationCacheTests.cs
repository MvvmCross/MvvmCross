using MvvmCross.Core.Navigation;
using NUnit.Framework;

namespace MvvmCross.Test.Navigation
{
    [TestFixture]
    public class NavigationCacheTests
    {
        [Test]
        [TestCase("a", 1, 1)]
        [TestCase("a", -1, -1)]
        [TestCase("a", int.MaxValue, int.MaxValue)]
        [TestCase("a", int.MinValue, int.MinValue)]
        [TestCase("a", default(int), default(int))]
        [TestCase("a", 1337, 1337)]
        public void Test_IntsCanBeCached(string key, int value, int expected)
        {
            SimpleTest(new MvxNavigationCache(), key, value, expected);
        }

        [Test]
        [TestCase("a", 1.0, 1.0)]
        [TestCase("a", -1.0, -1.0)]
        [TestCase("a", double.MaxValue, double.MaxValue)]
        [TestCase("a", double.MinValue, double.MinValue)]
        [TestCase("a", default(double), default(double))]
        [TestCase("a", 1337.0, 1337.0)]
        public void Test_DoublesCanBeCached(string key, double value, double expected)
        {
            SimpleTest(new MvxNavigationCache(), key, value, expected);
        }

        [Test]
        [TestCase("a", "a", "a")]
        [TestCase("a", "", "")]
        [TestCase("a", null, null)]
        public void Test_StringsCanBeCached(string key, string value, string expected)
        {
            SimpleTest(new MvxNavigationCache(), key, value, expected);
        }

        private static void SimpleTest<T>(IMvxNavigationCache cache, string key, T value, T expected)
        {
            Assert.IsTrue(cache.AddValue(key, value));
            var addedValue = cache.GetValueOrDefault<T>(key);
            Assert.AreEqual(expected, addedValue);
        }

        [Test]
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

        [Test]
        public void Test_CacheCanClear()
        {
            var cache = new MvxNavigationCache();
            cache.AddValue("a", 0);
            cache.AddValue("b", 0);
            cache.AddValue("c", 0);
            cache.AddValue("d", 0);

            Assert.IsTrue(cache.Contains("a"));
            Assert.IsTrue(cache.Contains("b"));
            Assert.IsTrue(cache.Contains("c"));
            Assert.IsTrue(cache.Contains("d"));

            cache.Clear();

            Assert.IsFalse(cache.Contains("a"));
            Assert.IsFalse(cache.Contains("b"));
            Assert.IsFalse(cache.Contains("c"));
            Assert.IsFalse(cache.Contains("d"));
        }

        [Test]
        public void Test_CacheCanRemoveItem()
        {
            var cache = new MvxNavigationCache();
            cache.AddValue("e", 0);
            cache.AddValue("f", 0);
            cache.AddValue("g", 0);
            cache.AddValue("h", 0);

            Assert.IsTrue(cache.Contains("e"));
            Assert.IsTrue(cache.Contains("f"));
            Assert.IsTrue(cache.Contains("g"));
            Assert.IsTrue(cache.Contains("h"));

            cache.Remove("g");

            Assert.IsFalse(cache.Contains("g"));
            Assert.IsTrue(cache.Contains("e"));
            Assert.IsTrue(cache.Contains("f"));
            Assert.IsTrue(cache.Contains("h"));
        }

        public class TestObject
        {
        }
    }
}
