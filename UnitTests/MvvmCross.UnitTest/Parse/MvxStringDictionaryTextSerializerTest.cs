// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.Core.Parse.StringDictionary;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.TestViewModels;
using MvvmCross.ViewModels;

namespace MvvmCross.UnitTest.Parse
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
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

        [Test]
        public async Task Test_Round_Trip_Works_For_Normal_ViewModel_Requests()
        {
            var parameterBundle = new MvxBundle(new Dictionary<string, string> { { "On'e", "1'\\" }, { "Two", "2" } });
            var presentationBundle =
                new MvxBundle(new Dictionary<string, string> { { "Thre\"\'\\e", "3\"\'\\" }, { "Four", null } });
            var request = new MvxViewModelRequest<Test1ViewModel>(parameterBundle, presentationBundle);

            var serializer = new MvxViewModelRequestCustomTextSerializer();
            var output = serializer.SerializeObject(request);

            var deserializer = new MvxViewModelRequestCustomTextSerializer();
            var deserialized = deserializer.DeserializeObject<MvxViewModelRequest>(output);

            await Assert.That(deserialized.ViewModelType).IsEqualTo(typeof(Test1ViewModel));
            await Assert.That(deserialized.PresentationValues.Count).IsEqualTo(2);
            await Assert.That(deserialized.ParameterValues.Count).IsEqualTo(2);
            await Assert.That(deserialized.ParameterValues["On'e"]).IsEqualTo("1'\\");
            await Assert.That(deserialized.ParameterValues["Two"]).IsEqualTo("2");
            await Assert.That(deserialized.PresentationValues["Thre\"\'\\e"]).IsEqualTo("3\"\'\\");
            await Assert.That(deserialized.PresentationValues["Four"]).IsNull();
        }

        [Test]
        public async Task Test_Round_Trip_Works_For_Part_Empty_ViewModel_Requests()
        {
            var parameterBundle = new MvxBundle();
            var presentationBundle = new MvxBundle();
            var request = new MvxViewModelRequest<Test1ViewModel>(parameterBundle, presentationBundle);

            var serializer = new MvxViewModelRequestCustomTextSerializer();
            var output = serializer.SerializeObject(request);

            var deserializer = new MvxViewModelRequestCustomTextSerializer();
            var deserialized = deserializer.DeserializeObject<MvxViewModelRequest>(output);

            await Assert.That(deserialized.ViewModelType).IsEqualTo(typeof(Test1ViewModel));
            await Assert.That(deserialized.PresentationValues).IsEmpty();
            await Assert.That(deserialized.ParameterValues).IsEmpty();
        }
    }
}
