// MvxTouchResourceLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using MvvmCross.Plugins.File;
using MvvmCross.Plugins.File.Touch;
using System;
using System.IO;

namespace MvvmCross.Plugins.ResourceLoader.Touch
{
    public class MvxTouchResourceLoader
        : MvxResourceLoader

    {
        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            resourcePath = MvxTouchFileStore.ResScheme + resourcePath;
            var fileService = Mvx.Resolve<IMvxFileStore>();
            if (!fileService.TryReadBinaryFile(resourcePath, (stream) =>
                {
                    streamAction?.Invoke(stream);
                    return true;
                }))
                throw new MvxException("Failed to read file {0}", resourcePath);
        }
    }
}