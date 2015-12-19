// MvxViewModelByNameLookupTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.Platform
{
    using System;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Test.Core;
    using MvvmCross.Test.Mocks.TestViewModels;

    using NUnit.Framework;

    [TestFixture]
    public class MvxViewModelByNameLookupTest : MvxIoCSupportingTest
    {
        [Test]
        public void Test_ViewModelByName_Finds_Expected_ViewModel()
        {
            ClearAll();

            var assembly = this.GetType().Assembly;
            var finder = new MvxViewModelByNameLookup();
            finder.AddAll(assembly);
            Type result;
            Assert.IsTrue(finder.TryLookupByName("Test1ViewModel", out result));
            Assert.AreEqual(typeof(Test1ViewModel), result);
            Assert.IsTrue(finder.TryLookupByName("Test2ViewModel", out result));
            Assert.AreEqual(typeof(Test2ViewModel), result);
            Assert.IsTrue(finder.TryLookupByName("Test3ViewModel", out result));
            Assert.AreEqual(typeof(Test3ViewModel), result);
            Assert.IsFalse(finder.TryLookupByName("AbstractTest1ViewModel", out result));
            Assert.IsNull(result);
            Assert.IsFalse(finder.TryLookupByName("NoWayTestViewModel", out result));
            Assert.IsNull(result);
            Assert.IsTrue(finder.TryLookupByFullName("Cirrious.MvvmCross.Test.Mocks.TestViewModels.Test1ViewModel",
                                                     out result));
            Assert.AreEqual(typeof(Test1ViewModel), result);
            Assert.IsTrue(finder.TryLookupByFullName("Cirrious.MvvmCross.Test.Mocks.TestViewModels.Test2ViewModel",
                                                     out result));
            Assert.AreEqual(typeof(Test2ViewModel), result);
            Assert.IsTrue(finder.TryLookupByFullName("Cirrious.MvvmCross.Test.Mocks.TestViewModels.Test3ViewModel",
                                                     out result));
            Assert.AreEqual(typeof(Test3ViewModel), result);
            Assert.IsFalse(
                finder.TryLookupByFullName("Cirrious.MvvmCross.Test.Mocks.TestViewModels.AbstractTest1ViewModel",
                                           out result));
            Assert.IsNull(result);
            Assert.IsFalse(finder.TryLookupByFullName(
                "Cirrious.MvvmCross.Test.Mocks.TestViewModels.NoWayTestViewModel", out result));
            Assert.IsNull(result);
        }
    }
}