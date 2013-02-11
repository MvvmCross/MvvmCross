using System;
using Android.App;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    /// <summary>
    /// Activity binding manager handles bindings for an activity. This is only useful for you if you're implementing
    /// binding support for custom activity base class.
    /// </summary>
    /// 
    /// <example>
    /// You should only have one instance of it per activity, ie:
    /// 
    /// <code>
    /// public class MvxMyAndroidBindingActivity : extends MyAndroidActivity {
    ///     // ...
    /// 
    ///     private readonly IMvxViewBindingManager _bindings;
    ///     
    ///     public MvxMyAndroidBindingActivity() : base() {
    ///         _bindings = new MvxActivityBindingManager(this);
    ///     }
    /// 
    ///     public IMvxViewBindingManager BindingManager {
    ///         get { return _bindings; }
    ///     }
    /// 
    ///     // ...
    /// }
    /// </code>
    /// </example>
    public class MvxActivityBindingManager : IMvxViewBindingManager
    {
        private readonly WeakReference _activity;

        public MvxActivityBindingManager (Activity activity)
        {
            // We're using weak references just in case
            _activity = new WeakReference (activity);
        }

        protected View RootView {
            get {
                var activity = _activity.Target as Activity;
                if (activity == null)
                    return null;
                return activity.Window.FindViewById (Android.Resource.Id.Content);
            }
        }

        #region IMvxViewBindingManager implementation

        public void BindView (View view, object dataSource = null)
        {
            throw new NotImplementedException ();
        }

        public void UnbindView (View view)
        {
            throw new NotImplementedException ();
        }

        public void RebindViews ()
        {
            throw new NotImplementedException ();
        }

        public void AddBinding (Cirrious.MvvmCross.Binding.Interfaces.IMvxBinding binding)
        {
            throw new NotImplementedException ();
        }

        public void RemoveBinding (Cirrious.MvvmCross.Binding.Interfaces.IMvxBinding binding)
        {
            throw new NotImplementedException ();
        }

        public void UnbindAll ()
        {
            throw new NotImplementedException ();
        }

        #endregion
    }
}

