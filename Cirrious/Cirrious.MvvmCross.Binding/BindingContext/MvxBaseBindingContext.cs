// MvxBaseBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public class MvxBaseBindingContext<TViewType>
        : IMvxBaseBindingContext<TViewType>
        where TViewType : class
    {
        private readonly List<IMvxUpdateableBinding> _directBindings = new List<IMvxUpdateableBinding>();

        private readonly List<KeyValuePair<TViewType, IList<IMvxUpdateableBinding>>> _viewBindings =
            new List<KeyValuePair<TViewType, IList<IMvxUpdateableBinding>>>();

        private object _dataContext;

        protected MvxBaseBindingContext(object dataContext)
        {
            _dataContext = dataContext;
        }

        ~MvxBaseBindingContext()
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

        public object DataContext
        {
            get { return _dataContext; }
            set
            {
                _dataContext = value;
                OnDataContextChange();
            }
        }

        protected virtual void OnDataContextChange()
        {
            foreach (var binding in _viewBindings)
            {
                foreach (var bind in binding.Value)
                {
                    bind.DataContext = _dataContext;
                }
            }
        }

        public virtual void RegisterBinding(IMvxUpdateableBinding binding)
        {
            _directBindings.Add(binding);
        }

        public virtual void RegisterBindingsFor(TViewType view, IList<IMvxUpdateableBinding> bindings)
        {
            if (view == null)
                return;

            _viewBindings.Add(new KeyValuePair<TViewType, IList<IMvxUpdateableBinding>>(view, bindings));
        }

        public virtual void ClearBindings(TViewType view)
        {
            if (view == null)
                return;

            for (var i = _viewBindings.Count - 1; i >= 0; i--)
            {
                var candidate = _viewBindings[i];
                if (candidate.Key == view)
                {
                    foreach (var binding in candidate.Value)
                    {
                        binding.Dispose();
                    }
                    _viewBindings.RemoveAt(i);
                    break;
                }
            }
        }

        public virtual void ClearAllBindings()
        {
            ClearAllViewBindings();
            ClearAllDirectBindings();
        }

        protected virtual void ClearAllDirectBindings()
        {
            foreach (var binding in _directBindings)
            {
                binding.Dispose();
            }
            _directBindings.Clear();
        }

        protected virtual void ClearAllViewBindings()
        {
            foreach (var kvp in _viewBindings)
            {
                foreach (var binding in kvp.Value)
                {
                    binding.Dispose();
                }
            }
            _viewBindings.Clear();
        }
    }
}