// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using MvvmCross.Exceptions;

namespace MvvmCross.Plugin.ResourceLoader.Platforms.Ios
{
    public class MvxIosResourceLoader
        : MvxResourceLoader
    {
        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            if (!System.IO.File.Exists(resourcePath))
            {
                throw new MvxException("Failed to read file {0}", resourcePath);
            }

            using (var fileStream = System.IO.File.Open(resourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                streamAction?.Invoke(fileStream);
            }
        }
    }
}
