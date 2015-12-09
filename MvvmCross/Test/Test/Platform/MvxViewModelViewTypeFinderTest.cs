// MvxViewModelViewTypeFinderTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Test.Core;
using Cirrious.MvvmCross.Test.Mocks.TestViewModels;
using Cirrious.MvvmCross.Test.Mocks.TestViews;
using Cirrious.MvvmCross.ViewModels;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Test.Platform
{
    [TestFixture]
    public class MvxViewModelViewTypeFinderTest : MvxIoCSupportingTest
    {
        [Test]
        public void Test_MvxViewModelViewTypeFinder()
        {
            ClearAll();

            var assembly = this.GetType().Assembly;
            var viewModelNameLookup = new MvxViewModelByNameLookup();
            viewModelNameLookup.AddAll(assembly);
            var nameMapping = new MvxPostfixAwareViewToViewModelNameMapping("View", "Oddness");
            var test = new MvxViewModelViewTypeFinder(viewModelNameLookup, nameMapping);

            // test for positives
            var result = test.FindTypeOrNull(typeof(Test1View));
            Assert.AreEqual(typeof(Test1ViewModel), result);
            result = test.FindTypeOrNull(typeof(NotTest2View));
            Assert.AreEqual(typeof(Test2ViewModel), result);
            result = test.FindTypeOrNull(typeof(NotTest3View));
            Assert.AreEqual(typeof(Test3ViewModel), result);
            result = test.FindTypeOrNull(typeof(OddNameOddness));
            Assert.AreEqual(typeof(OddNameViewModel), result);

            // test for negatives
            result = test.FindTypeOrNull(typeof(AbstractTest1View));
            Assert.IsNull(result);
            result = test.FindTypeOrNull(typeof(NotReallyAView));
            Assert.IsNull(result);
        }
    }
}