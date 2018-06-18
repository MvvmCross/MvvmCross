// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.Bindings;

namespace MvvmCross.Binding.BindingContext
{
    public class MvxBindingContext : IMvxBindingContext
    {
        public class TargetAndBinding
        {
            public TargetAndBinding(object target, IMvxUpdateableBinding binding)
            {
                Target = target;
                Binding = binding;
            }

            public object Target { get; private set; }
            public IMvxUpdateableBinding Binding { get; }
        }

        private readonly List<Action> _delayedActions = new List<Action>();

        private readonly List<TargetAndBinding> _directBindings = new List<TargetAndBinding>();

        private readonly List<KeyValuePair<object, IList<TargetAndBinding>>> _viewBindings =
            new List<KeyValuePair<object, IList<TargetAndBinding>>>();

        private object _dataContext;

        public MvxBindingContext()
            : this((object)null)
        {
        }

        public MvxBindingContext(object dataContext)
        {
            _dataContext = dataContext;
        }

        public MvxBindingContext(IDictionary<object, string> firstBindings)
        {
            Init(null, firstBindings);
        }

        public MvxBindingContext(object dataContext, IDictionary<object, string> firstBindings)
        {
            Init(dataContext, firstBindings);
        }

        public MvxBindingContext(IDictionary<object, IEnumerable<MvxBindingDescription>> firstBindings)
        {
            Init(null, firstBindings);
        }

        public MvxBindingContext(object dataContext, IDictionary<object, IEnumerable<MvxBindingDescription>> firstBindings)
        {
            Init(dataContext, firstBindings);
        }

        public MvxBindingContext Init(object dataContext, IDictionary<object, IEnumerable<MvxBindingDescription>> firstBindings)
        {
            foreach (var kvp in firstBindings)
            {
                AddDelayedAction(kvp);
            }
            if (dataContext != null)
                DataContext = dataContext;

            return this;
        }

        public MvxBindingContext Init(object dataContext, IDictionary<object, string> firstBindings)
        {
            foreach (var kvp in firstBindings)
            {
                AddDelayedAction(kvp);
            }
            if (dataContext != null)
                DataContext = dataContext;

            return this;
        }

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

        private void AddDelayedAction(KeyValuePair<object, string> kvp)
        {
            _delayedActions.Add(() =>
            {
                var bindings = Binder.Bind(DataContext, kvp.Key, kvp.Value);
                foreach (var b in bindings)
                    RegisterBinding(kvp.Key, b);
            });
        }

        private void AddDelayedAction(KeyValuePair<object, IEnumerable<MvxBindingDescription>> kvp)
        {
            _delayedActions.Add(() =>
            {
                var bindings = Binder.Bind(DataContext, kvp.Key, kvp.Value);
                foreach (var b in bindings)
                    RegisterBinding(kvp.Key, b);
            });
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

        public event EventHandler DataContextChanged;

        protected virtual void OnDataContextChange()
        {
            // update existing bindings
            foreach (var binding in _viewBindings)
            {
                foreach (var bind in binding.Value)
                {
                    bind.Binding.DataContext = _dataContext;
                }
            }

            foreach (var binding in _directBindings)
            {
                binding.Binding.DataContext = _dataContext;
            }

            // add new bindings
            if (_delayedActions.Count == 0)
            {
                return;
            }

            foreach (var action in _delayedActions)
            {
                action();
            }
            _delayedActions.Clear();
        }

        public virtual void DelayBind(Action action)
        {
            _delayedActions.Add(action);
        }

        public virtual void RegisterBinding(object target, IMvxUpdateableBinding binding)
        {
            _directBindings.Add(new TargetAndBinding(target, binding));
        }

        public virtual void RegisterBindingsWithClearKey(object clearKey, IEnumerable<KeyValuePair<object, IMvxUpdateableBinding>> bindings)
        {
            _viewBindings.Add(new KeyValuePair<object, IList<TargetAndBinding>>(clearKey, bindings.Select(b => new TargetAndBinding(b.Key, b.Value)).ToList()));
        }

        public virtual void RegisterBindingWithClearKey(object clearKey, object target, IMvxUpdateableBinding binding)
        {
            var list = new List<TargetAndBinding>() { new TargetAndBinding(target, binding) };
            _viewBindings.Add(new KeyValuePair<object, IList<TargetAndBinding>>(clearKey, list));
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