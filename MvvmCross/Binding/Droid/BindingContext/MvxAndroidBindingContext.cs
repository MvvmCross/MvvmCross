// MvxAndroidBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using System;

namespace Cirrious.MvvmCross.Binding.Droid.BindingContext
{
    public class MvxAndroidBindingContext
        : MvxBindingContext, IMvxAndroidBindingContext
    {
        private readonly Context _droidContext;
        private IMvxLayoutInflaterHolder _layoutInflaterHolder;

        public MvxAndroidBindingContext(Context droidContext, IMvxLayoutInflaterHolder layoutInflaterHolder, object source = null)
            : base(source)
        {
            _droidContext = droidContext;
            this._layoutInflaterHolder = layoutInflaterHolder;
        }

        public IMvxLayoutInflaterHolder LayoutInflaterHolder
        {
            get { return this._layoutInflaterHolder; }
            set { this._layoutInflaterHolder = value; }
        }

        public virtual View BindingInflate(int resourceId, ViewGroup viewGroup)
        {
            return BindingInflate(resourceId, viewGroup, true);
        }

        public virtual View BindingInflate(int resourceId, ViewGroup viewGroup, bool attachToRoot)
        {
            var view = CommonInflate(
                resourceId,
                viewGroup,
                attachToRoot);
            return view;
        }

        [Obsolete("Switch to new CommonInflate method - with additional attachToRoot parameter")]
        protected virtual View CommonInflate(int resourceId, ViewGroup viewGroup)
        {
            return CommonInflate(resourceId, viewGroup, viewGroup != null);
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