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
using Cirrious.MvvmCross.Platform;

namespace Cirrious.MvvmCross.Touch.Platform
{
    public class MvxTouchResourceLoader : MvxBaseResourceLoader
    {
        #region Implementation of IMvxResourceLoader

        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
            resourcePath = MvxTouchFileStoreService.ResScheme + resourcePath;
            var fileService = new MvxTouchFileStoreService();
            if (!fileService.TryReadBinaryFile(resourcePath, (stream) =>
                                                                 {
                                                                     streamAction(stream);
                                                                     return true;
                                                                 }))
#warning TODO - better exception here!
                throw new MvxException();
        }

        #endregion
    }
}
