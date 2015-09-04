// MvxResourceLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;

namespace MvvmCross.Plugins.ResourceLoader
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

        #endregion
    }
}