// MvxAndroidResourceLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Android.Content.Res;
using MvvmCross.Platform.Droid;
using MvvmCross.Platform;

namespace MvvmCross.Plugins.ResourceLoader.Droid
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

        private AssetManager Assets => _assets ?? (_assets = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext.Assets);
    }
}