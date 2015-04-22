// MvxAndroidLocalFileImageLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Graphics;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Plugins.File;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Droid
{
    public class MvxAndroidLocalFileImageLoader
        : IMvxLocalFileImageLoader<Bitmap>          
    {
        private const string ResourcePrefix = "res:";

        public MvxImage<Bitmap> Load(string localPath, bool shouldCache /* ignored here */)
        {
            Bitmap bitmap;
            if (localPath.StartsWith(ResourcePrefix))
            {
                var resourcePath = localPath.Substring(ResourcePrefix.Length);
                bitmap = LoadResourceBitmap(resourcePath);
            }
            else
            {
                bitmap = LoadBitmap(localPath);
            }

            return new MvxAndroidImage(bitmap);
        }

        private IMvxAndroidGlobals _androidGlobals;
        protected IMvxAndroidGlobals AndroidGlobals
        {
            get
            {
                if (_androidGlobals == null)
                    _androidGlobals = Mvx.Resolve<IMvxAndroidGlobals>();
                return _androidGlobals;
            }
        }

        private Bitmap LoadResourceBitmap(string resourcePath)
        {
            var resources = AndroidGlobals.ApplicationContext.Resources;
            var id = resources.GetIdentifier(resourcePath, "drawable", AndroidGlobals.ApplicationContext.PackageName);
            if (id == 0)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Value '{0}' was not a known drawable name", resourcePath);
                return null;
            }

            return BitmapFactory.DecodeResource(resources, id, new BitmapFactory.Options { InPurgeable = true });
        }

        private Bitmap LoadBitmap(string localPath)
        {
            var fileStore = Mvx.Resolve<IMvxFileStore>();
            byte[] contents;
            if (!fileStore.TryReadBinaryFile(localPath, out contents))
                return null;

			// the InPurgeable option is very important for Droid memory management.
			// see http://slodge.blogspot.co.uk/2013/02/huge-android-memory-bug-and-bug-hunting.html
			var options = new BitmapFactory.Options { InPurgeable = true };
            var image = BitmapFactory.DecodeByteArray(contents, 0, contents.Length, options);
            return image;
        }
    }
}