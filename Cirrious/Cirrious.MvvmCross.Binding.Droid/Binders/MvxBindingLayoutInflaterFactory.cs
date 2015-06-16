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

        protected IMvxAndroidViewFactory AndroidViewFactory
        {
            get
            {
                if (_androidViewFactory == null)
                    _androidViewFactory = Mvx.Resolve<IMvxAndroidViewFactory>();
                return _androidViewFactory;
            }
        }

        protected IMvxAndroidViewBinder Binder
        {
            get
            {
                if (_binder == null)
                    _binder = Mvx.Resolve<IMvxAndroidViewBinderFactory>().Create(_source);
                return _binder;
            }
        }

        public IList<KeyValuePair<object,IMvxUpdateableBinding>> CreatedBindings
        {
            get { return Binder.CreatedBindings; }
        }

        public View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            if (name == "fragment")
            {
                // MvvmCross does not inflate Fragments - instead it returns null and lets Android inflate them.
                return null;
            }

            View view = AndroidViewFactory.CreateView(parent, name, context, attrs);
            return this.BindCreatedView(view, context, attrs);
        }

        public View BindCreatedView(View view, Context context, IAttributeSet attrs)
        {
            if (view != null)
                Binder.BindView(view, context, attrs);
            return view;
        }
    }
}