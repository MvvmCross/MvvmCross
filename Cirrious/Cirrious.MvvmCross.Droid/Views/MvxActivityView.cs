// MvxActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class TypedEventArgs<T> : EventArgs
    {
        public TypedEventArgs(T value)
        {
            Value = value;
        }
        
        public T Value { get; private set; }
    }

    public static class MvxDelegateExtensionMethods
    {
        public static void Raise(this EventHandler eventHandler, object sender)
        {
            if (eventHandler == null)
                return;

            eventHandler(sender, EventArgs.Empty);
        }

        public static void Raise<T>(this EventHandler<TypedEventArgs<T>> eventHandler, object sender, T value)
        {
            if (eventHandler == null)
                return;

            eventHandler(sender, new TypedEventArgs<T>(value));
        }
    }	

    public interface IDisposeSource
    {
        event EventHandler DisposeCalled;
    }

    public class StartActivityForResultParameters
    {
        public StartActivityForResultParameters(Intent intent, int requestCode)
        {
            RequestCode = requestCode;
            Intent = intent;
        }

        public Intent Intent { get; private set; }
        public int RequestCode { get; private set; }    
    }

    public class ActivityResultParameters
    {
        public ActivityResultParameters(int requestCode, Result resultCode, Intent data)
        {
            Data = data;
            ResultCode = resultCode;
            RequestCode = requestCode;
        }

        public int RequestCode { get; private set; }
        public Result ResultCode { get; private set; }
        public Intent Data { get; private set; }
    }

    public interface IActivityEventSource : IDisposeSource
    {
        event EventHandler<TypedEventArgs<Bundle>> CreateWillBeCalled;
        event EventHandler<TypedEventArgs<Bundle>> CreateCalled;
        event EventHandler DestroyCalled;
        event EventHandler<TypedEventArgs<Intent>> NewIntentCalled;
        event EventHandler ResumeCalled;
        event EventHandler PauseCalled;
        event EventHandler StartCalled;
        event EventHandler RestartCalled;
        event EventHandler StopCalled;
        event EventHandler<TypedEventArgs<StartActivityForResultParameters>> StartActivityForResultCalled;
        event EventHandler<TypedEventArgs<ActivityResultParameters>> ActivityResultCalled;
    }

    public abstract class EventSourceActivity
        : Activity
        , IActivityEventSource
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            CreateCalled.Raise(this, bundle);   
        }

        protected override void OnDestroy()
        {
            DestroyCalled.Raise(this);
            base.OnDestroy();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            NewIntentCalled.Raise(this, intent);
        }

        protected override void OnResume()
        {
            base.OnResume();
            ResumeCalled.Raise(this);
        }

        protected override void OnPause()
        {
            PauseCalled.Raise(this);
            base.OnPause();
        }

        protected override void OnStart()
        {
            base.OnStart();
            StartCalled.Raise(this);
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            RestartCalled.Raise(this);
        }

        protected override void OnStop()
        {
            StopCalled.Raise(this);
            base.OnStop();
        }

        public override void StartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResultCalled.Raise(this, new StartActivityForResultParameters(intent, requestCode));
            base.StartActivityForResult(intent, requestCode);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            ActivityResultCalled.Raise(this, new ActivityResultParameters(requestCode, resultCode, data));
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public event EventHandler DisposeCalled;
        public event EventHandler<TypedEventArgs<Bundle>> CreateWillBeCalled;
        public event EventHandler<TypedEventArgs<Bundle>> CreateCalled;
        public event EventHandler DestroyCalled;
        public event EventHandler<TypedEventArgs<Intent>> NewIntentCalled;
        public event EventHandler ResumeCalled;
        public event EventHandler PauseCalled;
        public event EventHandler StartCalled;
        public event EventHandler RestartCalled;
        public event EventHandler StopCalled;
        public event EventHandler<TypedEventArgs<StartActivityForResultParameters>> StartActivityForResultCalled;
        public event EventHandler<TypedEventArgs<ActivityResultParameters>> ActivityResultCalled;
    }

    public class BaseActivityAdapter
    {
        private readonly IActivityEventSource _eventSource;

        protected Activity Activity
        {
            get { return _eventSource as Activity; }
        }

        public BaseActivityAdapter(IActivityEventSource eventSource)
        {
            _eventSource = eventSource;

            _eventSource.CreateCalled += EventSourceOnCreateCalled;
            _eventSource.CreateWillBeCalled += EventSourceOnCreateWillBeCalled;
            _eventSource.StartCalled += EventSourceOnStartCalled;
            _eventSource.RestartCalled += EventSourceOnRestartCalled;
            _eventSource.ResumeCalled += EventSourceOnResumeCalled;
            _eventSource.PauseCalled += EventSourceOnPauseCalled;
            _eventSource.StopCalled += EventSourceOnStopCalled;
            _eventSource.DestroyCalled += EventSourceOnDestroyCalled;
            _eventSource.DisposeCalled += EventSourceOnDisposeCalled;

            _eventSource.NewIntentCalled += EventSourceOnNewIntentCalled;
            
            _eventSource.ActivityResultCalled += EventSourceOnActivityResultCalled;
            _eventSource.StartActivityForResultCalled += EventSourceOnStartActivityForResultCalled;
        }

        protected virtual void EventSourceOnCreateWillBeCalled(object sender, TypedEventArgs<Bundle> typedEventArgs)
        {
        }

        protected virtual void EventSourceOnStopCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnStartCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnStartActivityForResultCalled(object sender, TypedEventArgs<StartActivityForResultParameters> typedEventArgs)
        {
        }

        protected virtual void EventSourceOnResumeCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnRestartCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnPauseCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnNewIntentCalled(object sender, TypedEventArgs<Intent> typedEventArgs)
        {
        }

        protected virtual void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnCreateCalled(object sender, TypedEventArgs<Bundle> typedEventArgs)
        {
        }

        protected virtual void EventSourceOnActivityResultCalled(object sender, TypedEventArgs<ActivityResultParameters> typedEventArgs)
        {
        }
    }

    public class MvxActivityAdapter : BaseActivityAdapter
    {
        protected IMvxAndroidView AndroidView
        {
            get { return Activity as IMvxAndroidView; }
        }

        public MvxActivityAdapter(IActivityEventSource eventSource)
            : base(eventSource)
        {
        }

        protected override void EventSourceOnStopCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewStop();
        }

        protected override void EventSourceOnStartCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewStart();
        }

        protected override void EventSourceOnStartActivityForResultCalled(object sender, TypedEventArgs<StartActivityForResultParameters> typedEventArgs)
        {
            var requestCode = typedEventArgs.Value.RequestCode;
            switch (requestCode)
            {
                case (int)MvxIntentRequestCode.PickFromFile:
                    MvxTrace.Trace("Warning - activity request code may clash with Mvx code for {0}",
                                   (MvxIntentRequestCode)requestCode);
                    break;
            }
        }

        protected override void EventSourceOnResumeCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.IsVisible = true;
            AndroidView.OnViewResume();
        }

        protected override void EventSourceOnRestartCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewRestart();
        }

        protected override void EventSourceOnPauseCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewPause();
            AndroidView.IsVisible = false;
        }

        protected override void EventSourceOnNewIntentCalled(object sender, TypedEventArgs<Intent> typedEventArgs)
        {
            AndroidView.OnViewNewIntent();
        }

        protected override void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
            AndroidView.OnViewDestroy();
        }

        protected override void EventSourceOnCreateCalled(object sender, TypedEventArgs<Bundle> typedEventArgs)
        {
            AndroidView.IsVisible = true;
            AndroidView.OnViewCreate();
        }

        protected override void EventSourceOnActivityResultCalled(object sender, TypedEventArgs<ActivityResultParameters> typedEventArgs)
        {
            var sink = MvxServiceProviderExtensions.GetService<IMvxIntentResultSink>();
            var args = typedEventArgs.Value;
            var intentResult = new MvxIntentResultEventArgs(args.RequestCode, args.ResultCode, args.Data);
            sink.OnResult(intentResult);
        }
    }

    public static class MvxActivityViewExtensions
    {
        public static void AddEventListeners(this IActivityEventSource activity)
        {
            if (activity is IMvxAndroidView)
            {
                var adapter = new MvxActivityAdapter(activity);
            }
            if (activity is IMvxBindingActivity)
            {
                var bindingAdapter = new MvxBindingActivityAdapter(activity);
            }
            if (activity is IMvxChildViewModelOwner)
            {
                var childOwnerAdapter = new MvxChildViewModelOwnerAdapter(activity);
            }
        }
    }

    public abstract class MvxActivityView
        : EventSourceActivity
        , IMvxAndroidView
    {
        protected MvxActivityView()
        {
            BindingOwnerHelper = new MvxBindingOwnerHelper(this, this, this);
            this.AddEventListeners();
        }

        public bool IsVisible { get; set; }

        public object DataContext { get; set; }

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            base.StartActivityForResult(intent, requestCode);
        }

        public IMvxBindingOwnerHelper BindingOwnerHelper { get; private set; }

        public override void SetContentView(int layoutResId)
        {
            var view = this.BindingInflate(layoutResId, null);
            SetContentView(view);
        }

        protected abstract void OnViewModelSet();
    }
}