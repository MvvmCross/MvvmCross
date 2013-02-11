// MvxBindingTabActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Droid.Binders;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public abstract partial class MvxBindingTabActivityView<TViewModel>
        : IMvxBindingActivity, IMvxServiceConsumer
    {
        // Code shared across all binding activities, copy paste as you will.
        
        private readonly IMvxViewBindingManager _bindings;
        
        public MvxBindingTabActivityView () : base()
        {
            _bindings = new MvxActivityBindingManager (this);
        }
        
        protected override void OnCreate (Bundle bundle)
        {
            BindingManager.UnbindAll ();
            base.OnCreate (bundle);
        }
        
        protected override void OnStart ()
        {
            base.OnStart ();
            BindingManager.RebindViews ();
        }
        
        protected override void OnDestroy ()
        {
            BindingManager.UnbindAll ();
            base.OnDestroy ();
        }
        
        protected override void Dispose (bool disposing)
        {
            if (disposing)
                BindingManager.UnbindAll ();
            base.Dispose (disposing);
        }
        
        public virtual IMvxViewBindingManager BindingManager {
            get {
                return _bindings;
            }
        }
        
        #region Obsolete interfaces that need to be removed at some point
        
        [Obsolete]
        public void RegisterBindingsFor (View view)
        {
            BindingManager.BindView (view);
        }
        
        [Obsolete]
        public void RegisterBinding (IMvxBinding binding)
        {
            BindingManager.AddBinding (binding);
        }
        
        [Obsolete]
        public void ClearBindings (View view)
        {
            BindingManager.UnbindView (view);
        }
        
        [Obsolete]
        public View BindingInflate (object source, int resourceId, ViewGroup viewGroup)
        {
            var view = LayoutInflater.Inflate (resourceId, viewGroup);
            BindingManager.BindView (view, source);
            return view;
        }
        
        [Obsolete]
        public View BindingInflate (int resourceId, ViewGroup viewGroup)
        {
            var view = LayoutInflater.Inflate (resourceId, viewGroup);
            BindingManager.BindView (view, DefaultBindingSource);
            return view;
        }
        
        [Obsolete]
        public View NonBindingInflate (int resourceId, ViewGroup viewGroup)
        {
            return LayoutInflater.Inflate (resourceId, viewGroup);
        }
        
        #endregion
        
        protected override void OnViewModelSet ()
        {
            // TODO: Make sure all of the fragments have updated their view models before we rebind.
            BindingManager.RebindViews ();
        }
        
        public override View OnCreateView (string name, Context context, IAttributeSet attrs)
        {
            View view = base.OnCreateView (name, context, attrs);
            if (view != null) {
                this.GetService<IMvxBindingInflater> ().Attach (view, attrs);
            } else {
                view = this.GetService<IMvxBindingInflater> ().Inflate (name, context, attrs);
            }
            return view;
        }
        
        // TODO: Figure out how to handle this...
        public virtual object DefaultBindingSource {
            get { return ViewModel; }
        }
    }
}