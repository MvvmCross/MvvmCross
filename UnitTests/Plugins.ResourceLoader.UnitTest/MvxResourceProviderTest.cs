// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Plugin.ResourceLoader;

namespace MvvmCross.Plugin.ResourceLoader.UnitTest
{
    public class MvxResourceProviderTest
    {
        private class MvxResourceProviderStub : MvxResourceProvider
        {
            public string GetResourceNameForPath(string path)
            {
                return GenerateResourceNameFromPath(path);
            }
        }

        [Test]
        public async Task ICanGenerateResourceNameWithNumbers()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("123");

            await Assert.That(numberPath).IsEqualTo("_123");
        }

        [Test]
        public async Task ICanGenerateResourceNameWithDots()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("abc.def");

            await Assert.That(numberPath).IsEqualTo("abc.def");
        }

        [Test]
        public async Task ICanGenerateResourceNameWithSlashes()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("abc/def");

            await Assert.That(numberPath).IsEqualTo("abc.def");
        }

        [Test]
        public async Task ICanGenerateResourceNameWithVersionNumbers()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("abc1.2.3");

            await Assert.That(numberPath).IsEqualTo("abc1._2._3");
        }

        [Test]
        public async Task ICanGenerateResourceNameWithNumberFolders()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("abc1/2/3");

            await Assert.That(numberPath).IsEqualTo("abc1._2._3");
        }

        [Test]
        public async Task ICanGenerateResourceNameWithInvalidCharacters()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("abc1-2~3");

            await Assert.That(numberPath).IsEqualTo("abc1_2_3");
        }

        [Test]
        public async Task ICanGenerateResourceNameWithDoubleSlashes()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("abc//def");

            await Assert.That(numberPath).IsEqualTo("abc.def");
        }
    }
}
