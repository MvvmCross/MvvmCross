using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.File.Wpf
{
    public class WpfFileStoreConfiguration : IMvxPluginConfiguration
    {
        public string RootFolder { get; set; }
    }
}