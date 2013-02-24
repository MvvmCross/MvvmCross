﻿// MvxAndroidFileStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using System.IO;
using Android.Content;
using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.CrossCore.Interfaces.IoC;

#endregion

namespace Cirrious.MvvmCross.Plugins.File.Droid
{
    public class MvxAndroidFileStore
        : MvxBaseFileStore
          , IMvxConsumer
    {
        private Context _context;

        private Context Context
        {
            get
            {
                if (_context == null)
                {
                    _context = this.GetService<IMvxAndroidGlobals>().ApplicationContext;
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