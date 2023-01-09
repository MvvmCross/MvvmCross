// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views.Fragments.EventSource;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace MvvmCross.Platforms.Android.Views.Fragments
{
    public class MvxBindingFragmentAdapter
        : MvxBaseFragmentAdapter
    {
        public IMvxFragmentView? FragmentView => Fragment as IMvxFragmentView;

        public MvxBindingFragmentAdapter(IMvxEventSourceFragment eventSource)
            : base(eventSource)
        {
            if (eventSource is not IMvxFragmentView)
                throw new ArgumentException("eventSource must be an IMvxFragmentView", nameof(eventSource));
        }

        protected override void HandleCreateCalled(object? sender, MvxValueEventArgs<Bundle>? e)
        {
            // Create is called after Fragment is attached to Activity
            // it's safe to assume that Fragment has activity

            if (Fragment?.Activity is not IMvxAndroidView hostMvxView)
            {
                MvxLogHost.GetLog<MvxBindingFragmentAdapter>()?.Log(LogLevel.Warning,
                    "Fragment host for fragment type {FragmentType} is not of type IMvxAndroidView", Fragment?.GetType());
                return;
            }

            // if restoring state, Activity.ViewModel might be null, so a harder mechanism is necessary
            var viewModelType = hostMvxView.ViewModel != null
                ? hostMvxView.ViewModel.GetType()
                : hostMvxView.FindAssociatedViewModelTypeOrNull();

            if (viewModelType == null)
            {
                MvxLogHost.GetLog<MvxBindingFragmentAdapter>()?.Log(LogLevel.Warning,
                    "ViewModel type for Activity {FragmentActivityType} not found when trying to show fragment: {FragmentType}",
                    Fragment.Activity.GetType(), Fragment.GetType());
                return;
            }

            (Bundle? bundle, MvxViewModelRequest? request) = GetAndroidBundleAndRequest(e);

            var mvxBundle = ReadAndroidBundle(bundle);
            if (mvxBundle != null)
                FragmentView?.OnCreate(mvxBundle, request);
        }

        private (Bundle? bundle, MvxViewModelRequest? request) GetAndroidBundleAndRequest(MvxValueEventArgs<Bundle>? bundleArgs)
        {
            Bundle? bundle = null;
            MvxViewModelRequest? request = null;
            if (bundleArgs?.Value != null)
            {
                // saved state
                bundle = bundleArgs.Value;
            }
            else if (FragmentView is Fragment { Arguments: { } } fragment)
            {
                bundle = fragment.Arguments;
                var json = bundle.GetString("__mvxViewModelRequest");
                if (string.IsNullOrEmpty(json))
                    return (bundle, request);

                request = ReadRequest(request, json);
            }

            return (bundle, request);
        }

        private static MvxViewModelRequest? ReadRequest(MvxViewModelRequest? request, string json)
        {
            if (Mvx.IoCProvider?.TryResolve(out IMvxNavigationSerializer serializer) == true)
            {
                request = serializer.Serializer.DeserializeObject<MvxViewModelRequest>(json);
            }
            else
            {
                MvxLogHost.GetLog<MvxBindingFragmentAdapter>()?.Log(LogLevel.Warning,
                    "Navigation Serializer not available, deserializing ViewModel Request will be hard");
            }

            return request;
        }

        private static IMvxBundle? ReadAndroidBundle(Bundle? bundle)
        {
            if (Mvx.IoCProvider?.TryResolve(out IMvxSavedStateConverter converter) == true && bundle != null)
            {
                return converter.Read(bundle);
            }

            MvxLogHost.GetLog<MvxBindingFragmentAdapter>()?.Log(LogLevel.Warning,
            "Saved state converter not available - saving state will be hard");

            return null;
        }

        protected override void HandleCreateViewCalled(
            object? sender, MvxValueEventArgs<MvxCreateViewParameters> e) =>
            FragmentView?.EnsureBindingContextIsSet(e.Value.Inflater);

        protected override void HandleSaveInstanceStateCalled(object? sender, MvxValueEventArgs<Bundle> e)
        {
            // it is guaranteed that SaveInstanceState call will be executed before OnStop (thus before Fragment detach)
            // it is safe to assume that Fragment has activity attached

            var mvxBundle = FragmentView?.CreateSaveStateBundle();
            if (mvxBundle != null)
            {
                if (!Mvx.IoCProvider.TryResolve(out IMvxSavedStateConverter converter))
                {
                    MvxLogHost.GetLog<MvxBindingFragmentAdapter>()?.Log(LogLevel.Warning,
                        "Saved state converter not available - saving state will be hard");
                }
                else
                {
                    converter.Write(e.Value, mvxBundle);
                }
            }

            if (FragmentView == null)
                return;

            var cache = Mvx.IoCProvider.Resolve<IMvxMultipleViewModelCache>();
            cache.Cache(FragmentView.ViewModel, FragmentView.UniqueImmutableCacheTag);
        }

        protected override void HandleDestroyViewCalled(object? sender, EventArgs e)
        {
            FragmentView?.BindingContext?.ClearAllBindings();
            base.HandleDestroyViewCalled(sender, e);
        }

        protected override void HandleDisposeCalled(object? sender, EventArgs e)
        {
            FragmentView?.BindingContext?.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}
