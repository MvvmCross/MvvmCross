// MvxBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public class MvxBindingContext
        : IMvxBindingContext
    {
        public class TargetAndBinding
        {
            public TargetAndBinding(object target, IMvxUpdateableBinding binding)
            {
                Target = target;
                Binding = binding;
            }

            public object Target { get; private set; }
            public IMvxUpdateableBinding Binding { get; private set; }
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
            : this(null, firstBindings)
        {
        }

        public MvxBindingContext(object dataContext, IDictionary<object, string> firstBindings)
        {
            foreach (var kvp in firstBindings)
            {
                AddDelayedAction(kvp);
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
                AddDelayedAction(kvp);
            }
            if (dataContext != null)
                DataContext = dataContext;
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
                var handler = DataContextChanged;
                handler?.Invoke(this, EventArgs.Empty);
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

        public virtual void RegisterBindingsWithClearKey(object clearKey, IList<KeyValuePair<object, IMvxUpdateableBinding>> bindings)
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