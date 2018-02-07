// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugin.File
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
