// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.Bindings;

namespace MvvmCross.Binding.BindingContext
{
    /// <summary>
    /// OnDataContextChange executes asynchronously on a worker thread
    /// </summary>
    public class MvxTaskBasedBindingContext : IMvxBindingContext
    {
        private readonly List<Action> _delayedActions = new List<Action>();
        private readonly List<MvxBindingContext.TargetAndBinding> _directBindings = new List<MvxBindingContext.TargetAndBinding>();
        private readonly List<KeyValuePair<object, IList<MvxBindingContext.TargetAndBinding>>> _viewBindings = new List<KeyValuePair<object, IList<MvxBindingContext.TargetAndBinding>>>();
        private object _dataContext;
        private IMvxBinder _binder;

        public bool RunSynchronously { get; set; }

        public event EventHandler DataContextChanged;

        public IMvxBindingContext Init(object dataContext, object firstBindingKey, IEnumerable<MvxBindingDescription> firstBindingValue)
        {
            AddDelayedAction(firstBindingKey, firstBindingValue);
            if (dataContext != null)
                DataContext = dataContext;

            return this;
        }

        public IMvxBindingContext Init(object dataContext, object firstBindingKey, string firstBindingValue)
        {
            AddDelayedAction(firstBindingKey, firstBindingValue);
            if (dataContext != null)
                DataContext = dataContext;

            return this;
        }

        private void AddDelayedAction(object key, string value)
        {
            _delayedActions.Add(() =>
            {
                var bindings = Binder.Bind(DataContext, key, value);
                foreach (var b in bindings)
                    RegisterBinding(key, b);
            });
        }

        private void AddDelayedAction(object key, IEnumerable<MvxBindingDescription> value)
        {
            _delayedActions.Add(() =>
            {
                var bindings = Binder.Bind(DataContext, key, value);
                foreach (var b in bindings)
                    RegisterBinding(key, b);
            });
        }

        ~MvxTaskBasedBindingContext()
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

        protected IMvxBinder Binder
        {
            get
            {
                _binder = _binder ?? Mvx.IoCProvider.Resolve<IMvxBinder>();
                return _binder;
            }
        }

        public object DataContext
        {
            get
            {
                return _dataContext;
            }
            set
            {
                if (_dataContext == value)
                    return;

                _dataContext = value;
                OnDataContextChange();
                DataContextChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Must be called on main thread as it creates the target bindings, and creating target bindings might subscribe to events that
        /// needs to be done on main thread (like touchupinside). 
        /// If the code is run in Synchronous mode there will be a performance hit, there are however some use-cases(iOS automatic resizing cells).
        /// </summary>
        protected virtual void OnDataContextChange()
        {
            if (_delayedActions.Count != 0)
            {
                foreach (var action in _delayedActions)
                {
                    action();
                }
                _delayedActions.Clear();
            }

            // Copy the lists to ensure that if the main thread modifies the collection
            // once we are on the background thread we don't get an InvalidOperationException. 
            // Issue: #1398
            // View bindings need to be deep copied
            var viewBindingsCopy = _viewBindings.Select(vb => new KeyValuePair<object, IList<MvxBindingContext.TargetAndBinding>>(vb.Key, vb.Value.ToList()))
                                                     .ToList();

            var directBindingsCopy = _directBindings.ToList();

            Action setBindingsAction = () =>
            {
                foreach (var binding in viewBindingsCopy)
                {
                    foreach (var bind in binding.Value)
                    {
                        bind.Binding.DataContext = _dataContext;
                    }
                }

                foreach (var binding in directBindingsCopy)
                {
                    binding.Binding.DataContext = _dataContext;
                }
            };

            if (RunSynchronously)
                setBindingsAction();
            else 
                Task.Run(setBindingsAction);
        }

        public virtual void DelayBind(Action action)
        {
            _delayedActions.Add(action);
        }

        public virtual void RegisterBinding(object target, IMvxUpdateableBinding binding)
        {
            _directBindings.Add(new MvxBindingContext.TargetAndBinding(target, binding));
        }

        public virtual void RegisterBindingsWithClearKey(object clearKey, IEnumerable<KeyValuePair<object, IMvxUpdateableBinding>> bindings)
        {
            _viewBindings.Add(new KeyValuePair<object, IList<MvxBindingContext.TargetAndBinding>>(clearKey, bindings.Select(b => new MvxBindingContext.TargetAndBinding(b.Key, b.Value)).ToList()));
        }

        public virtual void RegisterBindingWithClearKey(object clearKey, object target, IMvxUpdateableBinding binding)
        {
            var list = new List<MvxBindingContext.TargetAndBinding> { new MvxBindingContext.TargetAndBinding(target, binding) };
            _viewBindings.Add(new KeyValuePair<object, IList<MvxBindingContext.TargetAndBinding>>(clearKey, list));
        }

        public virtual void ClearBindings(object clearKey)
        {
            if (clearKey == null)
                return;

            for (var i = _viewBindings.Count - 1; i >= 0; i--)
            {
                var candidate = _viewBindings[i];
                if (candidate.Key.Equals(clearKey))
                {
                    foreach (var binding in candidate.Value)
                    {
                        binding.Binding.Dispose();
                    }
                    _viewBindings.RemoveAt(i);
                }
            }
        }

        public virtual void ClearAllBindings()
        {
            ClearAllViewBindings();
            ClearAllDirectBindings();
            ClearAllDelayedBindings();
        }

        protected virtual void ClearAllDelayedBindings()
        {
            _delayedActions.Clear();
        }

        protected virtual void ClearAllDirectBindings()
        {
            foreach (var binding in _directBindings)
            {
                binding.Binding.Dispose();
            }
            _directBindings.Clear();
        }

        protected virtual void ClearAllViewBindings()
        {
            foreach (var kvp in _viewBindings)
            {
                foreach (var binding in kvp.Value)
                {
                    binding.Binding.Dispose();
                }
            }
            _viewBindings.Clear();
        }
    }
}