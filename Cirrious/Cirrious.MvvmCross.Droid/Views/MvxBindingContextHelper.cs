// MvxBindingActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxBindingContext 
        : IMvxBindingContext
    {
        private readonly Activity _droidContext;
        private readonly IMvxDataConsumer _dataConsumer;

        private readonly List<View> _boundViews = new List<View>();
        private readonly List<IMvxBinding> _bindings = new List<IMvxBinding>();

        public MvxBindingContext(Activity droidContext, IMvxDataConsumer dataConsumer)
        {
            _droidContext = droidContext;
            _dataConsumer = dataConsumer;
        }

        ~MvxBindingContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearAllBindings();
            }
        }

        public virtual void RegisterBindingsFor(View view)
        {
            if (view == null)
                return;

            _boundViews.Add(view);
        }

        public virtual void RegisterBinding(IMvxBinding binding)
        {
            _bindings.Add(binding);
        }

        public virtual void ClearBindings(View view)
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

        public virtual void ClearAllBindings()
        {
            var cleaner = new MvxBindingLayoutCleaner();
            _boundViews.ForEach(cleaner.Clean);
            _boundViews.Clear();
            _bindings.ForEach(b => b.Dispose());
            _bindings.Clear();
        }

        public virtual View BindingInflate(int resourceId, ViewGroup viewGroup)
        {
            var view = BindingInflate(_dataConsumer.DataContext, resourceId, viewGroup);
            return view;
        }

        public virtual View BindingInflate(object source, int resourceId, ViewGroup viewGroup)
        {
            var view = CommonInflate(
                resourceId,
                viewGroup,
                (layoutInflator) => new MvxBindingLayoutInflatorFactory(source, layoutInflator));
            RegisterBindingsFor(view);
            return view;
        }

        protected virtual View CommonInflate(int resourceId, ViewGroup viewGroup,
                                   Func<LayoutInflater, MvxBindingLayoutInflatorFactory> factoryProvider)
        {
            using (new MvxBindingContextStackRegistration(this))
            {
                var layoutInflator = _droidContext.LayoutInflater;
                using (var clone = layoutInflator.CloneInContext(_droidContext))
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
        }        
    }
}