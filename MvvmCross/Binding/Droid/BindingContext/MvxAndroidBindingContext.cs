// MvxAndroidBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.BindingContext
{
    using System;

    using Android.Content;
    using Android.Views;

    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Droid.Views;

    public class MvxAndroidBindingContext
        : MvxBindingContext, IMvxAndroidBindingContext
    {
        private readonly Context _droidContext;
        private IMvxLayoutInflaterHolder _layoutInflaterHolder;

        public MvxAndroidBindingContext(Context droidContext, IMvxLayoutInflaterHolder layoutInflaterHolder, object source = null)
            : base(source)
        {
            this._droidContext = droidContext;
            this._layoutInflaterHolder = layoutInflaterHolder;
        }

        public IMvxLayoutInflaterHolder LayoutInflaterHolder
        {
            get { return this._layoutInflaterHolder; }
            set { this._layoutInflaterHolder = value; }
        }

        public virtual View BindingInflate(int resourceId, ViewGroup viewGroup)
        {
            return this.BindingInflate(resourceId, viewGroup, true);
        }

        public virtual View BindingInflate(int resourceId, ViewGroup viewGroup, bool attachToRoot)
        {
            var view = this.CommonInflate(
                resourceId,
                viewGroup,
                attachToRoot);
            return view;
        }

        [Obsolete("Switch to new CommonInflate method - with additional attachToRoot parameter")]
        protected virtual View CommonInflate(int resourceId, ViewGroup viewGroup)
        {
            return this.CommonInflate(resourceId, viewGroup, viewGroup != null);
        }

        protected virtual View CommonInflate(int resourceId, ViewGroup viewGroup, bool attachToRoot)
        {
            using (new MvxBindingContextStackRegistration<IMvxAndroidBindingContext>(this))
            {
                var layoutInflater = this._layoutInflaterHolder.LayoutInflater;
                {
                    // This is most likely a MvxLayoutInflater but it doesn't have to be.
                    // It handles setting the bindings and interacts with this instance of
                    // MvxAndroidBindingContext through the use of MvxAndroidBindingContextHelpers.Current().
                    return layoutInflater.Inflate(resourceId, viewGroup, attachToRoot);
                }
            }
        }
    }
}