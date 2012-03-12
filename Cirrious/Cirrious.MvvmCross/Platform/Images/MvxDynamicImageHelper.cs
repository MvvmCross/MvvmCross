#region Copyright
// <copyright file="MvxDynamicImageHelper.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Platform.Images
{
    public class MvxDynamicImageHelper<T>
        : IMvxServiceConsumer<IMvxLocalFileImageLoader<T>>
        , IDisposable
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

        private string _httpImageUrl;

        public string DefaultImagePath
        {
            get { return _defaultImagePath; }
            set
            {
                if (_defaultImagePath == value)
                    return;
                _defaultImagePath = value;
                OnDefaultImagePathChanged();

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
                OnErrorImagePathChanged();
            }
        }

        public string HttpImageUrl
        {
            get { return _httpImageUrl; }
            set
            {
                if (_httpImageUrl == value)
                    return;
                _httpImageUrl = value;
                RequestImage(_httpImageUrl);
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

        private void FireImageChanged(T image)
        {
            var handler = ImageChanged;
            if (handler != null)
                handler(this, new MvxValueEventArgs<T>(image));

#warning Need to think carefully here - not sure about IDisposable issues...
        }

        private void RequestImage(string httpImageSource)
        {
            ClearCurrentHttpImageRequest();

            if (string.IsNullOrEmpty(httpImageSource))
                return;

            NewHttpImageRequested();

            _currentImageRequest = new MvxImageRequest<T>(httpImageSource);
            _currentImageRequest.Complete += CurrentImageRequestOnComplete;
            _currentImageRequest.Error += CurrentImageRequestOnError;
            _currentImageRequest.Start();
        }

        private void ClearCurrentHttpImageRequest()
        {
            if (_currentImageRequest == null)
                return;

            _currentImageRequest.Complete -= CurrentImageRequestOnComplete;
            _currentImageRequest.Error -= CurrentImageRequestOnError;

            _currentImageRequest = null;
        }

        private void CurrentImageRequestOnError(object sender, MvxExceptionEventArgs mvxExceptionEventArgs)
        {
            if (sender != _currentImageRequest)
                return;

            HttpImageErrorSeen();
            ClearCurrentHttpImageRequest();
        }

        private void CurrentImageRequestOnComplete(object sender, MvxValueEventArgs<T> mvxValueEventArgs)
        {
            if (sender != _currentImageRequest)
                return;

            var image = mvxValueEventArgs.Value;
            NewHttpImageAvailable(image);
            ClearCurrentHttpImageRequest();
        }

        protected virtual void OnDefaultImagePathChanged()
        {
            switch (_currentImageState)
            {
                case ImageState.DefaultShown:
                    ShowDefaultImage();
                    break;
                case ImageState.ErrorShown:
                case ImageState.HttpImageShown:
                    // do nothing
                    break;
            }
        }

        private void OnErrorImagePathChanged()
        {
            switch (_currentImageState)
            {
                case ImageState.ErrorShown:
                    ShowErrorImage();
                    break;
                case ImageState.DefaultShown:
                case ImageState.HttpImageShown:
                    // do nothing
                    break;
            }
        }

        private void ShowDefaultImage()
        {
            ShowLocalFile(_defaultImagePath);
        }

        private void ShowErrorImage()
        {
            ShowLocalFile(_errorImagePath);
        }

        private void ShowLocalFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                FireImageChanged(null);
            }
            else
            {
                FireImageChanged(ImageFromLocalFile(filePath));
            }
        }

        private T ImageFromLocalFile(string path)
        {
            var loader = this.GetService<IMvxLocalFileImageLoader<T>>();
            var wrapped = loader.Load(path, true);
            return wrapped.RawImage;
        }

        private void NewHttpImageRequested()
        {
            _currentImageState = ImageState.DefaultShown;
            ShowDefaultImage();
        }

        private void HttpImageErrorSeen()
        {
            _currentImageState = ImageState.ErrorShown;
            ShowDefaultImage();
        }

        private void NewHttpImageAvailable(T image)
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