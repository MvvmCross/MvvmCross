using MvvmCross.Plugin;

namespace MvvmCross.UnitTest.Mocks.Plugin
{
    [MvxPlugin]
    public class PluginMock1 : IMvxPlugin
    {
        public static int LoadCount { get; private set; }

        public void Load()
        {
            LoadCount++;
        }
    }

    [MvxPlugin]
    public class PluginMock2 : IMvxPlugin
    {
        public static int LoadCount { get; private set; }

        public void Load()
        {
            LoadCount++;
        }
    }

    [MvxPlugin]
    public class PluginMock3 : IMvxPlugin
    {
        public static int LoadCount { get; private set; }

        public void Load()
        {
            LoadCount++;
        }
    }

    [MvxPlugin]
    public class PluginMock4 : IMvxPlugin
    {
        public static int LoadCount { get; private set; }

        public void Load()
        {
            LoadCount++;
        }
    }

    [MvxPlugin]
    public class PluginMock5 : IMvxPlugin
    {
        public static int LoadCount { get; private set; }

        public void Load()
        {
            LoadCount++;
        }
    }

    [MvxPlugin]
    public class PluginMock6 : IMvxPlugin
    {
        public static int LoadCount { get; private set; }

        public void Load()
        {
            LoadCount++;
        }
    }
}
