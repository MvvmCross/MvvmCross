using System;
using System.IO;
using System.Threading;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Localization;

namespace Cirrious.MvvmCross.Platform
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
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex.MvxWrap("Cannot load resource {0}", resourcePath);
            }
        }

        public abstract void GetResourceStream(string resourcePath, Action<Stream> streamAction);

        #endregion
    }
}