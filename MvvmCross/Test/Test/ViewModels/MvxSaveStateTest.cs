﻿// MvxSaveStateTest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.ViewModels
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Test.Core;
    using MvvmCross.Test.Mocks.TestViewModels;

    using NUnit.Framework;

    [TestFixture]
    public class MvxSaveStateTest : MvxIoCSupportingTest
    {
        [Test]
        public void Test_SaveState()
        {
            ClearAll();

            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var viewModel = new Test3ViewModel
            {
                AdditionalSaveStateFields = new Dictionary<string, string>
                {
                    {"Life1", "John"},
                    {"Life2", "Jane"},
                },
                SaveStateBundleObject = new BundleObject
                {
                    TheBool1 = false,
                    TheBool2 = true,
                    TheGuid1 = Guid.NewGuid(),
                    TheGuid2 = new Guid(123, 10, 444, 1, 2, 3, 4, 5, 6, 7, 8),
                    TheInt1 = 123,
                    TheInt2 = 456,
                    TheString1 = "Hello World",
                    TheString2 = null
                }
            };

            var bundle = viewModel.SaveStateBundle();

            var extracted = bundle.Read<BundleObject>();
            Assert.AreEqual(viewModel.SaveStateBundleObject, extracted);
            Assert.AreEqual("John", bundle.Data["Life1"]);
            Assert.AreEqual("Jane", bundle.Data["Life2"]);
        }

        [Test]
        public void Test_NullSaveState()
        {
            ClearAll();

            var viewModel = new Test3ViewModel();

            var bundle = viewModel.SaveStateBundle();
            Assert.AreEqual(0, bundle.Data.Count);
        }
    }
}