// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Provider;
using MvvmCross.Exceptions;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Views.Base;
using Path = System.IO.Path;
using Stream = System.IO.Stream;
using Uri = Android.Net.Uri;
using ExifInterface = Android.Support.Media.ExifInterface;
using MvvmCross.Logging;

namespace MvvmCross.Plugin.PictureChooser.Platforms.Android
{
    [Preserve(AllMembers = true)]
    public class MvxPictureChooserTask
        : MvxAndroidTask, IMvxPictureChooserTask
    {
        private Uri _cachedUriLocation;
        private RequestParameters _currentRequestParameters;

        #region IMvxPictureChooserTask Members

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream, string> pictureAvailable,
                                     Action assumeCancelled)
        {
            var intent = new Intent(Intent.ActionGetContent);
            intent.SetType("image/*");
            ChoosePictureCommon(MvxIntentRequestCode.PickFromFile, intent, maxPixelDimension, percentQuality,
                                pictureAvailable, assumeCancelled);
        }

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                             Action assumeCancelled)
        {
            ChoosePictureFromLibrary(maxPixelDimension, percentQuality, (stream, name) => pictureAvailable(stream), assumeCancelled);
        }

        public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                Action assumeCancelled)
        {
            var intent = new Intent(MediaStore.ActionImageCapture);

            _cachedUriLocation = GetNewImageUri();
            intent.PutExtra(MediaStore.ExtraOutput, _cachedUriLocation);
            intent.PutExtra("outputFormat", Bitmap.CompressFormat.Jpeg.ToString());
            intent.PutExtra("return-data", true);

            ChoosePictureCommon(MvxIntentRequestCode.PickFromCamera, intent, maxPixelDimension, percentQuality,
                                (stream, name) => pictureAvailable(stream), assumeCancelled);
        }

        public Task<Stream> ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality)
        {
            var task = new TaskCompletionSource<Stream>();
            ChoosePictureFromLibrary(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
            return task.Task;
        }

        public Task<Stream> TakePicture(int maxPixelDimension, int percentQuality)
        {
            var task = new TaskCompletionSource<Stream>();
            TakePicture(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
            return task.Task;
        }

        public void ContinueFileOpenPicker(object args)
        {
        }

        #endregion

        private Uri GetNewImageUri()
        {
            // Optional - specify some metadata for the picture
            var contentValues = new ContentValues();
            //contentValues.Put(MediaStore.Images.ImageColumnsConsts.Description, "A camera photo");

            // Specify where to put the image
            return
                Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>()
                    .ApplicationContext.ContentResolver.Insert(MediaStore.Images.Media.ExternalContentUri, contentValues);
        }

        public void ChoosePictureCommon(MvxIntentRequestCode pickId, Intent intent, int maxPixelDimension,
                                        int percentQuality, Action<Stream, string> pictureAvailable, Action assumeCancelled)
        {
            if (_currentRequestParameters != null)
                throw new MvxException("Cannot request a second picture while the first request is still pending");

            _currentRequestParameters = new RequestParameters(maxPixelDimension, percentQuality, pictureAvailable,
                                                              assumeCancelled);
            StartActivityForResult((int)pickId, intent);
        }

        protected override void ProcessMvxIntentResult(MvxIntentResultEventArgs result)
        {
            try
            {
                MvxPluginLog.Instance.Trace("ProcessMvxIntentResult started...");

                Uri uri;

                switch ((MvxIntentRequestCode)result.RequestCode)
                {
                    case MvxIntentRequestCode.PickFromFile:
                        uri = result.Data?.Data;
                        break;
                    case MvxIntentRequestCode.PickFromCamera:
                        uri = _cachedUriLocation;
                        break;
                    default:
                        // ignore this result - it's not for us
                        MvxPluginLog.Instance.Trace("Unexpected request received from MvxIntentResult - request was {0}",
                                       result.RequestCode);
                        return;
                }

                ProcessPictureUri(result, uri);
            }
            catch (Exception e)
            {
                // TODO: We currently have no way of bubbling this up. Throwing here
                // can crash the app :(

                MvxPluginLog.Instance.ErrorException("Failed to process Intent from PictureChooser", e);
            }
        }

        private void ProcessPictureUri(MvxIntentResultEventArgs result, Uri uri)
        {
            if (_currentRequestParameters == null)
            {
                MvxPluginLog.Instance.Error("Internal error - response received but _currentRequestParameters is null");
                return; // we have not handled this - so we return null
            }

            var responseSent = false;
            try
            {
                // Note for furture bug-fixing/maintenance - it might be better to use var outputFileUri = data.GetParcelableArrayExtra("outputFileuri") here?
                if (result.ResultCode != Result.Ok)
                {
                    MvxPluginLog.Instance.Trace("Non-OK result received from MvxIntentResult - {0} - request was {1}",
                                   result.ResultCode, result.RequestCode);
                    return;
                }

                if (string.IsNullOrEmpty(uri?.Path))
                {
                    MvxPluginLog.Instance.Trace("Empty uri or file path received for MvxIntentResult");
                    return;
                }

                MvxPluginLog.Instance.Trace("Loading InMemoryBitmap started...");
                var memoryStream = LoadInMemoryBitmap(uri);
                if (memoryStream == null)
                {
                    MvxPluginLog.Instance.Trace("Loading InMemoryBitmap failed...");
                    return;
                }
                MvxPluginLog.Instance.Trace("Loading InMemoryBitmap complete...");
                responseSent = true;
                MvxPluginLog.Instance.Trace("Sending pictureAvailable...");
                _currentRequestParameters.PictureAvailable(memoryStream, Path.GetFileNameWithoutExtension(uri.Path));
                MvxPluginLog.Instance.Trace("pictureAvailable completed...");
                return;
            }
            finally
            {
                if (!responseSent)
                    _currentRequestParameters.AssumeCancelled();

                _currentRequestParameters = null;
            }
        }

        private MemoryStream LoadInMemoryBitmap(Uri uri)
        {
            var memoryStream = new MemoryStream();
            var bitmap = LoadScaledBitmap(uri);
            if (bitmap == null)
                return null;
            using (bitmap)
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, _currentRequestParameters.PercentQuality, memoryStream);
            }
            memoryStream.Seek(0L, SeekOrigin.Begin);
            return memoryStream;
        }

        private Bitmap LoadScaledBitmap(Uri uri)
        {
            ContentResolver contentResolver = Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>().ApplicationContext.ContentResolver;
            var maxDimensionSize = GetMaximumDimension(contentResolver, uri);
            var sampleSize = (int)Math.Ceiling(maxDimensionSize / (double)_currentRequestParameters.MaxPixelDimension);
            if (sampleSize < 1)
            {
                // this shouldn't happen, but if it does... then trace the error and set sampleSize to 1
                MvxPluginLog.Instance.Trace(
                    "Warning - sampleSize of {0} was requested - how did this happen - based on requested {1} and returned image size {2}",
                    sampleSize,
                    _currentRequestParameters.MaxPixelDimension,
                    maxDimensionSize);
                // following from https://github.com/MvvmCross/MvvmCross/issues/565 we return null in this case
                // - it suggests that Android has returned a corrupt image uri
                return null;
            }
            var sampled = LoadResampledBitmap(contentResolver, uri, sampleSize);
            try
            {
                var rotated = ExifRotateBitmap(contentResolver, uri, sampled);
                return rotated;
            }
            catch (Exception pokemon)
            {
                MvxPluginLog.Instance.Trace("Problem seem in Exit Rotate {0}", pokemon.ToLongString());
                return sampled;
            }
        }

        private Bitmap LoadResampledBitmap(ContentResolver contentResolver, Uri uri, int sampleSize)
        {
            using (var inputStream = contentResolver.OpenInputStream(uri))
            {
                var optionsDecode = new BitmapFactory.Options { InSampleSize = sampleSize };

                return BitmapFactory.DecodeStream(inputStream, null, optionsDecode);
            }
        }

        private static int GetMaximumDimension(ContentResolver contentResolver, Uri uri)
        {
            using (var inputStream = contentResolver.OpenInputStream(uri))
            {
                var optionsJustBounds = new BitmapFactory.Options
                {
                    InJustDecodeBounds = true
                };
                var metadataResult = BitmapFactory.DecodeStream(inputStream, null, optionsJustBounds);
                var maxDimensionSize = Math.Max(optionsJustBounds.OutWidth, optionsJustBounds.OutHeight);
                return maxDimensionSize;
            }
        }

        private Bitmap ExifRotateBitmap(ContentResolver contentResolver, Uri uri, Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

            using (var inputStream = contentResolver.OpenInputStream(uri))
            {
                var exifInterface = new ExifInterface(inputStream);
                var orientation = exifInterface.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Normal);

                var rotationInDegrees = ExifToDegrees(orientation);
                if (rotationInDegrees == 0)
                    return bitmap;

                using (var matrix = new Matrix())
                {
                    matrix.PreRotate(rotationInDegrees);
                    return Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, matrix, true);
                }
            }
        }

        private static int ExifToDegrees(int exifOrientation)
        {
            switch (exifOrientation)
            {
                case (int)Orientation.Rotate90:
                    return 90;
                case (int)Orientation.Rotate180:
                    return 180;
                case (int)Orientation.Rotate270:
                    return 270;
            }

            return 0;
        }

        #region Nested type: RequestParameters

        private class RequestParameters
        {
            public RequestParameters(int maxPixelDimension, int percentQuality, Action<Stream, string> pictureAvailable,
                                     Action assumeCancelled)
            {
                PercentQuality = percentQuality;
                MaxPixelDimension = maxPixelDimension;
                AssumeCancelled = assumeCancelled;
                PictureAvailable = pictureAvailable;
            }

            public Action<Stream, string> PictureAvailable { get; private set; }
            public Action AssumeCancelled { get; private set; }
            public int MaxPixelDimension { get; private set; }
            public int PercentQuality { get; private set; }
        }

        #endregion
    }
}
