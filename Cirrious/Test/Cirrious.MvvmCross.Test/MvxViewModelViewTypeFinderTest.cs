using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Test.Core;
using Cirrious.MvvmCross.Test.TestViewModels;
using Cirrious.MvvmCross.Test.TestViews;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Test
{
    [TestFixture]
    public class MvxViewModelViewLookupBuilderTest : MvxIoCSupportingTest
    {
        [Test]
        public void Test_Builder()
        {
            ClearAll();

            var assembly = this.GetType().Assembly;
            var viewModelNameLookup = new MvxViewModelByNameLookup(new[] { assembly });
            var finder = new MvxViewModelViewTypeFinder(viewModelNameLookup);
            Ioc.RegisterSingleton<IMvxViewModelTypeFinder>(finder);

            var builder = new MvxViewModelViewLookupBuilder();
            var result = builder.Build(new []  { assembly });

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(typeof(Test1View), result[typeof(Test1ViewModel)]);
            Assert.AreEqual(typeof(NotTest2View), result[typeof(Test2ViewModel)]);
            Assert.AreEqual(typeof(NotTest3View), result[typeof(Test3ViewModel)]);
        }
    }

    [TestFixture]
    public class MvxViewModelViewTypeFinderTest : MvxIoCSupportingTest
    {
        [Test]
        public void Test_MvxViewModelViewTypeFinder()
        {
            ClearAll();

            var assembly = this.GetType().Assembly;
            var viewModelNameLookup = new MvxViewModelByNameLookup(new [] { assembly });
            var test = new MvxViewModelViewTypeFinder(viewModelNameLookup);
            
            // test for positives
            var result = test.FindTypeOrNull(typeof(Test1View));
            Assert.AreEqual(typeof(Test1ViewModel), result);
            result = test.FindTypeOrNull(typeof(NotTest2View));
            Assert.AreEqual(typeof(Test2ViewModel), result);
            result = test.FindTypeOrNull(typeof(NotTest3View));
            Assert.AreEqual(typeof(Test3ViewModel), result);

            // test for negatives
            result = test.FindTypeOrNull(typeof(AbstractTest1View));
            Assert.IsNull(result);
            result = test.FindTypeOrNull(typeof(NotReallyAView));
            Assert.IsNull(result);
        }
    }
}