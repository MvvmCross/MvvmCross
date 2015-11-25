// MvxWpfResourceLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;

namespace MvvmCross.Plugins.ResourceLoader.Wpf
{
    public class MvxWpfResourceLoader : MvxResourceLoader
    {
        #region Implementation of IMvxResourceLoader

        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            var streamInfo = System.Windows.Application.GetResourceStream(new Uri(resourcePath, UriKind.Relative));
            if (streamInfo != null)
                streamAction(streamInfo.Stream);
            else
                streamAction(null);
        }

        #endregion Implementation of IMvxResourceLoader
    }
}