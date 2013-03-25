﻿#region Copyright

// <copyright file="MvxPssResourceLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

#if false

#warning removed as its not really useful currently


using System;
using System.IO;
using System.Reflection;
using Cirrious.MvvmCross.Platform;

namespace Cirrious.MvvmCross.Pss.Services
{
    public class MvxPssResourceLoader : MvxBaseResourceLoader
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
