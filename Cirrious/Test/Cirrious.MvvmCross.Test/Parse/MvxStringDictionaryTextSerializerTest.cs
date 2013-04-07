﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Parse.StringDictionary;
using Cirrious.MvvmCross.Test.Mocks.TestViewModels;
using Cirrious.MvvmCross.ViewModels;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Test.Parse
{
    [TestFixture]
    public class MvxStringDictionaryTextSerializerTest
    {
        [Test]
        public void Test_Round_Trip_Works_For_Normal_ViewModel_Requests()
        {
            var parameterBundle = new MvxBundle(new Dictionary<string, string>() {{"On'e", "1'\\"}, {"Two", "2"}});
            var presentationBundle = new MvxBundle(new Dictionary<string, string>() { { "Thre\"\'\\e", "3\"\'\\" }, { "Four", null } });
            var request = new MvxViewModelRequest<Test1ViewModel>(parameterBundle, presentationBundle,
                                                                  new MvxRequestedBy(
                                                                      MvxRequestedByType.AutomatedService, "HelloWorld"));

            var serializer = new MvxViewModelRequestCustomTextSerializer();
            var output = serializer.SerializeObject(request);

            var deserializer = new MvxViewModelRequestCustomTextSerializer();
            var deserialized = deserializer.DeserializeObject<MvxViewModelRequest>(output);

            Assert.AreEqual(typeof(Test1ViewModel), deserialized.ViewModelType);
            Assert.AreEqual(MvxRequestedByType.AutomatedService, deserialized.RequestedBy.Type);
            Assert.AreEqual("HelloWorld", deserialized.RequestedBy.AdditionalInfo);
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
            var request = new MvxViewModelRequest<Test1ViewModel>(parameterBundle, presentationBundle,
                                                                  null);

            var serializer = new MvxViewModelRequestCustomTextSerializer();
            var output = serializer.SerializeObject(request);

            var deserializer = new MvxViewModelRequestCustomTextSerializer();
            var deserialized = deserializer.DeserializeObject<MvxViewModelRequest>(output);

            Assert.AreEqual(typeof(Test1ViewModel), deserialized.ViewModelType);
            Assert.AreEqual(MvxRequestedByType.Unknown, deserialized.RequestedBy.Type);
            Assert.AreEqual(null, deserialized.RequestedBy.AdditionalInfo);
            Assert.AreEqual(0, deserialized.PresentationValues.Count);
            Assert.AreEqual(0, deserialized.ParameterValues.Count);
        }
    }
}
