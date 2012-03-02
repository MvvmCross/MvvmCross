#region Copyright
// <copyright file="MvxPictureChooserTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Provider;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.Platform.Tasks;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Uri = Android.Net.Uri;

namespace Cirrious.MvvmCross.Android.Platform.Tasks
{
    public class MvxPictureChooserTask 
        : MvxAndroidTask
        , IMvxPictureChooserTask
        , IMvxServiceConsumer<IMvxAndroidGlobals>
        , IMvxServiceConsumer<IMvxSimpleFileStoreService>
    {
        private Uri _cachedUriLocation;
        private RequestParameters _currentRequestParameters;

        #region IMvxPictureChooserTask Members

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            var intent = new Intent(Intent.ActionGetContent);
            intent.SetType("image/*");
            ChoosePictureCommon(MvxIntentRequestCode.PickFromFile, intent, maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            var intent = new Intent(MediaStore.ActionImageCapture);

            _cachedUriLocation = GetNewImageUri();
            intent.PutExtra(MediaStore.ExtraOutput, _cachedUriLocation);
            intent.PutExtra("outputFormat", Bitmap.CompressFormat.Jpeg.ToString());
            intent.PutExtra("return-data", true);

            ChoosePictureCommon(MvxIntentRequestCode.PickFromCamera, intent, maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        #endregion

        private Uri GetNewImageUri()
        {
            // Optional - specify some metadata for the picture
            var contentValues = new ContentValues();
            //contentValues.Put(MediaStore.Images.ImageColumnsConsts.Description, "A camera photo");

            // Specify where to put the image
            return this.GetService<IMvxAndroidGlobals>().ApplicationContext.ContentResolver.Insert(MediaStore.Images.Media.ExternalContentUri, contentValues);
        }

        public void ChoosePictureCommon(MvxIntentRequestCode pickId, Intent intent, int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
        {
            if (_currentRequestParameters != null)
                throw new MvxException("Cannot request a second picture while the first request is still pending");

            _currentRequestParameters = new RequestParameters(maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
            StartActivityForResult((int)pickId, intent);
        }

        protected override bool ProcessMvxIntentResult(MvxIntentResultEventArgs result)
        {
            Uri uri;

            switch ((MvxIntentRequestCode)result.RequestCode)
            {
                case MvxIntentRequestCode.PickFromFile:
                    uri = (result.Data == null) ? null : result.Data.Data;
                    break;
                case MvxIntentRequestCode.PickFromCamera:
                    uri = _cachedUriLocation; 
                    break;
                default:
                    // ignore this result - it's not for us
                    MvxTrace.Trace("Unexpected request received from MvxIntentResult - request was {0}", result.RequestCode);
                    return base.ProcessMvxIntentResult(result);
            }

            return ProcessPictureUri(result, uri);
        }

        private bool ProcessPictureUri(MvxIntentResultEventArgs result, Uri uri)
        {
            if (_currentRequestParameters == null)
            {
                MvxTrace.Trace("Internal error - response received but _currentRequestParameters is null");
                return false; // we have not handled this - so we return null
            }

            var responseSent = false;
            try
            {
                // Note for furture bug-fixing/maintenance - it might be better to use var outputFileUri = data.GetParcelableArrayExtra("outputFileuri") here?
                if (result.ResultCode != Result.Ok)
                {
                    MvxTrace.Trace("Non-OK result received from MvxIntentResult - {0} - request was {1}",
                                   result.ResultCode, result.RequestCode);
                    return true;
                }

                if (uri == null
                    || string.IsNullOrEmpty(uri.Path))
                {
                    MvxTrace.Trace("Empty uri or file path received for MvxIntentResult");
                    return true;
                }

                var memoryStream = LoadInMemoryBitmap(uri);
                responseSent = true;
                _currentRequestParameters.PictureAvailable(memoryStream);
                return true;
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
            ContentResolver contentResolver = this.GetService<IMvxAndroidGlobals>().ApplicationContext.ContentResolver;
            var maxDimensionSize = GetMaximumDimension(contentResolver, uri);
            var sampleSize = (int)Math.Ceiling(((double) maxDimensionSize)/
                                     ((double) _currentRequestParameters.MaxPixelDimension));
            if (sampleSize < 1)
            {
                // this shouldn't happen, but if it does... then trace the error and set sampleSize to 1
                MvxTrace.Trace("Warning - sampleSize of {0} was requested - how did this happen - based on requested {1} and returned image size {2}", 
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
                var optionsJustBounds = new BitmapFactory.Options()
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
            public RequestParameters(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
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
            ViewDispatcher.RequestMainThreadAction(() => success(input));
        }
         */
    }
}