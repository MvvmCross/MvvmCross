using System;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Test.Core;
using Cirrious.MvvmCross.Test.TestViewModels;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Test
{
    [TestFixture]
    public class MvxViewModelByNameLookupTest : MvxIoCSupportingTest
    {
        [Test]
        public void Test_ViewModelByName_Finds_Expected_ViewModel()
        {
            ClearAll();

            var assembly = this.GetType().Assembly;
            var finder = new MvxViewModelByNameLookup(new [] { assembly });
            Type result;
            Assert.IsTrue(finder.TryLookup("Test1ViewModel", out result));
            Assert.AreEqual(typeof(Test1ViewModel), result);
            Assert.IsTrue(finder.TryLookup("Test2ViewModel", out result));
            Assert.AreEqual(typeof(Test2ViewModel), result);
            Assert.IsTrue(finder.TryLookup("Test3ViewModel", out result));
            Assert.AreEqual(typeof(Test3ViewModel), result);
            Assert.IsFalse(finder.TryLookup("AbstractTest1ViewModel", out result));
            Assert.IsNull(result);
            Assert.IsFalse(finder.TryLookup("NoWayTestViewModel", out result));
            Assert.IsNull(result);
        }
    }
}