// MvxConsoleResourceLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#if false

#warning removed as its not really useful currently


using System;
using System.IO;
using System.Reflection;
using Cirrious.MvvmCross.Platform;

namespace Cirrious.MvvmCross.Console.Services
{
    public class MvxConsoleResourceLoader : MvxBaseResourceLoader
    {
        #region Implementation of IMvxResourceLoader

        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
#warning ? need to check and clarify what exceptions can be thrown here!
#warning HACK HACK HACK - we should have a different way to get the correct namespace
            var entryAssembly = Assembly.GetEntryAssembly();
            var fullPath = entryAssembly.GetName().Name + "." + resourcePath.Replace('/', '.').Replace('\\', '.');
            var stream = Assembly.GetEntryAssembly().GetManifestResourceStream(fullPath);
            streamAction(stream);
        }

        #endregion
    }
}

#endif