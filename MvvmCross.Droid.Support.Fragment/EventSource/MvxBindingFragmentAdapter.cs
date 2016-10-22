// MvxBindingFragmentAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using Android.Support.V4.App;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Platform;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using System;
using MvvmCross.Droid.Shared.Fragments.EventSource;
using MvvmCross.Droid.Shared.Fragments;
using MvvmCross.Droid.Shared;
using MvvmCross.Droid.Shared.Attributes;

namespace MvvmCross.Droid.Support.V4.EventSource
{
    public class MvxBindingFragmentAdapter
        : MvxBaseFragmentAdapter
    {
        public IMvxFragmentView FragmentView => Fragment as IMvxFragmentView;

        public MvxBindingFragmentAdapter(IMvxEventSourceFragment eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxFragmentView))
                throw new ArgumentException("eventSource must be an IMvxFragmentView");
        }

        protected override void HandleCreateCalled(object sender, MvxValueEventArgs<Bundle> bundleArgs)
        {
            FragmentView.EnsureSetupInitialized();

			// Create is called after Fragment is attached to Activity
			// it's safe to assume that Fragment has activity
			if (!FragmentView.GetType().IsFragmentCacheable(Fragment.Activity.GetType()))
				return;

			FragmentView.RegisterFragmentViewToCacheIfNeeded(Fragment.Activity.GetType());

            Bundle bundle = null;
            MvxViewModelRequest request = null;
            if (bundleArgs?.Value != null)
            {
                // saved state
                bundle = bundleArgs.Value;
            }
            else
            {
                var fragment = FragmentView as Android.Support.V4.App.Fragment;
                if (fragment?.Arguments != null)
                {
                    bundle = fragment.Arguments;
                    var json = bundle.GetString("__mvxViewModelRequest");
                    if (!string.IsNullOrEmpty(json))
                    {
                        IMvxNavigationSerializer serializer;
                        if (!Mvx.TryResolve(out serializer))
                        {
                            MvxTrace.Warning(
                                "Navigation Serializer not available, deserializing ViewModel Request will be hard");
                        }
                        else
                        {
                            request = serializer.Serializer.DeserializeObject<MvxViewModelRequest>(json);
                        }
                    }
                }
            }

            IMvxSavedStateConverter converter;
            if (!Mvx.TryResolve(out converter))
            {
                MvxTrace.Warning("Saved state converter not available - saving state will be hard");
            }
            else
            {
                if (bundle != null)
                {
                    var mvxBundle = converter.Read(bundle);
                    FragmentView.OnCreate(mvxBundle, request);
                }
            }
        }

        protected override void HandleCreateViewCalled(object sender,
                                               MvxValueEventArgs<MvxCreateViewParameters> args)
        {
            FragmentView.EnsureBindingContextIsSet(args.Value.Inflater);
        }

        protected override void HandleSaveInstanceStateCalled(object sender, MvxValueEventArgs<Bundle> bundleArgs)
        {
			// it is guarannted that SaveInstanceState call will be executed before OnStop (thus before Fragment detach)
			// it is safe to assume that Fragment has activity attached
			if (!FragmentView.GetType().IsFragmentCacheable(Fragment.Activity.GetType()))
                return;

            var mvxBundle = FragmentView.CreateSaveStateBundle();
            if (mvxBundle != null)
            {
                IMvxSavedStateConverter converter;
                if (!Mvx.TryResolve(out converter))
                {
                    MvxTrace.Warning("Saved state converter not available - saving state will be hard");
                }
                else
                {
                    converter.Write(bundleArgs.Value, mvxBundle);
                }
            }
            var cache = Mvx.Resolve<IMvxMultipleViewModelCache>();
            cache.Cache(FragmentView.ViewModel, FragmentView.UniqueImmutableCacheTag);
        }

        protected override void HandleDestroyViewCalled(object sender, EventArgs e)
        {
            FragmentView.BindingContext?.ClearAllBindings();
            base.HandleDestroyViewCalled(sender, e);
        }

        protected override void HandleDisposeCalled(object sender, EventArgs e)
        {
            FragmentView.BindingContext?.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}