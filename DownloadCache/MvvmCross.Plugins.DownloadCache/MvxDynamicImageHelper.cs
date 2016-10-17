// MvxDynamicImageHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace MvvmCross.Plugins.DownloadCache
{
    [Preserve(AllMembers = true)]
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

        #endregion ImageState enum

        private ImageState _currentImageState = ImageState.DefaultShown;

		private CancellationTokenSource _cancellationSource;

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
                OnImagePathChanged().ConfigureAwait(false);

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
                OnImagePathChanged().ConfigureAwait(false);
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
                RequestImageAsync(_imageUrl).ConfigureAwait(false);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members

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
            handler?.Invoke(this, new MvxValueEventArgs<T>(image));
        }

        private async Task RequestImageAsync(string imageSource)
        {
			if (_cancellationSource != null) 
			{
				_cancellationSource.Cancel ();
				_cancellationSource = null;
			}

			var cancelTokenSource = new CancellationTokenSource ();
			var cancelToken = cancelTokenSource.Token;
			_cancellationSource = cancelTokenSource;

            FireImageChanged(null);

            if (string.IsNullOrEmpty(imageSource))
            {
                await ShowDefaultImage().ConfigureAwait(false);
                return;
            }

            if (imageSource.ToUpper().StartsWith("HTTP"))
            {
                await NewHttpImageRequestedAsync().ConfigureAwait(false);

                var error = false;
                try
                {
                    var cache = Mvx.Resolve<IMvxImageCache<T>>();
                    var image = await cache.RequestImage(imageSource).ConfigureAwait(false);

					if (cancelToken.IsCancellationRequested) {
						return;
					}

                    if (image == null)
                        await ShowErrorImage().ConfigureAwait(false);
                    else
                        NewImageAvailable(image);
                }
                catch (Exception ex)
                {
                    Mvx.Trace("failed to download image {0} : {1}", imageSource, ex.ToLongString());
                    error = true;
                }

                if (error)
                    await HttpImageErrorSeenAsync().ConfigureAwait(false);
            }
            else
            {
                try
                {
                    var image = await ImageFromLocalFileAsync(imageSource).ConfigureAwait(false);

					if (cancelToken.IsCancellationRequested) {
						return;
					}

                    if (image == null)
                        await ShowErrorImage().ConfigureAwait(false);
                    else
                        NewImageAvailable(image);
                }
                catch (Exception ex)
                {
                    Mvx.Error(ex.Message);
                }
            }
        }

        private Task OnImagePathChanged()
        {
            switch (_currentImageState)
            {
                case ImageState.ErrorShown:
                    return ShowErrorImage();

                default:
                    return ShowDefaultImage();
            }
        }

        private Task ShowDefaultImage()
        {
            return ShowLocalFileAsync(_defaultImagePath);
        }

        private Task ShowErrorImage()
        {
            return ShowLocalFileAsync(_errorImagePath);
        }

        private async Task ShowLocalFileAsync(string filePath)
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
                    var localImage = await ImageFromLocalFileAsync(filePath).ConfigureAwait(false);
                    if (localImage == null)
                        MvxTrace.Warning("Failed to load local image for filePath {0}", filePath);

                    FireImageChanged(localImage);
                }
                catch (Exception ex)
                {
                    Mvx.Error(ex.Message);
                }
            }
        }

        private async Task<T> ImageFromLocalFileAsync(string path)
        {
            var loader = Mvx.Resolve<IMvxLocalFileImageLoader<T>>();
            var img = await loader.Load(path, true, MaxWidth, MaxHeight).ConfigureAwait(false);
            return img.RawImage;
        }

        private Task NewHttpImageRequestedAsync()
        {
            _currentImageState = ImageState.DefaultShown;
            return ShowDefaultImage();
        }

        private Task HttpImageErrorSeenAsync()
        {
            _currentImageState = ImageState.ErrorShown;
            return ShowErrorImage();
        }

        private void NewImageAvailable(T image)
        {
            _currentImageState = ImageState.HttpImageShown;
            FireImageChanged(image);
        }

        protected virtual void Dispose(bool isDisposing)
        {
        }
    }
}