// MvxBindingLayoutInflaterFactory.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Binders
{
    using System.Collections.Generic;

    using Android.Content;
    using Android.Util;
    using Android.Views;

    using MvvmCross.Binding.Bindings;
    using MvvmCross.Platform;

    public class MvxBindingLayoutInflaterFactory
        : IMvxLayoutInflaterHolderFactory
    {
        private readonly object _source;

        private IMvxAndroidViewFactory _androidViewFactory;
        private IMvxAndroidViewBinder _binder;

        public MvxBindingLayoutInflaterFactory(object source)
        {
            this._source = source;
        }

        protected virtual IMvxAndroidViewFactory AndroidViewFactory => this._androidViewFactory ?? (this._androidViewFactory = Mvx.Resolve<IMvxAndroidViewFactory>());

        protected virtual IMvxAndroidViewBinder Binder => this._binder ?? (this._binder = Mvx.Resolve<IMvxAndroidViewBinderFactory>().Create(this._source));

        public virtual IList<KeyValuePair<object, IMvxUpdateableBinding>> CreatedBindings => this.Binder.CreatedBindings;

        public virtual View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            if (name == "fragment")
            {
                // MvvmCross does not inflate Fragments - instead it returns null and lets Android inflate them.
                return null;
            }

            View view = this.AndroidViewFactory.CreateView(parent, name, context, attrs);
            return this.BindCreatedView(view, context, attrs);
        }

        public virtual View BindCreatedView(View view, Context context, IAttributeSet attrs)
        {
            if (view != null)
                this.Binder.BindView(view, context, attrs);
            return view;
        }
    }
}