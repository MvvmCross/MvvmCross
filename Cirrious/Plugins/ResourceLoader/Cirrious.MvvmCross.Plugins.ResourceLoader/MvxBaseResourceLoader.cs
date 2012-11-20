#region Copyright
// <copyright file="MvxBaseResourceLoader.cs" company="Cirrious">
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
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.MvvmCross.Plugins.ResourceLoader
{
    public abstract class MvxBaseResourceLoader: IMvxResourceLoader
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
                GetResourceStream(resourcePath, stream =>
                    {
                        found = stream != null;
                    });
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