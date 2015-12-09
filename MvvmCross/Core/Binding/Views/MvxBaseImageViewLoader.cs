// MvxBaseImageViewLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Views
{
    using System;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.WeakSubscription;

    public abstract class MvxBaseImageViewLoader<TImage>
        : IDisposable
        where TImage : class
    {
        private readonly IMvxImageHelper<TImage> _imageHelper;
        private readonly Action<TImage> _imageSetAction;
        private IDisposable _subscription;

        protected MvxBaseImageViewLoader(Action<TImage> imageSetAction)
        {
            this._imageSetAction = imageSetAction;
            if (!Mvx.TryResolve(out this._imageHelper))
            {
                MvxBindingTrace.Error(
                    "Unable to resolve the image helper - have you referenced and called EnsureLoaded on the DownloadCache plugin?");
                return;
            }
            var eventInfo = this._imageHelper.GetType().GetEvent("ImageChanged");
            this._subscription = eventInfo.WeakSubscribe<TImage>(this._imageHelper, ImageHelperOnImageChanged);
        }

        ~MvxBaseImageViewLoader()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Note - this is public because we use it in weak referenced situations
        public virtual void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<TImage> mvxValueEventArgs)
        {
            this._imageSetAction(mvxValueEventArgs.Value);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._subscription != null)
                {
                    this._subscription.Dispose();
                    this._subscription = null;
                }
            }
        }

        public string ImageUrl
        {
            get { return this._imageHelper.ImageUrl; }
            set { this._imageHelper.ImageUrl = value; }
        }

        public string DefaultImagePath
        {
            get { return this._imageHelper.DefaultImagePath; }
            set { this._imageHelper.DefaultImagePath = value; }
        }

        public string ErrorImagePath
        {
            get { return this._imageHelper.ErrorImagePath; }
            set { this._imageHelper.ErrorImagePath = value; }
        }
    }
}