// MvxWpfResourceLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using System.Windows;

namespace MvvmCross.Plugins.ResourceLoader.Wpf
{
    public class MvxWpfResourceLoader : MvxResourceLoader
    {
        #region Implementation of IMvxResourceLoader

        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            var streamInfo = Application.GetResourceStream(new Uri(resourcePath, UriKind.Relative));
            streamAction?.Invoke(streamInfo?.Stream);
        }

        #endregion Implementation of IMvxResourceLoader
    }
}