#region Copyright
// <copyright file="MvxTouchResourceLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.IO;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.File.Touch;

namespace Cirrious.MvvmCross.Plugins.ResourceLoader.Touch
{
    public class MvxTouchResourceLoader 
        : MvxBaseResourceLoader
        , IMvxServiceConsumer<IMvxSimpleFileStoreService>
    {
        #region Implementation of IMvxResourceLoader

        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
#warning This direct use of MvxTouchFileStoreService seems a bit "naughty" - will make testing hard? Maybe constant should go somewhere else?
            resourcePath = MvxTouchFileStoreService.ResScheme + resourcePath;
            var fileService = this.GetService<IMvxSimpleFileStoreService>();
            if (!fileService.TryReadBinaryFile(resourcePath, (stream) =>
                                                                 {
                                                                     streamAction(stream);
                                                                     return true;
                                                                 }))
#warning TODO - better exception here!
                throw new MvxException("Failed to read file {0}", resourcePath);
        }

        #endregion
    }
}
