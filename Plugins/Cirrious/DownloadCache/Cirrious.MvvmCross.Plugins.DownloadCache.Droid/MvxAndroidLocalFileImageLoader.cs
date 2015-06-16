// MvxAndroidLocalFileImageLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Threading.Tasks;
using Android.Graphics;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Plugins.File;
using System.Collections.Generic;
using System;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Droid
{
    public class MvxAndroidLocalFileImageLoader
        : IMvxLocalFileImageLoader<Bitmap>
    {
        private const string ResourcePrefix = "res:";
        private readonly IDictionary<CacheKey, WeakReference<Bitmap>> _memCache = new Dictionary<CacheKey, WeakReference<Bitmap>>();


        public Task<MvxImage<Bitmap>> Load(string localPath, bool shouldCache, int maxWidth, int maxHeight)
        {
            return Task.Run(() =>
            {
                Bitmap bitmap;
                var shouldAddToCache = shouldCache;
                if (shouldCache && TryGetCachedBitmap(localPath, maxWidth, maxHeight, out bitmap))
                {
                    shouldAddToCache = false;
                }
                else if (localPath.StartsWith(ResourcePrefix))
                {
                    var resourcePath = localPath.Substring(ResourcePrefix.Length);
                    bitmap = LoadResourceBitmap(resourcePath);
                }
                else
                {
                    bitmap = LoadBitmap(localPath, maxWidth, maxHeight);
                }

                if (shouldAddToCache)
                {
                    AddToCache(localPath, maxWidth, maxHeight, bitmap);
                }

                return (MvxImage<Bitmap>)new MvxAndroidImage(bitmap);
            });
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

        private Bitmap LoadBitmap(string localPath, int maxWidth, int maxHeight)
        {
            if (maxWidth > 0 || maxHeight > 0)
            {
                // load thumbnail - see: http://developer.android.com/training/displaying-bitmaps/load-bitmap.html
                var options = new BitmapFactory.Options();
                options.InJustDecodeBounds = true;
                BitmapFactory.DecodeFile(localPath, options);

                // Calculate inSampleSize
                options.InSampleSize = CalculateInSampleSize(options, maxWidth, maxHeight);
                // see http://slodge.blogspot.co.uk/2013/02/huge-android-memory-bug-and-bug-hunting.html
                options.InPurgeable = true;

                // Decode bitmap with inSampleSize set
                options.InJustDecodeBounds = false;
                return BitmapFactory.DecodeFile(localPath, options);
            }
            else
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

        private static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > reqHeight || width > reqWidth)
            {

                int halfHeight = height / 2;
                int halfWidth = width / 2;

                // Calculate the largest inSampleSize value that is a power of 2 and keeps both
                // height and width larger than the requested height and width.
                while ((halfHeight / inSampleSize) > reqHeight
                        && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return inSampleSize;
        }

        private bool TryGetCachedBitmap(string localPath, int maxWidth, int maxHeight, out Bitmap bitmap)
        {
            var key = new CacheKey(localPath, maxWidth, maxHeight);
            WeakReference<Bitmap> reference;
           
            if (_memCache.TryGetValue(key, out reference))
            {
                Bitmap target;
                if (reference.TryGetTarget(out target) && target != null && target.Handle != IntPtr.Zero && !target.IsRecycled)
                {
                    bitmap = target;
                    return true;
                }
                else
                {
                    _memCache.Remove(key);
                }
            }
            bitmap = null;
            return false;
        }

        private void AddToCache(string localPath, int maxWidth, int maxHeight, Bitmap bitmap)
        {
            if (bitmap != null)
            {
                var key = new CacheKey(localPath, maxWidth, maxHeight);
                _memCache[key] = new WeakReference<Bitmap>(bitmap);
            }
        }

        private class CacheKey
        {
            public string LocalPath { get; set; }
            public int MaxWidth { get; set; }
            public int MaxHeight { get; set; }

            public CacheKey(string localPath, int maxWidth, int maxHeight)
            {
                if (localPath == null) throw new ArgumentNullException("localPath");
                LocalPath = localPath;
                MaxWidth = maxWidth;
                MaxHeight = maxHeight;
            }

            public override int GetHashCode()
            {
                return LocalPath.GetHashCode() + MaxWidth.GetHashCode() + MaxHeight.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var other = obj as CacheKey;
                if (other == null)
                    return false;

                return object.Equals(LocalPath, other.LocalPath) &&
                       object.Equals(MaxWidth, other.MaxWidth) &&
                       object.Equals(MaxHeight, other.MaxHeight);
            }
        }
    }
}