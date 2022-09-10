// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using AndroidX.ViewPager.Widget;
using Google.Android.Material.Tabs;
using Java.Lang;
using Microsoft.Extensions.Logging;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Views.ViewPager;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;
using DialogFragment = AndroidX.Fragment.App.DialogFragment;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;

namespace MvvmCross.Platforms.Android.Presenters
{
#nullable enable
    public class MvxAndroidViewPresenter : MvxAttributeViewPresenter, IMvxAndroidViewPresenter
    {
        public const string ViewModelRequestBundleKey = "__mvxViewModelRequest";
        public const string SharedElementsBundleKey = "__sharedElementsKey";

        private readonly Lazy<IMvxAndroidCurrentTopActivity> _androidCurrentTopActivity =
            new Lazy<IMvxAndroidCurrentTopActivity>(() => Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>());

        private readonly Lazy<IMvxAndroidActivityLifetimeListener> _activityLifetimeListener =
            new Lazy<IMvxAndroidActivityLifetimeListener>(() => Mvx.IoCProvider.Resolve<IMvxAndroidActivityLifetimeListener>());

        private readonly Lazy<IMvxNavigationSerializer> _navigationSerializer =
            new Lazy<IMvxNavigationSerializer>(() => Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>());

        private readonly Lazy<ILogger?> _logger =
            new Lazy<ILogger?>(() => MvxLogHost.GetLog<MvxAndroidViewPresenter>());

        protected IEnumerable<Assembly> AndroidViewAssemblies { get; set; }

        protected MvxViewModelRequest? PendingRequest { get; set; }

        protected virtual FragmentManager? CurrentFragmentManager
        {
            get
            {
                if (CurrentActivity.IsActivityDead())
                    return null;

                return CurrentActivity!.SupportFragmentManager;
            }
        }

        protected virtual Activity? CurrentActivity =>
            _androidCurrentTopActivity.Value?.Activity as Activity;

        protected IMvxAndroidActivityLifetimeListener ActivityLifetimeListener =>
            _activityLifetimeListener.Value;

        protected IMvxNavigationSerializer NavigationSerializer =>
            _navigationSerializer.Value;

        public MvxAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies)
        {
            AndroidViewAssemblies = androidViewAssemblies;
            ActivityLifetimeListener.ActivityChanged += ActivityLifetimeListenerOnActivityChanged;
        }

        protected virtual void ActivityLifetimeListenerOnActivityChanged(object sender, MvxActivityEventArgs e)
        {
            if (e.ActivityState == MvxActivityState.OnResume && PendingRequest != null)
            {
                Show(PendingRequest);
                PendingRequest = null;
            }
            else if (e.ActivityState == MvxActivityState.OnCreate && e.Extras is Bundle savedBundle)
            {
                //TODO: Restore fragments from bundle
            }
            else if (e.ActivityState == MvxActivityState.OnSaveInstanceState && e.Extras is Bundle outBundle)
            {
                //TODO: Save fragments into bundle
            }
            else if (e.ActivityState == MvxActivityState.OnDestroy)
            {
                //TODO: Should be check for Fragments on this Activity and destroy them?
            }
        }

        protected Type? GetAssociatedViewModelType(Type fromFragmentType)
        {
            var viewModelType = ViewModelTypeFinder?.FindTypeOrNull(fromFragmentType);
            return viewModelType ?? fromFragmentType.GetBasePresentationAttributes().First().ViewModelType;
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Register<MvxActivityPresentationAttribute>(ShowActivity, CloseActivity);
            AttributeTypesToActionsDictionary.Register<MvxFragmentPresentationAttribute>(ShowFragment, CloseFragment);
            AttributeTypesToActionsDictionary.Register<MvxDialogFragmentPresentationAttribute>(ShowDialogFragment, CloseFragmentDialog);
            AttributeTypesToActionsDictionary.Register<MvxTabLayoutPresentationAttribute>(ShowTabLayout, CloseViewPagerFragment);
            AttributeTypesToActionsDictionary.Register<MvxViewPagerFragmentPresentationAttribute>(ShowViewPagerFragment, CloseViewPagerFragment);
        }

        public override MvxBasePresentationAttribute GetPresentationAttribute(MvxViewModelRequest request)
        {
            ValidateArguments(request);

            var viewType = ViewsContainer?.GetViewType(request.ViewModelType);
            if (viewType == null)
                throw new InvalidOperationException($"Could not get view type for ViewModel Type: {request.ViewModelType}");

            var overrideAttribute = GetOverridePresentationAttribute(request, viewType);
            if (overrideAttribute != null)
                return overrideAttribute;

            IList<MvxBasePresentationAttribute> attributes =
                viewType.GetCustomAttributes<MvxBasePresentationAttribute>(true).ToList();
            if (attributes.Count > 0)
            {
                MvxBasePresentationAttribute? attribute = null;

                if (attributes.Count > 1)
                {
                    var fragmentAttributes = attributes.OfType<MvxFragmentPresentationAttribute>().ToArray();

                    // check if fragment can be displayed as child fragment first
                    attribute = GetAttributeForFragmentChildPresentation(fragmentAttributes);

                    // if attribute is still null, check if fragment can be displayed in current activity
                    attribute ??= GetAttributeForFragmentPresentation(fragmentAttributes);
                }

                // fallback to first attribute
                attribute ??= attributes[0];
                attribute.ViewType = viewType;

                return attribute;
            }

            return CreatePresentationAttribute(request.ViewModelType, viewType);
        }

        private MvxBasePresentationAttribute? GetAttributeForFragmentPresentation(
            IEnumerable<MvxFragmentPresentationAttribute> fragmentAttributes)
        {
            MvxBasePresentationAttribute? attribute = null;

            var currentActivityHostViewModelType = GetCurrentActivityViewModelType();

            foreach (var item in fragmentAttributes.Where(
                att => att.ActivityHostViewModelType != null))
            {
                if (CurrentActivity.IsActivityDead())
                    break;

                if (CurrentActivity!.FindViewById(item.FragmentContentId) != null &&
                    item.ActivityHostViewModelType == currentActivityHostViewModelType)
                {
                    attribute = item;
                    break;
                }
            }

            return attribute;
        }

        private MvxBasePresentationAttribute? GetAttributeForFragmentChildPresentation(
            IEnumerable<MvxFragmentPresentationAttribute> fragmentAttributes)
        {
            MvxBasePresentationAttribute? attribute = null;

            foreach (var item in fragmentAttributes.Where(
                att => att.FragmentHostViewType != null))
            {
                var fragment = GetFragmentByViewType(item.FragmentHostViewType);

                // if the fragment exists, and is on top, then use the current attribute 
                if (fragment?.IsVisible != true || fragment.View.FindViewById(item.FragmentContentId) == null)
                    continue;

                attribute = item;
                break;
            }

            return attribute;
        }

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType)
        {
            if (viewType == null)
                throw new ArgumentNullException(nameof(viewType));

            if (viewType.IsSubclassOf(typeof(DialogFragment)))
            {
                _logger.Value?.Log(LogLevel.Trace, "PresentationAttribute not found for {viewName}. Assuming DialogFragment presentation", viewType.Name);
                return new MvxDialogFragmentPresentationAttribute(enterAnimation: int.MinValue)
                {
                    ViewType = viewType,
                    ViewModelType = viewModelType
                };
            }

            if (viewType.IsSubclassOf(typeof(Fragment)))
            {
                _logger.Value?.Log(LogLevel.Trace, "PresentationAttribute not found for {viewName}. Assuming Fragment presentation", viewType.Name);
                return new MvxFragmentPresentationAttribute(GetCurrentActivityViewModelType(), global::Android.Resource.Id.Content)
                {
                    ViewType = viewType,
                    ViewModelType = viewModelType
                };
            }

            if (viewType.IsSubclassOf(typeof(Activity)))
            {
                _logger.Value?.Log(LogLevel.Trace, "PresentationAttribute not found for {viewName}. Assuming Activity presentation", viewType.Name);
                return new MvxActivityPresentationAttribute
                {
                    ViewType = viewType,
                    ViewModelType = viewModelType
                };
            }

            throw new InvalidOperationException($"Don't know how to create a presentation attribute for type {viewType}");
        }

        public override Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (hint == null)
                throw new ArgumentNullException(nameof(hint));

            if (hint is MvxPagePresentationHint pagePresentationHint)
            {
                var result = ChangePagePresentation(pagePresentationHint);
                return Task.FromResult(result);
            }

            return base.ChangePresentation(hint);
        }

        private bool ChangePagePresentation(MvxPagePresentationHint pagePresentationHint)
        {
            var request = new MvxViewModelRequest(pagePresentationHint.ViewModel);
            var attribute = GetPresentationAttribute(request);

            if (attribute is MvxViewPagerFragmentPresentationAttribute pagerFragmentAttribute)
            {
                var viewPager = FindViewPagerInFragmentPresentation(pagerFragmentAttribute);
                if (viewPager?.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
                {
                    var fragmentInfo = FindFragmentInfoFromAttribute(pagerFragmentAttribute, adapter);
                    var index = adapter.FragmentsInfo.IndexOf(fragmentInfo);
                    if (index < 0)
                    {
                        _logger.Value?.Log(LogLevel.Trace, "Did not find ViewPager index for {fragment}, skipping presentation change...", pagerFragmentAttribute.Tag);
                        return true;
                    }

                    viewPager.SetCurrentItem(index, true);
                    return true;
                }
            }

            return false;
        }

        protected virtual ViewPager? FindViewPagerInFragmentPresentation(
            MvxViewPagerFragmentPresentationAttribute pagerFragmentAttribute)
        {
            ValidateArguments(pagerFragmentAttribute);

            ViewPager? viewPager = null;

            // check for a ViewPager inside a Fragment
            if (pagerFragmentAttribute.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(pagerFragmentAttribute.FragmentHostViewType);
                viewPager = fragment?.View.FindViewById<ViewPager>(pagerFragmentAttribute.ViewPagerResourceId);
            }

            // check for a ViewPager inside an Activity
            if (viewPager == null && pagerFragmentAttribute.ActivityHostViewModelType != null &&
                CurrentActivity.IsActivityAlive())
            {
                viewPager = CurrentActivity!.FindViewById<ViewPager>(pagerFragmentAttribute.ViewPagerResourceId);
            }

            return viewPager;
        }

        protected Type? GetCurrentActivityViewModelType()
        {
            Type? currentActivityType = null;
            if (CurrentActivity.IsActivityAlive())
                currentActivityType = CurrentActivity!.GetType();

            if (currentActivityType == null)
                return null;

            return ViewModelTypeFinder?.FindTypeOrNull(currentActivityType);
        }

        #region Show implementations
        protected virtual Task<bool> ShowActivity(
            Type view,
            MvxActivityPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(view, attribute, request);

            var intent = CreateIntentForRequest(request);
            if (attribute.Extras != null)
                intent.PutExtras(attribute.Extras);

            ShowIntent(intent, CreateActivityTransitionOptions(intent, attribute, request));
            return Task.FromResult(true);
        }

        protected virtual Bundle CreateActivityTransitionOptions(
            Intent intent, MvxActivityPresentationAttribute attribute, MvxViewModelRequest request)
        {
            ValidateArguments(attribute, request);

            if (intent == null)
                throw new ArgumentNullException(nameof(intent));

            var bundle = Bundle.Empty!;

            if (!(CurrentActivity is IMvxAndroidSharedElements sharedElementsActivity))
            {
                return bundle;
            }

            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                _logger.Value?.Log(LogLevel.Warning, "Shared element transition requires Android v21+.");
                return bundle;
            }

            if (CurrentActivity.IsActivityAlive())
            {
                var (elements, transitionElementPairs) =
                    GetTransitionElements(attribute, request, sharedElementsActivity);

                if (transitionElementPairs.Count == 0)
                {
                    _logger.Value?.Log(LogLevel.Warning, "No transition elements are provided");
                    return bundle;
                }

                var transitionElementsBundle = CreateTransitionElementsBundle(intent, transitionElementPairs, elements);
                if (transitionElementsBundle != null)
                    return transitionElementsBundle;
            }

            return bundle;
        }

        private Bundle? CreateTransitionElementsBundle(
            Intent intent, IEnumerable<Pair> transitionElementPairs, IEnumerable<string> elements)
        {
            var activityOptions = ActivityOptions.MakeSceneTransitionAnimation(
                CurrentActivity, transitionElementPairs.ToArray());
            if (activityOptions == null)
                return null;

            intent.PutExtra(SharedElementsBundleKey, string.Join("|", elements));
            var activityOptionsBundle = activityOptions.ToBundle();
            return activityOptionsBundle;
        }

        private (List<string> elements, List<Pair> transitionElementPairs) GetTransitionElements(
            MvxBasePresentationAttribute attribute, MvxViewModelRequest request,
            IMvxAndroidSharedElements sharedElementsActivity)
        {
            var elements = new List<string>();
            var transitionElementPairs = new List<Pair>();

            foreach (var (key, value) in sharedElementsActivity.FetchSharedElementsToAnimate(attribute, request))
            {
                var transitionName = value.GetTransitionNameSupport();
                if (!string.IsNullOrEmpty(transitionName))
                {
                    var pair = Pair.Create(value, transitionName);
                    if (pair != null)
                    {
                        transitionElementPairs.Add(pair);
                        elements.Add($"{key}:{transitionName}");
                    }
                }
                else
                {
                    _logger.Value?.Log(LogLevel.Warning, "A XML transitionName is required in order to transition a control when navigating.");
                }
            }

            return (elements, transitionElementPairs);
        }

        protected virtual Intent CreateIntentForRequest(MvxViewModelRequest? request)
        {
            var requestTranslator = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelRequestTranslator>();

            if (request is MvxViewModelInstanceRequest viewModelInstanceRequest)
            {
                var intentWithKey = requestTranslator.GetIntentWithKeyFor(
                    viewModelInstanceRequest.ViewModelInstance,
                    viewModelInstanceRequest
                );

                return intentWithKey.intent;
            }

            return requestTranslator.GetIntentFor(request);
        }

        protected virtual void ShowIntent(Intent intent, Bundle? bundle)
        {
            if (intent == null)
                throw new ArgumentNullException(nameof(intent));

            var activity = CurrentActivity;
            if (activity.IsActivityDead())
            {
                _logger.Value?.Log(LogLevel.Warning, "Cannot Resolve current top activity. Creating new activity from Application Context");
                intent.AddFlags(ActivityFlags.NewTask);
                StartActivity(Application.Context, intent, bundle);
                return;
            }

            StartActivity(activity!, intent, bundle);
        }

        private static void StartActivity(Context context, Intent intent, Bundle? bundle)
        {
            if (bundle != null)
            {
                context.StartActivity(intent, bundle);
            }
            else
            {
                context.StartActivity(intent);
            }
        }

        protected virtual void ShowHostActivity(MvxFragmentPresentationAttribute attribute)
        {
            ValidateArguments(attribute);

            if (attribute.ActivityHostViewModelType == null)
                throw new ArgumentException("ActivityHostViewModelType not set on attribute");

            var viewType = ViewsContainer?.GetViewType(attribute.ActivityHostViewModelType);
            if (viewType?.IsSubclassOf(typeof(Activity)) != true)
                throw new MvxException("The host activity doesn't inherit Activity");

            var hostViewModelRequest = MvxViewModelRequest.GetDefaultRequest(attribute.ActivityHostViewModelType);
            if (PendingRequest != null)
                hostViewModelRequest.PresentationValues = PendingRequest.PresentationValues;

            Show(hostViewModelRequest);
        }

        protected virtual Task<bool> ShowFragment(
            Type view,
            MvxFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(view, attribute, request);

            // if attribute has a Fragment Host, then show it as nested and return
            if (attribute.FragmentHostViewType != null)
            {
                ShowNestedFragment(view, attribute, request);

                return Task.FromResult(true);
            }

            // if there is no Activity host associated, assume is the current activity
            if (attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            var currentHostViewModelType = GetCurrentActivityViewModelType();
            if (attribute.ActivityHostViewModelType != currentHostViewModelType)
            {
                _logger.Value?.Log(LogLevel.Warning, "Activity host with ViewModelType {activityHostViewModelType} is not CurrentTopActivity. Showing Activity before showing Fragment for {viewModelType}",
                    attribute.ActivityHostViewModelType, attribute.ViewModelType);
                PendingRequest = request;
                ShowHostActivity(attribute);
            }
            else if (CurrentActivity.IsActivityAlive())
            {
                if (CurrentActivity!.FindViewById(attribute!.FragmentContentId) == null)
                    throw new InvalidOperationException("FrameLayout to show Fragment not found");

                PerformShowFragmentTransaction(CurrentActivity.SupportFragmentManager, attribute, request);
            }
            return Task.FromResult(true);
        }

        protected virtual void ShowNestedFragment(
            Type view,
            MvxFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(view, attribute, request);

            // current implementation only supports one level of nesting 

            var fragmentHost = GetFragmentByViewType(attribute!.FragmentHostViewType);
            if (fragmentHost == null)
                throw new InvalidOperationException($"Fragment host not found when trying to show View {view.Name} as Nested Fragment");

            if (!fragmentHost.IsVisible)
                _logger.Value?.Log(LogLevel.Warning, "Fragment host is not visible when trying to show View {viewName} as Nested Fragment", view!.Name);

            PerformShowFragmentTransaction(fragmentHost.ChildFragmentManager, attribute, request);
        }

        protected virtual void PerformShowFragmentTransaction(
            FragmentManager fragmentManager,
            MvxFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(attribute, request);

            if (fragmentManager == null)
                throw new ArgumentNullException(nameof(fragmentManager));

            var fragmentName = attribute.Tag ?? attribute.ViewType.FragmentJavaName();

            IMvxFragmentView? fragmentView = null;
            if (attribute.IsCacheableFragment)
            {
                fragmentView = (IMvxFragmentView)fragmentManager.FindFragmentByTag(fragmentName);
            }

            if (fragmentView == null && attribute.ViewType != null)
                fragmentView = CreateFragment(fragmentManager, attribute, attribute.ViewType);

            var fragment = fragmentView.ToFragment();
            if (fragment == null)
                throw new MvxException($"Fragment {fragmentName} is null. Cannot perform Fragment Transaction.");

            // MvxNavigationService provides an already instantiated ViewModel here
            if (request is MvxViewModelInstanceRequest instanceRequest)
            {
                fragmentView!.ViewModel = instanceRequest.ViewModelInstance;
            }

            // save MvxViewModelRequest in the Fragment's Arguments
#pragma warning disable CA2000 // Dispose objects before losing scope
            var bundle = new Bundle();
#pragma warning restore CA2000 // Dispose objects before losing scope
            var serializedRequest = NavigationSerializer.Serializer.SerializeObject(request);
            bundle.PutString(ViewModelRequestBundleKey, serializedRequest);

            if (fragment.Arguments == null)
            {
                fragment.Arguments = bundle;
            }
            else
            {
                fragment.Arguments.Clear();
                fragment.Arguments.PutAll(bundle);
            }

            var ft = fragmentManager.BeginTransaction();

            OnBeforeFragmentChanging(ft, fragment, attribute, request);

            if (attribute.AddToBackStack)
                ft.AddToBackStack(fragmentName);

            OnFragmentChanging(ft, fragment, attribute, request);

            if (attribute.AddFragment && fragment.IsAdded)
            {
                ft.Show(fragment);
            }
            else if (attribute.AddFragment)
            {
                ft.Add(attribute.FragmentContentId, fragment, fragmentName);
            }
            else
            {
                ft.Replace(attribute.FragmentContentId, fragment, fragmentName);
            }

            ft.CommitAllowingStateLoss();

            OnFragmentChanged(ft, fragment, attribute, request);
        }

        protected virtual void OnBeforeFragmentChanging(
            FragmentTransaction fragmentTransaction,
            Fragment fragment,
            MvxFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (fragmentTransaction == null)
                throw new ArgumentNullException(nameof(fragmentTransaction));

            if (fragment == null)
                throw new ArgumentNullException(nameof(fragment));

            ValidateArguments(attribute, request);

            if (CurrentActivity.IsActivityAlive() && CurrentActivity is IMvxAndroidSharedElements sharedElementsActivity)
            {
                var elements = new List<string>();

                foreach (var (key, value) in sharedElementsActivity.FetchSharedElementsToAnimate(attribute, request))
                {
                    var transitionName = value.GetTransitionNameSupport();
                    if (!string.IsNullOrEmpty(transitionName))
                    {
                        fragmentTransaction.AddSharedElement(value, transitionName);
                        elements.Add($"{key}:{transitionName}");
                    }
                    else
                    {
                        _logger.Value?.Log(LogLevel.Warning, "A XML transitionName is required in order to transition a control when navigating.");
                    }
                }

                if (elements.Count > 0)
                    fragment.Arguments.PutString(SharedElementsBundleKey, string.Join("|", elements));
            }

            if (!attribute.EnterAnimation.Equals(int.MinValue) && !attribute.ExitAnimation.Equals(int.MinValue))
            {
                if (!attribute.PopEnterAnimation.Equals(int.MinValue) && !attribute.PopExitAnimation.Equals(int.MinValue))
                    fragmentTransaction.SetCustomAnimations(attribute.EnterAnimation, attribute.ExitAnimation, attribute.PopEnterAnimation, attribute.PopExitAnimation);
                else
                    fragmentTransaction.SetCustomAnimations(attribute.EnterAnimation, attribute.ExitAnimation);
            }

            if (attribute.TransitionStyle != int.MinValue)
                fragmentTransaction.SetTransitionStyle(attribute.TransitionStyle);
        }

        protected virtual void OnFragmentChanged(FragmentTransaction? fragmentTransaction, Fragment? fragment, MvxFragmentPresentationAttribute? attribute, MvxViewModelRequest? request)
        {
        }

        protected virtual void OnFragmentChanging(FragmentTransaction? fragmentTransaction, Fragment? fragment, MvxFragmentPresentationAttribute? attribute, MvxViewModelRequest? request)
        {
        }

        protected virtual void OnFragmentPopped(FragmentTransaction? fragmentTransaction, Fragment? fragment, MvxFragmentPresentationAttribute? attribute)
        {
        }

        protected virtual Task<bool> ShowDialogFragment(
            Type view,
            MvxDialogFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(view, attribute, request);

            if (CurrentActivity == null)
                throw new InvalidOperationException("CurrentActivity is null");

            if (CurrentFragmentManager == null)
                throw new InvalidOperationException("CurrentFragmentManager is null. Cannot create Fragment Transaction.");

            if (attribute.ViewType == null)
                throw new InvalidOperationException($"{nameof(MvxDialogFragmentPresentationAttribute)}.ViewType is null");

            var fragmentName = attribute.Tag ?? attribute.ViewType.FragmentJavaName();
            IMvxFragmentView mvxFragmentView = CreateFragment(CurrentActivity.SupportFragmentManager, attribute, attribute.ViewType);
            var dialog = (DialogFragment)mvxFragmentView;

            // MvxNavigationService provides an already instantiated ViewModel here,
            // therefore just assign it
            if (request is MvxViewModelInstanceRequest instanceRequest)
            {
                mvxFragmentView.ViewModel = instanceRequest.ViewModelInstance;
            }
            else
            {
                mvxFragmentView.LoadViewModelFrom(request, null);
            }

            dialog.Cancelable = attribute.Cancelable;

            var ft = CurrentFragmentManager.BeginTransaction();

            OnBeforeFragmentChanging(ft, dialog, attribute, request);

            if (attribute.AddToBackStack)
                ft.AddToBackStack(fragmentName);

            OnFragmentChanging(ft, dialog, attribute, request);

            dialog.Show(ft, fragmentName);

            OnFragmentChanged(ft, dialog, attribute, request);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> ShowViewPagerFragment(
            Type view,
            MvxViewPagerFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(view, attribute, request);

            // if the attribute doesn't supply any host, assume current activity!
            if (attribute.FragmentHostViewType == null && attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            ViewPager? viewPager = null;
            FragmentManager? fragmentManager = null;

            // check for a ViewPager inside a Fragment
            if (attribute.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragment == null)
                    throw new MvxException("Fragment not found", attribute.FragmentHostViewType.Name);

                if (fragment.View == null)
                    throw new MvxException("Fragment.View is null. Please consider calling Navigate later in your code",
                        attribute!.FragmentHostViewType.Name);

                viewPager = fragment.View.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = fragment.ChildFragmentManager;
            }

            // check for a ViewPager inside an Activity
            if (attribute.ActivityHostViewModelType != null)
            {
                var currentActivityViewModelType = GetCurrentActivityViewModelType();

                // if the host Activity is not the top-most Activity, then show it before proceeding, and return false for now
                if (attribute.ActivityHostViewModelType != currentActivityViewModelType)
                {
                    PendingRequest = request;
                    ShowHostActivity(attribute);
                    return Task.FromResult(false);
                }

                if (CurrentActivity.IsActivityAlive())
                    viewPager = CurrentActivity!.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = CurrentFragmentManager;
            }

            // no more cases to check. Just throw if ViewPager wasn't found
            if (viewPager == null)
                throw new MvxException("ViewPager not found");

            var tag = attribute.Tag ?? attribute.ViewType.FragmentJavaName();
            var fragmentInfo = new MvxViewPagerFragmentInfo(attribute.Title, tag, attribute.ViewType, request);

            if (viewPager.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
            {
                adapter.FragmentsInfo.Add(fragmentInfo);
                adapter.NotifyDataSetChanged();
            }
            else
            {
                viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(
                    CurrentActivity,
                    fragmentManager,
                    new List<MvxViewPagerFragmentInfo>
                    {
                        fragmentInfo
                    }
                );
            }

            return Task.FromResult(true);
        }

        protected virtual async Task<bool> ShowTabLayout(
            Type view,
            MvxTabLayoutPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(view, attribute, request);

            var showViewPagerFragment = await ShowViewPagerFragment(view, attribute, request).ConfigureAwait(true);
            if (!showViewPagerFragment)
                return false;

            ViewPager? viewPager = null;
            TabLayout? tabLayout = null;

            // check for a ViewPager inside a Fragment
            if (attribute.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);

                viewPager = fragment?.View.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                tabLayout = fragment?.View.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
            }

            // check for a ViewPager inside an Activity
            if (CurrentActivity.IsActivityAlive() && attribute?.ActivityHostViewModelType != null)
            {
                viewPager = CurrentActivity?.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                tabLayout = CurrentActivity?.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
            }

            if (viewPager == null || tabLayout == null)
                throw new MvxException("ViewPager or TabLayout not found");

            tabLayout.SetupWithViewPager(viewPager);
            return true;
        }

        #endregion

        #region Close implementations
        protected virtual Task<bool> CloseActivity(IMvxViewModel viewModel, MvxActivityPresentationAttribute? attribute)
        {
            var currentView = CurrentActivity as IMvxView;

            if (currentView == null)
            {
                _logger.Value?.Log(LogLevel.Warning, "Ignoring close for viewmodel - rootframe has no current page");
                return Task.FromResult(false);
            }

            if (currentView.ViewModel != viewModel)
            {
                _logger.Value?.Log(LogLevel.Warning, "Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return Task.FromResult(false);
            }

            // don't kill the dead
            if (CurrentActivity.IsActivityAlive())
                CurrentActivity!.Finish();

            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseFragmentDialog(
            IMvxViewModel viewModel, MvxDialogFragmentPresentationAttribute attribute)
        {
            ValidateArguments(attribute);

            string tag = attribute.Tag ?? attribute.ViewType.FragmentJavaName();
            var toClose = CurrentFragmentManager?.FindFragmentByTag(tag);
            if (toClose is DialogFragment dialog)
            {
                dialog.DismissAllowingStateLoss();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        protected virtual bool CloseFragments()
        {
            try
            {
                CurrentFragmentManager?.PopBackStackImmediate();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (System.Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                _logger.Value?.Log(LogLevel.Warning, ex, "Cannot close any fragments");
            }
            return true;
        }

        protected virtual Task<bool> CloseFragment(
            IMvxViewModel viewModel, MvxFragmentPresentationAttribute attribute)
        {
            ValidateArguments(attribute);

            // try to close nested fragment first
            if (attribute.FragmentHostViewType != null)
            {
                var fragmentHost = GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragmentHost != null
                    && TryPerformCloseFragmentTransaction(fragmentHost.ChildFragmentManager, attribute))
                    return Task.FromResult(true);
            }

            // Close fragment. If it isn't successful, then close the current Activity
            if (CurrentFragmentManager != null && TryPerformCloseFragmentTransaction(CurrentFragmentManager, attribute))
            {
                return Task.FromResult(true);
            }

            if (CurrentActivity.IsActivityAlive())
            {
                CurrentActivity!.Finish();
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        protected virtual bool TryPerformCloseFragmentTransaction(
            FragmentManager fragmentManager,
            MvxFragmentPresentationAttribute fragmentAttribute)
        {
            ValidateArguments(fragmentAttribute);

            if (fragmentManager == null)
                throw new ArgumentNullException(nameof(fragmentManager));

            try
            {
                var fragmentName = fragmentAttribute.Tag ?? fragmentAttribute.ViewType.FragmentJavaName();
                if (fragmentManager.BackStackEntryCount > 0)
                {
                    PopOnBackstackEntries(fragmentName, fragmentManager, fragmentAttribute);
                    return true;
                }

                Fragment fragmentToPop = fragmentManager.FindFragmentByTag(fragmentName);
                if (fragmentToPop != null)
                {
                    PopFragment(fragmentManager, fragmentAttribute, fragmentToPop);
                    return true;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (System.Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                _logger.Value?.Log(LogLevel.Error, ex, "Cannot close fragment transaction");
                return false;
            }

            return false;
        }

        private void PopFragment(FragmentManager fragmentManager, MvxFragmentPresentationAttribute fragmentAttribute,
            Fragment fragmentToPop)
        {
            var ft = fragmentManager.BeginTransaction();

            if (!fragmentAttribute.EnterAnimation.Equals(int.MinValue) &&
                !fragmentAttribute.ExitAnimation.Equals(int.MinValue))
            {
                if (!fragmentAttribute.PopEnterAnimation.Equals(int.MinValue) &&
                    !fragmentAttribute.PopExitAnimation.Equals(int.MinValue))
                {
                    ft.SetCustomAnimations(
                        fragmentAttribute.EnterAnimation,
                        fragmentAttribute.ExitAnimation,
                        fragmentAttribute.PopEnterAnimation,
                        fragmentAttribute.PopExitAnimation);
                }
                else
                {
                    ft.SetCustomAnimations(
                        fragmentAttribute.EnterAnimation,
                        fragmentAttribute.ExitAnimation);
                }
            }

            if (fragmentAttribute.TransitionStyle != int.MinValue)
                ft.SetTransitionStyle(fragmentAttribute.TransitionStyle);

            ft.Remove(fragmentToPop);
            ft.CommitAllowingStateLoss();

            OnFragmentPopped(ft, fragmentToPop, fragmentAttribute);
        }

        private void PopOnBackstackEntries(
            string fragmentName, FragmentManager fragmentManager, MvxFragmentPresentationAttribute fragmentAttribute)
        {
            var popBackStackFragmentName =
                string.IsNullOrEmpty(fragmentAttribute.PopBackStackImmediateName?.Trim())
                    ? fragmentName
                    : fragmentAttribute.PopBackStackImmediateName;

            fragmentManager.PopBackStackImmediate(
                popBackStackFragmentName,
                (int)fragmentAttribute.PopBackStackImmediateFlag.ToNativePopBackStackFlags());

            OnFragmentPopped(null, null, fragmentAttribute);
        }

        protected virtual Task<bool> CloseViewPagerFragment(
            IMvxViewModel? viewModel,
            MvxViewPagerFragmentPresentationAttribute attribute)
        {
            ValidateArguments(attribute);

            ViewPager? viewPager = null;
            FragmentManager? fragmentManager;

            if (attribute.FragmentHostViewType != null)
            {
                var fragment = GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragment == null)
                    throw new MvxException("Fragment not found", attribute.FragmentHostViewType.Name);

                viewPager = fragment.View.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = fragment.ChildFragmentManager;
            }
            else
            {
                if (CurrentActivity.IsActivityAlive())
                    viewPager = CurrentActivity!.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                fragmentManager = CurrentFragmentManager;
            }

            if (viewPager?.Adapter is MvxCachingFragmentStatePagerAdapter adapter && fragmentManager != null)
            {
                var ft = fragmentManager.BeginTransaction();
                var fragmentInfo = FindFragmentInfoFromAttribute(attribute, adapter);
                if (fragmentInfo != null)
                {
                    var fragment = fragmentManager.FindFragmentByTag(fragmentInfo.Tag);
                    adapter.FragmentsInfo.Remove(fragmentInfo);
                    ft.Remove(fragment);
                    ft.CommitAllowingStateLoss();
                    adapter.NotifyDataSetChanged();

                    OnFragmentPopped(ft, fragment, attribute);
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }

        protected virtual MvxViewPagerFragmentInfo? FindFragmentInfoFromAttribute(
            MvxFragmentPresentationAttribute attribute,
            MvxCachingFragmentStatePagerAdapter adapter)
        {
            ValidateArguments(attribute);

            if (adapter == null)
                throw new ArgumentNullException(nameof(adapter));

            MvxViewPagerFragmentInfo? fragmentInfo = null;
            if (attribute.Tag != null)
            {
                fragmentInfo = adapter.FragmentsInfo?.FirstOrDefault(f => f.Tag == attribute.Tag);
            }

            if (fragmentInfo != null)
                return fragmentInfo;

            bool IsMatch(MvxViewPagerFragmentInfo? info)
            {
                if (attribute.ViewType == null) return false;

                var viewTypeMatches = info?.FragmentType == attribute.ViewType;

                if (attribute.ViewModelType != null)
                    return viewTypeMatches && info?.Request?.ViewModelType == attribute.ViewModelType;

                return viewTypeMatches;
            }

            fragmentInfo = adapter.FragmentsInfo?.FirstOrDefault(IsMatch);
            return fragmentInfo;
        }
        #endregion

        protected virtual IMvxFragmentView CreateFragment(
            FragmentManager fragmentManager,
            MvxBasePresentationAttribute attribute,
            Type fragmentType)
        {
            ValidateArguments(attribute);

            if (fragmentManager == null)
                throw new ArgumentNullException(nameof(fragmentManager));

            if (fragmentType == null)
                throw new ArgumentNullException(nameof(fragmentType));

            try
            {
                var fragmentClass = Class.FromType(fragmentType);
                var fragment = (IMvxFragmentView)fragmentManager.FragmentFactory.Instantiate(
                    fragmentClass.ClassLoader, fragmentClass.Name);
                return fragment;
            }
            catch (System.Exception ex)
            {
                _logger.Value?.Log(LogLevel.Error, ex, "Cannot create Fragment {fragmentName}", fragmentType.Name);
                throw new MvxException(ex, $"Cannot create Fragment '{fragmentType.Name}'");
            }
        }

        protected virtual Fragment? GetFragmentByViewType(Type? type)
        {
            if (type == null)
                return null;

            if (CurrentFragmentManager == null)
                return null;

            var fragmentName = type.FragmentJavaName();
            var fragment = CurrentFragmentManager.FindFragmentByTag(fragmentName);

            if (fragment != null)
            {
                return fragment;
            }

            return FindFragmentInChildren(fragmentName, CurrentFragmentManager);
        }

        protected virtual Fragment? FindFragmentInChildren(string? fragmentName, FragmentManager? fragManager)
        {
            if (string.IsNullOrWhiteSpace(fragmentName))
                return null;

            if (fragManager == null)
                return null;

            if (fragManager.BackStackEntryCount == 0)
                return null;

            for (int i = 0; i < fragManager.BackStackEntryCount; i++)
            {
                var parentFrag = fragManager.FindFragmentById(fragManager.GetBackStackEntryAt(i).Id);

                //let's try again finding it
                var frag = parentFrag?.ChildFragmentManager?.FindFragmentByTag(fragmentName);

                if (frag == null)
                {
                    //reloop for other fragments
                    frag = FindFragmentInChildren(fragmentName, parentFrag?.ChildFragmentManager);
                }

                //if we found the frag lets return it!
                if (frag != null)
                {
                    return frag;
                }
            }

            return null;
        }

        private static void ValidateArguments(Type? view, MvxBasePresentationAttribute? attribute, MvxViewModelRequest? request)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            ValidateArguments(attribute, request);
        }

        private static void ValidateArguments(MvxBasePresentationAttribute? attribute, MvxViewModelRequest? request)
        {
            ValidateArguments(attribute);

            ValidateArguments(request);
        }

        private static void ValidateArguments(MvxBasePresentationAttribute? attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));
        }

        private static void ValidateArguments(MvxViewModelRequest? request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
        }
    }
#nullable restore
}
