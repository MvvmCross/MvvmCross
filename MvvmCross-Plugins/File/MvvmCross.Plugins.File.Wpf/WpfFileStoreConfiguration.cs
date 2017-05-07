using System;

namespace MvvmCross.Plugins.File.Wpf
{
    public class WpfFileStoreConfiguration : MvxFileConfiguration
    {
        public static new WpfFileStoreConfiguration Default = new WpfFileStoreConfiguration();

        public string RootFolder { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
}