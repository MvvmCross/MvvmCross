// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Windows;

namespace MvvmCross.Plugin.ResourceLoader.Platform.Wpf
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
