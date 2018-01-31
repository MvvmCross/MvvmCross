// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core.ViewModels;
using MvvmCross.Test.Core;
using MvvmCross.Test.Mocks.TestViewModels;
using MvvmCross.Test.Mocks.TestViews;
using NUnit.Framework;

namespace MvvmCross.Test.Platform
{
    [TestFixture]
    public class MvxViewModelViewLookupBuilderTest : MvxIoCSupportingTest
    {
        [Test]
        public void Test_Builder()
        {
            ClearAll();

            var assembly = GetType().Assembly;
            var viewModelNameLookup = new MvxViewModelByNameLookup();
            viewModelNameLookup.AddAll(assembly);
            var nameMapping = new MvxPostfixAwareViewToViewModelNameMapping("View", "Oddness");
            var finder = new MvxViewModelViewTypeFinder(viewModelNameLookup, nameMapping);
            Ioc.RegisterSingleton<IMvxViewModelTypeFinder>(finder);

            var builder = new MvxViewModelViewLookupBuilder();
            var result = builder.Build(new[] { assembly });

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(typeof(Test1View), result[typeof(Test1ViewModel)]);
            Assert.AreEqual(typeof(NotTest2View), result[typeof(Test2ViewModel)]);
            Assert.AreEqual(typeof(NotTest3View), result[typeof(Test3ViewModel)]);
            Assert.AreEqual(typeof(OddNameOddness), result[typeof(OddNameViewModel)]);
        }
    }
}