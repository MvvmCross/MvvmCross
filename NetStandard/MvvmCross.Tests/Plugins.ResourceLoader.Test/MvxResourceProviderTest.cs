using Xunit;

namespace MvvmCross.Plugins.ResourceLoader.Test
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

        [Fact]
        public void ICanGenerateResourceNameWithNumbers()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("123");

            Assert.Equal("_123", numberPath);
        }

        [Fact]
        public void ICanGenerateResourceNameWithDots()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("abc.def");

            Assert.Equal("abc.def", numberPath);
        }

        [Fact]
        public void ICanGenerateResourceNameWithSlashes()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("abc/def");

            Assert.Equal("abc.def", numberPath);
        }

        [Fact]
        public void ICanGenerateResourceNameWithVersionNumbers()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("abc1.2.3");

            Assert.Equal("abc1._2._3", numberPath);
        }

        [Fact]
        public void ICanGenerateResourceNameWithNumberFolders()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("abc1/2/3");

            Assert.Equal("abc1._2._3", numberPath);
        }

        [Fact]
        public void ICanGenerateResourceNameWithInvalidCharacters()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("abc1-2~3");

            Assert.Equal("abc1_2_3", numberPath);
        }

        [Fact]
        public void ICanGenerateResourceNameWithDoubleSlashes()
        {
            MvxResourceProviderStub stub = new MvxResourceProviderStub();
            string numberPath = stub.GetResourceNameForPath("abc//def");

            Assert.Equal("abc.def", numberPath);
        }
    }
}
