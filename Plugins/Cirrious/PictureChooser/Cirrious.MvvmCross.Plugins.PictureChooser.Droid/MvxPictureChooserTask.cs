// MvxPictureChooserTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Provider;
using Cirrious.CrossCore.Droid;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Uri = Android.Net.Uri;

namespace Cirrious.MvvmCross.Plugins.PictureChooser.Droid
{
    public class MvxPictureChooserTask
        : MvxAndroidTask
          , IMvxPictureChooserTask
          
    {
        private Uri _cachedUriLocation;
        private RequestParameters _currentRequestParameters;

        #region IMvxPictureChooserTask Members

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                             Action assumeCancelled)
        {
            var intent = new Intent(Intent.ActionGetContent);
            intent.SetType("image/*");
            ChoosePictureCommon(MvxIntentRequestCode.PickFromFile, intent, maxPixelDimension, percentQuality,
                                pictureAvailable, assumeCancelled);
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
                                pictureAvailable, assumeCancelled);
        }

        #endregion

        private Uri GetNewImageUri()
        {
            // Optional - specify some metadata for the picture
            var contentValues = new ContentValues();
            //contentValues.Put(MediaStore.Images.ImageColumnsConsts.Description, "A camera photo");

            // Specify where to put the image
            return
                Mvx.Resolve<IMvxAndroidGlobals>()
                    .ApplicationContext.ContentResolver.Insert(MediaStore.Images.Media.ExternalContentUri, contentValues);
        }

        public void ChoosePictureCommon(MvxIntentRequestCode pickId, Intent intent, int maxPixelDimension,
                                        int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            if (_currentRequestParameters != null)
                throw new MvxException("Cannot request a second picture while the first request is still pending");

            _currentRequestParameters = new RequestParameters(maxPixelDimension, percentQuality, pictureAvailable,
                                                              assumeCancelled);
            StartActivityForResult((int) pickId, intent);
        }

        protected override void ProcessMvxIntentResult(MvxIntentResultEventArgs result)
        {
            MvxTrace.Trace("ProcessMvxIntentResult started...");

            Uri uri;

            switch ((MvxIntentRequestCode) result.RequestCode)
            {
                case MvxIntentRequestCode.PickFromFile:
                    uri = (result.Data == null) ? null : result.Data.Data;
                    break;
                case MvxIntentRequestCode.PickFromCamera:
                    uri = _cachedUriLocation;
                    break;
                default:
                    // ignore this result - it's not for us
                    MvxTrace.Trace("Unexpected request received from MvxIntentResult - request was {0}",
                                   result.RequestCode);
                    return;
            }

            ProcessPictureUri(result, uri);
        }

        private void ProcessPictureUri(MvxIntentResultEventArgs result, Uri uri)
        {
            if (_currentRequestParameters == null)
            {
                MvxTrace.Error("Internal error - response received but _currentRequestParameters is null");
                return; // we have not handled this - so we return null
            }

            var responseSent = false;
            try
            {
                // Note for furture bug-fixing/maintenance - it might be better to use var outputFileUri = data.GetParcelableArrayExtra("outputFileuri") here?
                if (result.ResultCode != Result.Ok)
                {
                    MvxTrace.Trace("Non-OK result received from MvxIntentResult - {0} - request was {1}",
                                   result.ResultCode, result.RequestCode);
                    return;
                }

                if (uri == null
                    || string.IsNullOrEmpty(uri.Path))
                {
                    MvxTrace.Trace("Empty uri or file path received for MvxIntentResult");
                    return;
                }

                MvxTrace.Trace("Loading InMemoryBitmap started...");
                var memoryStream = LoadInMemoryBitmap(uri);
                MvxTrace.Trace("Loading InMemoryBitmap complete...");
                responseSent = true;
                MvxTrace.Trace("Sending pictureAvailable...");
                _currentRequestParameters.PictureAvailable(memoryStream);
                MvxTrace.Trace("pictureAvailable completed...");
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
            using (Bitmap bitmap = LoadScaledBitmap(uri))
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, _currentRequestParameters.PercentQuality, memoryStream);
            }
            memoryStream.Seek(0L, SeekOrigin.Begin);
            return memoryStream;
        }

        private Bitmap LoadScaledBitmap(Uri uri)
        {
            ContentResolver contentResolver = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext.ContentResolver;
            var maxDimensionSize = GetMaximumDimension(contentResolver, uri);
            var sampleSize = (int) Math.Ceiling((maxDimensionSize)/
                                                ((double) _currentRequestParameters.MaxPixelDimension));
            if (sampleSize < 1)
            {
                // this shouldn't happen, but if it does... then trace the error and set sampleSize to 1
                MvxTrace.Trace(
                    "Warning - sampleSize of {0} was requested - how did this happen - based on requested {1} and returned image size {2}",
                    sampleSize,
                    _currentRequestParameters.MaxPixelDimension,
                    maxDimensionSize);
                sampleSize = 1;
            }
            return LoadResampledBitmap(contentResolver, uri, sampleSize);
        }

        private Bitmap LoadResampledBitmap(ContentResolver contentResolver, Uri uri, int sampleSize)
        {
            using (var inputStream = contentResolver.OpenInputStream(uri))
            {
                var optionsDecode = new BitmapFactory.Options {InSampleSize = sampleSize};

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

        #region Nested type: RequestParameters

        private class RequestParameters
        {
            public RequestParameters(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                     Action assumeCancelled)
            {
                PercentQuality = percentQuality;
                MaxPixelDimension = maxPixelDimension;
                AssumeCancelled = assumeCancelled;
                PictureAvailable = pictureAvailable;
            }

            public Action<Stream> PictureAvailable { get; private set; }
            public Action AssumeCancelled { get; private set; }
            public int MaxPixelDimension { get; private set; }
            public int PercentQuality { get; private set; }
        }

        #endregion

        /*
        private void ResizeThenCallOnMainThread(int maxPixelDimension, int percentQuality, Stream input, Action<Stream> success)
        {
            ResizeJpegStream(maxPixelDimension, percentQuality, input, (stream) => CallAsync(stream, success));
        }

        private void ResizeJpegStream(int maxPixelDimension, int percentQuality, Stream input, Action<Stream> success)
        {
            var bitmap = new BitmapImage();
            bitmap.SetSource(input);
            var writeable = new WriteableBitmap(bitmap);
            var ratio = 1.0;
            if (writeable.PixelWidth > writeable.PixelHeight)
                ratio = ((double)maxPixelDimension) / ((double)writeable.PixelWidth);
            else
                ratio = ((double)maxPixelDimension) / ((double)writeable.PixelHeight);
            writeable.Resize((int)Math.Round(ratio * writeable.PixelWidth), (int)Math.Round(ratio * writeable.PixelHeight), WriteableBitmapExtensions.Interpolation.Bilinear);

            // not - important - we do *not* use using here - disposing of memoryStream is someone else's problem
            var memoryStream = new MemoryStream();
            writeable.SaveJpeg(memoryStream, writeable.PixelWidth, writeable.PixelHeight, 0, percentQuality);
            memoryStream.Seek(0L, SeekOrigin.Begin);
            success(memoryStream);
        }

        private void CallAsync(Stream input, Action<Stream> success)
        {
            Dispatcher.RequestMainThreadAction(() => success(input));
        }
         */
    }
}