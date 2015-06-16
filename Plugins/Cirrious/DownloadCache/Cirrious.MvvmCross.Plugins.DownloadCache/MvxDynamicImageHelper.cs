// MvxDynamicImageHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading.Tasks;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Plugins.DownloadCache
{
    public class MvxDynamicImageHelper<T>
        : IMvxImageHelper<T>
        where T : class
    {
        #region ImageState enum

        public enum ImageState
        {
            DefaultShown,
            ErrorShown,
            HttpImageShown
        }

        #endregion

        private MvxImageRequest<T> _currentImageRequest;

        private ImageState _currentImageState = ImageState.DefaultShown;

        private string _defaultImagePath;

        private string _errorImagePath;

        private string _imageUrl;

        public string DefaultImagePath
        {
            get { return _defaultImagePath; }
            set
            {
                if (_defaultImagePath == value)
                    return;
                _defaultImagePath = value;
                OnImagePathChanged();

                if (string.IsNullOrEmpty(_errorImagePath))
                    ErrorImagePath = value;
            }
        }

        public string ErrorImagePath
        {
            get { return _errorImagePath; }
            set
            {
                if (_errorImagePath == value)
                    return;
                _errorImagePath = value;
                OnImagePathChanged();
            }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                if (_imageUrl == value)
                    return;
                _imageUrl = value;
                RequestImage(_imageUrl);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        ~MvxDynamicImageHelper()
        {
            Dispose(false);
        }

        public event EventHandler<MvxValueEventArgs<T>> ImageChanged;
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }

        private void FireImageChanged(T image)
        {
            var handler = ImageChanged;
            if (handler != null)
                handler(this, new MvxValueEventArgs<T>(image));
        }

        private async Task RequestImage(string imageSource)
        {
            ClearCurrentHttpImageRequest();

            if (string.IsNullOrEmpty(imageSource))
            {
                ShowDefaultImage();
                return;
            }

            if (imageSource.ToUpper().StartsWith("HTTP"))
            {
                NewHttpImageRequested();

                _currentImageRequest = new MvxImageRequest<T>(imageSource);
                _currentImageRequest.Complete += CurrentImageRequestOnComplete;
                _currentImageRequest.Error += CurrentImageRequestOnError;
                _currentImageRequest.Start();
            }
            else
            {
                FireImageChanged(null);
                try
                {
                    var image = await ImageFromLocalFile(imageSource);

                    if (image == null)
                    {
                        await ShowErrorImage();
                    }
                    else
                    {
                        NewImageAvailable(image);
                    }
                }
                catch (Exception ex)
                {
                    Mvx.Error(ex.Message);
                }

            }
        }

        private void ClearCurrentHttpImageRequest()
        {
            if (_currentImageRequest == null)
                return;

            _currentImageRequest.Complete -= CurrentImageRequestOnComplete;
            _currentImageRequest.Error -= CurrentImageRequestOnError;

            _currentImageRequest = null;
        }

        private void CurrentImageRequestOnError(object sender, MvxValueEventArgs<Exception> mvxExceptionEventArgs)
        {
            if (sender != _currentImageRequest)
                return;

            Mvx.Trace("failed to download image {0} : {1}", _currentImageRequest.Url, mvxExceptionEventArgs.Value.ToLongString());

            HttpImageErrorSeen();
            ClearCurrentHttpImageRequest();
        }

        private void CurrentImageRequestOnComplete(object sender, MvxValueEventArgs<T> mvxValueEventArgs)
        {
            if (sender != _currentImageRequest)
                return;

            var image = mvxValueEventArgs.Value;
            NewImageAvailable(image);
            ClearCurrentHttpImageRequest();
        }

        private async Task OnImagePathChanged()
        {
            switch (_currentImageState)
            {
                case ImageState.ErrorShown:
                    await ShowErrorImage();
                    break;
                case ImageState.DefaultShown:
                    await ShowDefaultImage();
                    break;
            }
        }

        private async Task ShowDefaultImage()
        {
            await ShowLocalFile(_defaultImagePath);
        }

        private async Task ShowErrorImage()
        {
            await ShowLocalFile(_errorImagePath);
        }

        private async Task ShowLocalFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                FireImageChanged(null);
            }
            else
            {
                FireImageChanged(null);

                try
                {
                    var localImage = await ImageFromLocalFile(filePath);

                    if (localImage == null)
                    {
                        MvxTrace.Warning("Failed to load local image for filePath {0}", filePath);
                    }
                    FireImageChanged(localImage);
                }
                catch (Exception ex)
                {
                    Mvx.Error(ex.Message);
                }

            }
        }

        private async Task<T> ImageFromLocalFile(string path)
        {
            var loader = Mvx.Resolve<IMvxLocalFileImageLoader<T>>();
            var img = await loader.Load(path, true, MaxWidth, MaxHeight);
            return img.RawImage;

        }

        private void NewHttpImageRequested()
        {
            _currentImageState = ImageState.DefaultShown;
            ShowDefaultImage();
        }

        private void HttpImageErrorSeen()
        {
            _currentImageState = ImageState.ErrorShown;
            ShowErrorImage();
        }

        private void NewImageAvailable(T image)
        {
            _currentImageState = ImageState.HttpImageShown;
            FireImageChanged(image);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                ClearCurrentHttpImageRequest();
            }
        }
    }
}