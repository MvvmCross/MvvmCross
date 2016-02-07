// MvxBindingContext.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding.Bindings.Source.Construction;

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

        private IMvxEnhancedDataContext _dataContext;

        public MvxBindingContext()
            : this((object)null)
        {
        }

        public MvxBindingContext(object dataContext, object parentDataContext = null)
            : this(dataContext, parentDataContext, (IDictionary<object, string>)null)
        {
        }

        public MvxBindingContext(IDictionary<object, string> firstBindings)
            : this(null, null, firstBindings)
        {
        }

        public MvxBindingContext(object dataContext, IDictionary<object, string> firstBindings)
            : this(dataContext, null, firstBindings)
        {
        }

        public MvxBindingContext(object dataContext, object parentDataContext, IDictionary<object, string> firstBindings)
        {
            if (firstBindings != null)
            {
                foreach (var kvp in firstBindings)
                {
                    this.AddDelayedAction(kvp);
                }
            }
            if (dataContext != null || parentDataContext != null)
            {
                this.EnhancedDataContext = MvxSimpleEnhancedDataContext.FromCoreAndParent(dataContext, parentDataContext);
            }
        }

        public MvxBindingContext(IDictionary<object, IEnumerable<MvxBindingDescription>> firstBindings)
            : this(null, null, firstBindings)
        {
        }

        public MvxBindingContext(object dataContext,
            IDictionary<object, IEnumerable<MvxBindingDescription>> firstBindings)
            : this(null, null, firstBindings)
        {
        }

        public MvxBindingContext(object dataContext,
                                 object parentDataContext,
                                 IDictionary<object, IEnumerable<MvxBindingDescription>> firstBindings)
        {
            foreach (var kvp in firstBindings)
            {
                this.AddDelayedAction(kvp);
            }
            if (dataContext != null || parentDataContext != null)
            {
                this.EnhancedDataContext = MvxSimpleEnhancedDataContext.FromCoreAndParent(dataContext, parentDataContext);
            }
        }

        private void AddDelayedAction(KeyValuePair<object, string> kvp)
        {
            this._delayedActions.Add(() =>
            {
#warning TODO - passing the EnhancedDataContext is v v v naughty...
                var bindings = this.Binder.Bind(this.EnhancedDataContext, kvp.Key, kvp.Value);
                foreach (var b in bindings)
                    this.RegisterBinding(kvp.Key, b);
            });
        }

        private void AddDelayedAction(KeyValuePair<object, IEnumerable<MvxBindingDescription>> kvp)
        {
            this._delayedActions.Add(() =>
            {
#warning TODO - passing the EnhancedDataContext is v v v naughty...
                var bindings = this.Binder.Bind(this.EnhancedDataContext, kvp.Key, kvp.Value);
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
            get { return this.EnhancedDataContext?.Core; }
            set
            {
                this.EnhancedDataContext = MvxSimpleEnhancedDataContext.FromCoreAndParent(value,
                    EnhancedDataContext?.Parent);
            }
        }

        public IMvxEnhancedDataContext EnhancedDataContext
        {
            get { return this._dataContext; }
            set
            {
                this._dataContext = MvxSimpleEnhancedDataContext.Clone(value);
                this.OnDataContextChange();
                var handler = this.DataContextChanged;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler DataContextChanged;

        private void SetDataContextOnBinding(IMvxUpdateableBinding binding)
        {
            // TODO - would be nice to drop this "is" check one day
            if (binding is IMvxEnhancedDataContextAwareBinding)
                binding.DataContext = this._dataContext;
            else
                binding.DataContext = this._dataContext?.Core;
        }

        protected virtual void OnDataContextChange()
        {
            // update existing bindings
            foreach (var binding in this._viewBindings)
            {
                foreach (var bind in binding.Value)
                {
                    SetDataContextOnBinding(bind.Binding);
                }
            }

            foreach (var binding in this._directBindings)
            {
                SetDataContextOnBinding(binding.Binding);
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