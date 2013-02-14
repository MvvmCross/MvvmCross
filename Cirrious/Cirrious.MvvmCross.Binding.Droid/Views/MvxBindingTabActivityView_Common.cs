// MvxBindingTabActivityView_Common.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

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
            BindingManager.BindView (BindingManager.RootView, DefaultBindingSource);
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
        
        protected override void OnViewModelSet ()
        {
            base.OnViewModelSet ();
            BindingManager.BindView (BindingManager.RootView, DefaultBindingSource);
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
        
        public virtual object DefaultBindingSource {
            get { return ViewModel; }
        }
    }
}