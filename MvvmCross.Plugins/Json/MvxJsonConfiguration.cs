// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Plugin;

namespace MvvmCross.Plugin.Json
{
    [Preserve(AllMembers = true)]
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
