// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using MvvmCross.Binding.Bindings;

namespace MvvmCross.Platforms.Android.Binding.Binders
{
    public class MvxBindingLayoutInflaterFactory
        : IMvxLayoutInflaterHolderFactory
    {
        private readonly object _source;

        private IMvxAndroidViewFactory _androidViewFactory;
        private IMvxAndroidViewBinder _binder;

        public MvxBindingLayoutInflaterFactory(object source)
        {
            _source = source;
        }

        protected virtual IMvxAndroidViewFactory AndroidViewFactory => _androidViewFactory ?? (_androidViewFactory = Mvx.IoCProvider.Resolve<IMvxAndroidViewFactory>());

        protected virtual IMvxAndroidViewBinder Binder => _binder ?? (_binder = Mvx.IoCProvider.Resolve<IMvxAndroidViewBinderFactory>().Create(_source));

        public virtual IList<KeyValuePair<object, IMvxUpdateableBinding>> CreatedBindings => Binder.CreatedBindings;

        public virtual View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            if (name == "fragment")
            {
                // MvvmCross does not inflate Fragments - instead it returns null and lets Android inflate them.
                return null;
            }

            View view = AndroidViewFactory.CreateView(parent, name, context, attrs);
            return BindCreatedView(view, context, attrs);
        }

        public virtual View BindCreatedView(View view, Context context, IAttributeSet attrs)
        {
            if (view != null)
                Binder.BindView(view, context, attrs);
            return view;
        }
    }
}
