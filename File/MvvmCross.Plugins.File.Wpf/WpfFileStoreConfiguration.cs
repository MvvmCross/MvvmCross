using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.CrossCore.Plugins;

namespace MvvmCross.Plugins.File.Wpf
{
    public class WpfFileStoreConfiguration: IMvxPluginConfiguration
    {
        public string RootFolder { get; set; }
    }
}
