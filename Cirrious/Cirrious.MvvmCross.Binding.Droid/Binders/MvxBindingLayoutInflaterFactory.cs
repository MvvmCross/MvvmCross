// MvxBindingLayoutInflaterFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Bindings;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    public class MvxBindingLayoutInflaterFactory
        : IMvxLayoutInflaterHolderFactory
    {
        private readonly object _source;

        private IMvxAndroidViewFactory _androidViewFactory;
        private IMvxAndroidViewBinder _binder;

        public MvxBindingLayoutInflaterFactory(
            object source)
        {
            _source = source;
        }

        protected virtual IMvxAndroidViewFactory AndroidViewFactory => _androidViewFactory ?? (_androidViewFactory = Mvx.Resolve<IMvxAndroidViewFactory>());

        protected virtual IMvxAndroidViewBinder Binder => _binder ?? (_binder = Mvx.Resolve<IMvxAndroidViewBinderFactory>().Create(_source));

        public virtual IList<KeyValuePair<object,IMvxUpdateableBinding>> CreatedBindings => Binder.CreatedBindings;

        public virtual View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            if (name == "fragment")
            {
                // MvvmCross does not inflate Fragments - instead it returns null and lets Android inflate them.
                return null;
            }

            View view = AndroidViewFactory.CreateView(parent, name, context, attrs);
            return this.BindCreatedView(view, context, attrs);
        }

        public virtual View BindCreatedView(View view, Context context, IAttributeSet attrs)
        {
            if (view != null)
                Binder.BindView(view, context, attrs);
            return view;
        }
    }
}