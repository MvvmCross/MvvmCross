﻿// MvxStringDictionaryTextSerializerTest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using MvvmCross.Core.Parse.StringDictionary;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Test.Core;
using MvvmCross.Test.Mocks.TestViewModels;
using NUnit.Framework;

namespace MvvmCross.Test.Parse
{
    [TestFixture]
    public class MvxStringDictionaryTextSerializerTest
        : MvxIoCSupportingTest
    {
        [Test]
        public void Test_Round_Trip_Works_For_Normal_ViewModel_Requests()
        {
            ClearAll();

            var viewModelNameLookup = new MvxViewModelByNameLookup();
            viewModelNameLookup.AddAll(GetType().Assembly);
            Mvx.RegisterSingleton<IMvxViewModelByNameLookup>(viewModelNameLookup);

            var parameterBundle = new MvxBundle(new Dictionary<string, string> { { "On'e", "1'\\" }, { "Two", "2" } });
            var presentationBundle =
                new MvxBundle(new Dictionary<string, string> { { "Thre\"\'\\e", "3\"\'\\" }, { "Four", null } });
            var request = new MvxViewModelRequest<Test1ViewModel>(parameterBundle, presentationBundle);

            var serializer = new MvxViewModelRequestCustomTextSerializer();
            var output = serializer.SerializeObject(request);

            var deserializer = new MvxViewModelRequestCustomTextSerializer();
            var deserialized = deserializer.DeserializeObject<MvxViewModelRequest>(output);

            Assert.AreEqual(typeof(Test1ViewModel), deserialized.ViewModelType);
            Assert.AreEqual(2, deserialized.PresentationValues.Count);
            Assert.AreEqual(2, deserialized.ParameterValues.Count);
            Assert.AreEqual("1'\\", deserialized.ParameterValues["On'e"]);
            Assert.AreEqual("2", deserialized.ParameterValues["Two"]);
            Assert.AreEqual("3\"\'\\", deserialized.PresentationValues["Thre\"\'\\e"]);
            Assert.AreEqual(null, deserialized.PresentationValues["Four"]);
        }

        [Test]
        public void Test_Round_Trip_Works_For_Part_Empty_ViewModel_Requests()
        {
            var parameterBundle = new MvxBundle();
            var presentationBundle = new MvxBundle();
            var request = new MvxViewModelRequest<Test1ViewModel>(parameterBundle, presentationBundle);

            var serializer = new MvxViewModelRequestCustomTextSerializer();
            var output = serializer.SerializeObject(request);

            var deserializer = new MvxViewModelRequestCustomTextSerializer();
            var deserialized = deserializer.DeserializeObject<MvxViewModelRequest>(output);

            Assert.AreEqual(typeof(Test1ViewModel), deserialized.ViewModelType);
            Assert.AreEqual(0, deserialized.PresentationValues.Count);
            Assert.AreEqual(0, deserialized.ParameterValues.Count);
        }
    }
}