// MvxBindingMapActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Droid.Maps
{
    public abstract class MvxBindingMapActivityView<TViewModel>
        : MvxMapActivityView<TViewModel>
          , IMvxBindingActivity
        where TViewModel : class, IMvxViewModel
    {
        #region Code shared across all binding activities - I hate this cut and paste

        private readonly List<View> _boundViews = new List<View>();
        private readonly List<IMvxBinding> _bindings = new List<IMvxBinding>();

        protected override void OnCreate(Bundle bundle)
        {
            ClearAllBindings();
            base.OnCreate(bundle);
        }

        protected override void OnDestroy()
        {
            ClearAllBindings();
            base.OnDestroy();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ClearAllBindings();
            base.Dispose(disposing);
        }

        public void ClearBindings(View view)
        {
            if (view == null)
                return;

            var cleaner = new MvxBindingLayoutCleaner();
            cleaner.Clean(view);
            for (var i = 0; i < _boundViews.Count; i++)
            {
                if (_boundViews[i] == view)
                {
                    _boundViews.RemoveAt(i);
                    break;
                }
            }
        }

        private void ClearAllBindings()
        {
            var cleaner = new MvxBindingLayoutCleaner();
            _boundViews.ForEach(cleaner.Clean);
            _boundViews.Clear();
            _bindings.ForEach(b => b.Dispose());
            _bindings.Clear();
        }

        public void RegisterBinding(IMvxBinding binding)
        {
            _bindings.Add(binding);
        }

        public void RegisterBindingsFor(View view)
        {
            if (view == null)
                return;

            _boundViews.Add(view);
        }

        public override LayoutInflater LayoutInflater
        {
            get
            {
                throw new InvalidOperationException(
                    "LayoutInflater must not be accessed directly in MvxBindingActivityView - use IMvxBindingActivity.BindingInflate or IMvxBindingActivity.NonBindingInflate instead");
            }
        }

        public virtual object DefaultBindingSource
        {
            get { return ViewModel; }
        }

        public View BindingInflate(int resourceId, ViewGroup viewGroup)
        {
            var view = BindingInflate(DefaultBindingSource, resourceId, viewGroup);
            return view;
        }

        public View BindingInflate(object source, int resourceId, ViewGroup viewGroup)
        {
            var view = CommonInflate(
                resourceId,
                viewGroup,
                (layoutInflator) => new MvxBindingLayoutInflatorFactory(source, layoutInflator));
            RegisterBindingsFor(view);
            return view;
        }

        public View NonBindingInflate(int resourceId, ViewGroup viewGroup)
        {
            return CommonInflate(
                resourceId,
                viewGroup,
                (layoutInflator) => null);
        }

        public override void SetContentView(int layoutResId)
        {
            var view = BindingInflate(ViewModel, layoutResId, null);
            SetContentView(view);
        }

        private View CommonInflate(int resourceId, ViewGroup viewGroup,
                                   Func<LayoutInflater, MvxBindingLayoutInflatorFactory> factoryProvider)
        {
            var layoutInflator = base.LayoutInflater;
            using (var clone = layoutInflator.CloneInContext(this))
            {
                using (var factory = factoryProvider(clone))
                {
                    if (factory != null)
                        clone.Factory = factory;
                    var toReturn = clone.Inflate(resourceId, viewGroup);
                    if (factory != null)
                    {
                        factory.StoreBindings(toReturn);
                    }
                    return toReturn;
                }
            }
        }

        #endregion
    }
}