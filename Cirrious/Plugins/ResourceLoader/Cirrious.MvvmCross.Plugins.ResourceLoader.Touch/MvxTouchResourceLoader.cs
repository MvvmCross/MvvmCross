﻿// MvxTouchResourceLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.File.Touch;

namespace Cirrious.MvvmCross.Plugins.ResourceLoader.Touch
{
    public class MvxTouchResourceLoader
        : MvxBaseResourceLoader
          , IMvxServiceConsumer
    {
        #region Implementation of IMvxResourceLoader

        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            resourcePath = MvxTouchFileStore.ResScheme + resourcePath;
			var fileService = this.GetService<IMvxFileStore>();
            if (!fileService.TryReadBinaryFile(resourcePath, (stream) =>
                {
                    streamAction(stream);
                    return true;
                }))
                throw new MvxException("Failed to read file {0}", resourcePath);
        }

        #endregion
    }
}