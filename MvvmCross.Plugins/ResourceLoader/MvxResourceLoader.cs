// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Plugin.ResourceLoader
{
    public abstract class MvxResourceLoader : IMvxResourceLoader
    {
        #region Implementation of IMvxResourceLoader

        public string GetTextResource(string resourcePath)
        {
            try
            {
                string text = null;
                GetResourceStream(resourcePath, (stream) =>
                    {
                        if (stream == null)
                            return;

                        using (var textReader = new StreamReader(stream))
                        {
                            text = textReader.ReadToEnd();
                        }
                    });
                return text;
            }
            //#if !NETFX_CORE
            //            catch (ThreadAbortException)
            //            {
            //                throw;
            //            }
            //#endif
            catch (Exception ex)
            {
                throw ex.MvxWrap("Cannot load resource {0}", resourcePath);
            }
        }

        public abstract void GetResourceStream(string resourcePath, Action<Stream> streamAction);

        public virtual bool ResourceExists(string resourcePath)
        {
            try
            {
                var found = false;
                GetResourceStream(resourcePath, stream => { found = stream != null; });
                return found;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion Implementation of IMvxResourceLoader
    }
}
