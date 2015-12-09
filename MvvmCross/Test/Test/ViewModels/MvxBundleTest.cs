// MvxBundleTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.ViewModels
{
    using System;
    using System.Linq;

    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Test.Mocks.TestViewModels;

    using NUnit.Framework;

    [TestFixture]
    public class MvxBundleTest : MvxIoCSupportingTest
    {
        [Test]
        public void Test_RoundTrip()
        {
            ClearAll();

            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var testObject = new BundleObject
            {
                TheBool1 = false,
                TheBool2 = true,
                TheGuid1 = Guid.NewGuid(),
                TheGuid2 = new Guid(123, 10, 444, 1, 2, 3, 4, 5, 6, 7, 8),
                TheInt1 = 123,
                TheInt2 = 456,
                TheString1 = "Hello World",
                TheString2 = null
            };
            var bundle = new MvxBundle();
            bundle.Write(testObject);

            var output = bundle.Read<BundleObject>();

            Assert.AreEqual(testObject, output);
        }

        public BundleObject TestFunction(string TheString1, string missing, int TheInt2, bool TheBool2, Guid TheGuid2)
        {
            return new BundleObject
            {
                TheString1 = TheString1,
                TheString2 = missing,
                TheInt2 = TheInt2,
                TheBool2 = TheBool2,
                TheGuid2 = TheGuid2
            };
        }

        [Test]
        public void Test_FillArguments()
        {
            ClearAll();

            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var testObject = new BundleObject
            {
                TheBool1 = false,
                TheBool2 = true,
                TheGuid1 = Guid.NewGuid(),
                TheGuid2 = new Guid(123, 10, 444, 1, 2, 3, 4, 5, 6, 7, 8),
                TheInt1 = 123,
                TheInt2 = 456,
                TheString1 = "Hello World",
                TheString2 = "Goo"
            };
            var bundle = new MvxBundle();
            bundle.Write(testObject);

            var method = this.GetType().GetMethod("TestFunction");
            var args = bundle.CreateArgumentList(method.GetParameters(), "ignored debug text");
            var output = method.Invoke(this, args.ToArray());

            var expected = new BundleObject
            {
                TheBool1 = false,
                TheBool2 = true,
                TheGuid1 = Guid.Empty,
                TheGuid2 = new Guid(123, 10, 444, 1, 2, 3, 4, 5, 6, 7, 8),
                TheInt1 = 0,
                TheInt2 = 456,
                TheString1 = "Hello World",
                TheString2 = null
            };
            Assert.AreEqual(expected, output);
        }
    }
}