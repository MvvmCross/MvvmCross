// MvxJsonConfiguration.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Plugins;

namespace MvvmCross.Plugins.Json
{
    public class MvxJsonConfiguration
        : IMvxPluginConfiguration
    {
        public static readonly MvxJsonConfiguration Default = new MvxJsonConfiguration();

        public MvxJsonConfiguration()
        {
            RegisterAsTextSerializer = true;
        }

        public bool RegisterAsTextSerializer { get; set; }
    }
}