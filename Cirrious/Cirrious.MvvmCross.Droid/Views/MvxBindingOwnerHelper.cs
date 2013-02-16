// MvxBindingActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxBindingOwnerHelper : IMvxBindingOwnerHelper
    {
        private readonly List<View> _boundViews = new List<View>();
        private readonly List<IMvxBinding> _bindings = new List<IMvxBinding>();
        private readonly Context _context;
        private readonly IMvxLayoutInflaterProvider _layoutInflaterProvider;
        private readonly IDataContext _dataContext;

        public MvxBindingOwnerHelper(Context context, IMvxLayoutInflaterProvider layoutInflaterProvider, IDataContext dataContext)
        {
            _context = context;
            _layoutInflaterProvider = layoutInflaterProvider;
            _dataContext = dataContext;
        }

        ~MvxBindingOwnerHelper()
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

        public void RegisterBindingsFor(View view)
        {
            if (view == null)
                return;

            _boundViews.Add(view);
        }

        public void RegisterBinding(IMvxBinding binding)
        {
            _bindings.Add(binding);
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

        public void ClearAllBindings()
        {
            var cleaner = new MvxBindingLayoutCleaner();
            _boundViews.ForEach(cleaner.Clean);
            _boundViews.Clear();
            _bindings.ForEach(b => b.Dispose());
            _bindings.Clear();
        }

        public View BindingInflate(int resourceId, ViewGroup viewGroup)
        {
            var view = BindingInflate(_dataContext.DataContext, resourceId, viewGroup);
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

        private View CommonInflate(int resourceId, ViewGroup viewGroup,
                                   Func<LayoutInflater, MvxBindingLayoutInflatorFactory> factoryProvider)
        {
            var layoutInflator = _layoutInflaterProvider.LayoutInflater;
            using (var clone = layoutInflator.CloneInContext(_context))
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