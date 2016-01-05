// MvxBindingContext.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.BindingContext
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.Bindings;
    using MvvmCross.Platform;

    public class MvxBindingContext
        : IMvxBindingContext
    {
        public class TargetAndBinding
        {
            public TargetAndBinding(object target, IMvxUpdateableBinding binding)
            {
                this.Target = target;
                this.Binding = binding;
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
            this._dataContext = dataContext;
        }

        public MvxBindingContext(IDictionary<object, string> firstBindings)
            : this(null, firstBindings)
        {
        }

        public MvxBindingContext(object dataContext, IDictionary<object, string> firstBindings)
        {
            foreach (var kvp in firstBindings)
            {
                this.AddDelayedAction(kvp);
            }
            if (dataContext != null)
                this.DataContext = dataContext;
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
                this.AddDelayedAction(kvp);
            }
            if (dataContext != null)
                this.DataContext = dataContext;
        }

        private void AddDelayedAction(KeyValuePair<object, string> kvp)
        {
            this._delayedActions.Add(() =>
            {
                var bindings = this.Binder.Bind(this.DataContext, kvp.Key, kvp.Value);
                foreach (var b in bindings)
                    this.RegisterBinding(kvp.Key, b);
            });
        }

        private void AddDelayedAction(KeyValuePair<object, IEnumerable<MvxBindingDescription>> kvp)
        {
            this._delayedActions.Add(() =>
            {
                var bindings = this.Binder.Bind(this.DataContext, kvp.Key, kvp.Value);
                foreach (var b in bindings)
                    this.RegisterBinding(kvp.Key, b);
            });
        }

        ~MvxBindingContext()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ClearAllBindings();
            }
        }

        private IMvxBinder _binder;

        protected IMvxBinder Binder
        {
            get
            {
                this._binder = this._binder ?? Mvx.Resolve<IMvxBinder>();
                return this._binder;
            }
        }

        public object DataContext
        {
            get { return this._dataContext; }
            set
            {
                this._dataContext = value;
                this.OnDataContextChange();
                var handler = this.DataContextChanged;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler DataContextChanged;

        protected virtual void OnDataContextChange()
        {
            // update existing bindings
            foreach (var binding in this._viewBindings)
            {
                foreach (var bind in binding.Value)
                {
                    bind.Binding.DataContext = this._dataContext;
                }
            }

            foreach (var binding in this._directBindings)
            {
                binding.Binding.DataContext = this._dataContext;
            }

            // add new bindings
            if (this._delayedActions.Count == 0)
            {
                return;
            }

            foreach (var action in this._delayedActions)
            {
                action();
            }
            this._delayedActions.Clear();
        }

        public virtual void DelayBind(Action action)
        {
            this._delayedActions.Add(action);
        }

        public virtual void RegisterBinding(object target, IMvxUpdateableBinding binding)
        {
            this._directBindings.Add(new TargetAndBinding(target, binding));
        }

        public virtual void RegisterBindingsWithClearKey(object clearKey, IEnumerable<KeyValuePair<object, IMvxUpdateableBinding>> bindings)
        {
            this._viewBindings.Add(new KeyValuePair<object, IList<TargetAndBinding>>(clearKey, bindings.Select(b => new TargetAndBinding(b.Key, b.Value)).ToList()));
        }

        public virtual void RegisterBindingWithClearKey(object clearKey, object target, IMvxUpdateableBinding binding)
        {
            var list = new List<TargetAndBinding>() { new TargetAndBinding(target, binding) };
            this._viewBindings.Add(new KeyValuePair<object, IList<TargetAndBinding>>(clearKey, list));
        }

        public virtual void ClearBindings(object clearKey)
        {
            if (clearKey == null)
                return;

            for (var i = this._viewBindings.Count - 1; i >= 0; i--)
            {
                var candidate = this._viewBindings[i];
                if (candidate.Key.Equals(clearKey))
                {
                    foreach (var binding in candidate.Value)
                    {
                        binding.Binding.Dispose();
                    }
                    this._viewBindings.RemoveAt(i);
                }
            }
        }

        public virtual void ClearAllBindings()
        {
            this.ClearAllViewBindings();
            this.ClearAllDirectBindings();
            this.ClearAllDelayedBindings();
        }

        protected virtual void ClearAllDelayedBindings()
        {
            this._delayedActions.Clear();
        }

        protected virtual void ClearAllDirectBindings()
        {
            foreach (var binding in this._directBindings)
            {
                binding.Binding.Dispose();
            }
            this._directBindings.Clear();
        }

        protected virtual void ClearAllViewBindings()
        {
            foreach (var kvp in this._viewBindings)
            {
                foreach (var binding in kvp.Value)
                {
                    binding.Binding.Dispose();
                }
            }
            this._viewBindings.Clear();
        }
    }
}