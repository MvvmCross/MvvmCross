// MvxBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public class MvxBindingContext
        : IMvxBindingContext
    {
        private readonly List<Action> _callOnNextDataContextChange = new List<Action>();

        private readonly List<IMvxUpdateableBinding> _directBindings = new List<IMvxUpdateableBinding>();

        private readonly List<KeyValuePair<object, IList<IMvxUpdateableBinding>>> _viewBindings =
            new List<KeyValuePair<object, IList<IMvxUpdateableBinding>>>();

        private object _dataContext;

        public MvxBindingContext(object dataContext = null)
        {
            _dataContext = dataContext;
        }

        public MvxBindingContext(IDictionary<object, string> firstBindings)
            : this(null, firstBindings)
        {
        }

        public MvxBindingContext(object dataContext, IDictionary<object, string> firstBindings)
        {
            foreach (var kvp in firstBindings)
            {
                _callOnNextDataContextChange.Add(() =>
                    {
                        var bindings = Binder.Bind(DataContext, kvp.Key, kvp.Value);
                        foreach (var b in bindings)
                            RegisterBinding(b);
                    });
            }
            if (dataContext != null)
                DataContext = dataContext;
        }

        public MvxBindingContext(IDictionary<object, IEnumerable<MvxBindingDescription>> firstBindings)
            : this(null, firstBindings)
        {
        }

        public MvxBindingContext(object dataContext,
                                 IDictionary<object, IEnumerable<MvxBindingDescription>> firstBindings)
        {
            foreach (var kvp in firstBindings)
            {
                _callOnNextDataContextChange.Add(() =>
                    {
                        var bindings = Binder.Bind(DataContext, kvp.Key, kvp.Value);
                        foreach (var b in bindings)
                            RegisterBinding(b);
                    });
            }
            if (dataContext != null)
                DataContext = dataContext;
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

        private IMvxBinder _binder;

        protected IMvxBinder Binder
        {
            get
            {
                _binder = _binder ?? Mvx.Resolve<IMvxBinder>();
                return _binder;
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
            // update existing bindings
            foreach (var binding in _viewBindings)
            {
                foreach (var bind in binding.Value)
                {
                    bind.DataContext = _dataContext;
                }
            }

            foreach (var binding in _directBindings)
            {
                binding.DataContext = _dataContext;
            }

            // add new bindings
            if (_callOnNextDataContextChange.Count == 0)
            {
                return;
            }

            foreach (var action in _callOnNextDataContextChange)
            {
                action();
            }
            _callOnNextDataContextChange.Clear();
        }

        public virtual void DoOnNextDataContextChange(Action action)
        {
            _callOnNextDataContextChange.Add(action);
        }

        public virtual void RegisterBinding(IMvxUpdateableBinding binding)
        {
            _directBindings.Add(binding);
        }

        public virtual void RegisterBindingsFor(object target, IList<IMvxUpdateableBinding> bindings)
        {
            if (target == null)
                return;

            _viewBindings.Add(new KeyValuePair<object, IList<IMvxUpdateableBinding>>(target, bindings));
        }

        public virtual void ClearBindings(object view)
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