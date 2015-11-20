// MvxBaseImageViewLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.WeakSubscription;
using System;

namespace Cirrious.MvvmCross.Binding.Views
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
                MvxBindingTrace.Error(
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