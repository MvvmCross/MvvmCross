// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Binding.Views
{
    public abstract class MvxBaseImageViewLoader<TImage>
        : IDisposable
        where TImage : class
    {
        private readonly IMvxImageHelper<TImage> _imageHelper;
        private readonly Action<TImage> _imageSetAction;
        private IDisposable _subscription;

        protected MvxBaseImageViewLoader(Action<TImage> imageSetAction)
        {
            _imageSetAction = imageSetAction;
            if (!Mvx.TryResolve(out _imageHelper))
            {
                MvxBindingLog.Error(
                    "Unable to resolve the image helper - have you referenced and called EnsureLoaded on the DownloadCache plugin?");
                return;
            }
            var eventInfo = _imageHelper.GetType().GetEvent("ImageChanged");
            _subscription = eventInfo.WeakSubscribe<TImage>(_imageHelper, ImageHelperOnImageChanged);
        }

        ~MvxBaseImageViewLoader()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Note - this is public because we use it in weak referenced situations
        public virtual void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<TImage> mvxValueEventArgs)
        {
            _imageSetAction(mvxValueEventArgs.Value);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }
            }
        }

        public string ImageUrl
        {
            get { return _imageHelper.ImageUrl; }
            set { _imageHelper.ImageUrl = value; }
        }

        public string DefaultImagePath
        {
            get { return _imageHelper.DefaultImagePath; }
            set { _imageHelper.DefaultImagePath = value; }
        }

        public string ErrorImagePath
        {
            get { return _imageHelper.ErrorImagePath; }
            set { _imageHelper.ErrorImagePath = value; }
        }
    }
}