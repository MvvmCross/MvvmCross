// MvxAndroidFileStore.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using System.IO;
using Android.Content;
using Cirrious.CrossCore.Droid;
using Cirrious.CrossCore;

#endregion

namespace MvvmCross.Plugins.File.Droid
{
    public class MvxAndroidFileStore
        : MvxIoFileStoreBase
    {
        private Context _context;

        private Context Context => _context ?? (_context = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext);

        protected override string FullPath(string path)
        {
            return Path.Combine(Context.FilesDir.Path, path);
        }
    }
}