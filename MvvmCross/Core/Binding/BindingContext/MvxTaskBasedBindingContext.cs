using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.Bindings;
using MvvmCross.Platform;

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
            this.AddDelayedAction(firstBindingKey, firstBindingValue);
            if (dataContext != null)
                this.DataContext = dataContext;

            return this;
        }

        public IMvxBindingContext Init(object dataContext, object firstBindingKey, string firstBindingValue)
        {
            this.AddDelayedAction(firstBindingKey, firstBindingValue);
            if (dataContext != null)
                this.DataContext = dataContext;

            return this;
        }

        private void AddDelayedAction(object key, string value)
        {
            this._delayedActions.Add(() =>
            {
                var bindings = this.Binder.Bind(this.DataContext, key, value);
                foreach (var b in bindings)
                    this.RegisterBinding(key, b);
            });
        }

        private void AddDelayedAction(object key, IEnumerable<MvxBindingDescription> value)
        {
            this._delayedActions.Add(() =>
            {
                var bindings = this.Binder.Bind(this.DataContext, key, value);
                foreach (var b in bindings)
                    this.RegisterBinding(key, b);
            });
        }

        ~MvxTaskBasedBindingContext()
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
            if (this._delayedActions.Count != 0)
            {
                foreach (var action in this._delayedActions)
                {
                    action();
                }
                this._delayedActions.Clear();
            }

            // Copy the lists to ensure that if the main thread modifies the collection
            // once we are on the background thread we don't get an InvalidOperationException. 
            // Issue: #1398
            // View bindings need to be deep copied
            var viewBindingsCopy = this._viewBindings.Select(vb => new KeyValuePair<object, IList<MvxBindingContext.TargetAndBinding>>(vb.Key, vb.Value.ToList()))
                                                     .ToList();

            var directBindingsCopy = this._directBindings.ToList();

            Action setBindingsAction = (() =>
                {
                    foreach (var binding in viewBindingsCopy)
                    {
                        foreach (var bind in binding.Value)
                        {
                            bind.Binding.DataContext = this._dataContext;
                        }
                    }

                    foreach (var binding in directBindingsCopy)
                    {
                        binding.Binding.DataContext = this._dataContext;
                    }
                });

            if (RunSynchronously)
                setBindingsAction();
            else 
                Task.Run(setBindingsAction);
        }

        public virtual void DelayBind(Action action)
        {
            this._delayedActions.Add(action);
        }

        public virtual void RegisterBinding(object target, IMvxUpdateableBinding binding)
        {
            this._directBindings.Add(new MvxBindingContext.TargetAndBinding(target, binding));
        }

        public virtual void RegisterBindingsWithClearKey(object clearKey, IEnumerable<KeyValuePair<object, IMvxUpdateableBinding>> bindings)
        {
            this._viewBindings.Add(new KeyValuePair<object, IList<MvxBindingContext.TargetAndBinding>>(clearKey, bindings.Select(b => new MvxBindingContext.TargetAndBinding(b.Key, b.Value)).ToList()));
        }

        public virtual void RegisterBindingWithClearKey(object clearKey, object target, IMvxUpdateableBinding binding)
        {
            var list = new List<MvxBindingContext.TargetAndBinding> { new MvxBindingContext.TargetAndBinding(target, binding) };
            this._viewBindings.Add(new KeyValuePair<object, IList<MvxBindingContext.TargetAndBinding>>(clearKey, list));
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