// MvxAndroidBindingContext.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.BindingContext
{
    using System;

    using Android.Content;
    using Android.Views;

    using Binding.BindingContext;
    using Views;

    public class MvxAndroidBindingContext
        : MvxBindingContext, IMvxAndroidBindingContext
    {
        private readonly Context _droidContext;
        private IMvxLayoutInflaterHolder _layoutInflaterHolder;

        public MvxAndroidBindingContext(Context droidContext, IMvxLayoutInflaterHolder layoutInflaterHolder, object source = null)
            : base(source)
        {
            _droidContext = droidContext;
            _layoutInflaterHolder = layoutInflaterHolder;
        }

        public IMvxLayoutInflaterHolder LayoutInflaterHolder
        {
            get { return _layoutInflaterHolder; }
            set { _layoutInflaterHolder = value; }
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

        protected virtual View CommonInflate(int resourceId, ViewGroup viewGroup, bool attachToRoot)
        {
            using (new MvxBindingContextStackRegistration<IMvxAndroidBindingContext>(this))
            {
                var layoutInflater = _layoutInflaterHolder.LayoutInflater;
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