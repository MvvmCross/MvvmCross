// MvxAndroidFileStoreService.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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
          , IMvxServiceConsumer
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