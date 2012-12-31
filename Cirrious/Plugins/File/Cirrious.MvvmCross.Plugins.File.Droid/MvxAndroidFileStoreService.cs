﻿#region Copyright

// <copyright file="MvxAndroidFileStoreService.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

#region using

using System.IO;
using Android.Content;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

#endregion

namespace Cirrious.MvvmCross.Plugins.File.Droid
{
    public class MvxAndroidFileStoreService
        : MvxBaseFileStoreService
          , IMvxServiceConsumer<IMvxAndroidGlobals>
    {
        private Context _context;

        private Context Context
        {
            get
            {
                if (_context == null)
                {
                    _context = this.GetService().ApplicationContext;
                }
                return _context;
            }
        }

        protected override string FullPath(string path)
        {
            return Path.Combine(Context.FilesDir.Path, path);
        }
    }
}