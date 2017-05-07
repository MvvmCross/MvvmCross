using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.File
{
    public class MvxFileConfiguration : IMvxPluginConfiguration
    {
        public static MvxFileConfiguration Default => new MvxFileConfiguration();

        /// <summary>
        /// Gets or sets a value indicating whether the default path should be appended to the one
        /// provided by the consumer when manipulating the files. Defaults to true.
        /// </summary>
        /// 
        public bool AppendDefaultPath { get; set; } = true;
    }
}
