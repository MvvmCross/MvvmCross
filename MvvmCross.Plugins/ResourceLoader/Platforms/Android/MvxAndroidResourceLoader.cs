// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Android.Content.Res;
using MvvmCross.Platforms.Android;

namespace MvvmCross.Plugin.ResourceLoader.Platforms.Android
{
    [Preserve(AllMembers = true)]
	public class MvxAndroidResourceLoader
        : MvxResourceLoader          
    {
        private AssetManager _assets;

        #region Implementation of IMvxResourceLoader

        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            using (var input = Assets.Open(resourcePath))
            {
                streamAction?.Invoke(input);
            }
        }

        #endregion

        private AssetManager Assets => _assets ?? (_assets = Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>().ApplicationContext.Assets);
    }
}
