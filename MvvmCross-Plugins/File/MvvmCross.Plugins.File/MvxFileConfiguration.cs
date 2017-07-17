using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.File
{
    public class MvxFileConfiguration : IMvxPluginConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether the default path should be appended to the one
        /// provided by the consumer when manipulating the files. Defaults to true.
        /// </summary>
        /// 
        public bool AppendDefaultPath { get; set; } = true;

        public string BasePath { get; set; }

        public MvxFileConfiguration(string basePath)
        {
            BasePath = basePath;
        }
    }
}
