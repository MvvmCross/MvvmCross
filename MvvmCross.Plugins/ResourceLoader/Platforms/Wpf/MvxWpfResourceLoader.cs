// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace MvvmCross.Plugin.ResourceLoader.Platforms.Wpf
{
    public class MvxWpfResourceLoader : MvxResourceLoader
    {
        #region Implementation of IMvxResourceLoader

        public override ValueTask GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            var streamInfo = Application.GetResourceStream(new Uri(resourcePath, UriKind.Relative));
            streamAction?.Invoke(streamInfo?.Stream);

            return new ValueTask();
        }

        #endregion Implementation of IMvxResourceLoader
    }
}
