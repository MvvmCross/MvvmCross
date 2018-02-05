using MvvmCross.Test;
using Xunit;

namespace MvvmCross.Plugins.Color.Test
{
    [CollectionDefinition("MvxTest")]
    public class MvxTestCollection : ICollectionFixture<MvxTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
