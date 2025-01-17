// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.Core.Parse.StringDictionary;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.TestViewModels;
using MvvmCross.ViewModels;
using Xunit;

namespace MvvmCross.UnitTest.Parse
{
    [Collection("MvxTest")]
    public class MvxStringDictionaryTextSerializerTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxStringDictionaryTextSerializerTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearAll();

            var viewModelNameLookup = new MvxViewModelByNameLookup();
            viewModelNameLookup.AddAll(GetType().Assembly);
            _fixture.Ioc.RegisterSingleton<IMvxViewModelByNameLookup>(viewModelNameLookup);
        }

        [Fact]
        public void Test_Round_Trip_Works_For_Normal_ViewModel_Requests()
        {
            var parameterBundle = new MvxBundle(new Dictionary<string, string> { { "On'e", "1'\\" }, { "Two", "2" } });
            var presentationBundle =
                new MvxBundle(new Dictionary<string, string> { { "Thre\"\'\\e", "3\"\'\\" }, { "Four", null } });
            var request = new MvxViewModelRequest<Test1ViewModel>(parameterBundle, presentationBundle);

            var serializer = new MvxViewModelRequestCustomTextSerializer();
            var output = serializer.SerializeObject(request);

            var deserializer = new MvxViewModelRequestCustomTextSerializer();
            var deserialized = deserializer.DeserializeObject<MvxViewModelRequest>(output);

            Assert.Equal(typeof(Test1ViewModel), deserialized.ViewModelType);
            Assert.Equal(2, deserialized.PresentationValues.Count);
            Assert.Equal(2, deserialized.ParameterValues.Count);
            Assert.Equal("1'\\", deserialized.ParameterValues["On'e"]);
            Assert.Equal("2", deserialized.ParameterValues["Two"]);
            Assert.Equal("3\"\'\\", deserialized.PresentationValues["Thre\"\'\\e"]);
            Assert.Null(deserialized.PresentationValues["Four"]);
        }

        [Fact]
        public void Test_Round_Trip_Works_For_Part_Empty_ViewModel_Requests()
        {
            var parameterBundle = new MvxBundle();
            var presentationBundle = new MvxBundle();
            var request = new MvxViewModelRequest<Test1ViewModel>(parameterBundle, presentationBundle);

            var serializer = new MvxViewModelRequestCustomTextSerializer();
            var output = serializer.SerializeObject(request);

            var deserializer = new MvxViewModelRequestCustomTextSerializer();
            var deserialized = deserializer.DeserializeObject<MvxViewModelRequest>(output);

            Assert.Equal(typeof(Test1ViewModel), deserialized.ViewModelType);
            Assert.Empty(deserialized.PresentationValues);
            Assert.Empty(deserialized.ParameterValues);
        }
    }
}
