using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Util;
using Android.Support.V4.View;
using Android.Support.V7.App;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public class MvxAppCompatViewPresenter : MvxAndroidViewPresenter
    {
        public MvxAppCompatViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

        protected new FragmentManager CurrentFragmentManager 
        {
            get
            {
                if(CurrentActivity is AppCompatActivity activity)
                    return activity.SupportFragmentManager;
                throw new InvalidCastException("Cannot use Android Support Fragment within non AppCompat Activity");
            }
        }

        protected override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();
            AttributeTypesToShowMethodDictionary.Add(
               typeof(MvxTabAttribute),
               (view, attribute, request) => ShowTab(view, (MvxTabAttribute)attribute, request));
        }

        protected override void ShowDialogFragment(Type view,
           MvxDialogAttribute attribute,
           MvxViewModelRequest request)
        {
            var fragmentName = FragmentJavaName(attribute.ViewType);
            var dialog = (DialogFragment)CreateFragment(fragmentName);
            dialog.Cancelable = attribute.Cancelable;
            dialog.Show(CurrentFragmentManager, fragmentName);
        }

        protected override void ShowActivity(Type view, MvxActivityAttribute attribute, MvxViewModelRequest request)
        {
            var intent = CreateIntentForRequest(request);
            if(attribute.Extras != null)
                intent.PutExtras(attribute.Extras);

            var activity = CurrentActivity;
            if (activity == null)
            {
                MvxTrace.Warning("Cannot Resolve current top activity");
                return;
            }

            if (attribute.SharedElements != null)
            {
                IList<Pair> sharedElements = new List<Pair>();
                foreach (var item in attribute.SharedElements)
                {
                    intent.PutExtra(item.Key, ViewCompat.GetTransitionName(item.Value));
                    sharedElements.Add(Pair.Create(item.Value, item.Key));
                }
                ActivityOptionsCompat options = ActivityOptionsCompat.MakeSceneTransitionAnimation(CurrentActivity, sharedElements.ToArray());
                activity.StartActivity(intent, options.ToBundle());
            }
            else
                activity.StartActivity(intent);
        }

        protected override IMvxFragmentView CreateFragment(string fragmentName)
        {
            var fragment = Fragment.Instantiate(CurrentActivity, fragmentName);
            return (IMvxFragmentView)fragment;
        }

        protected override async Task ShowHostActivity(MvxFragmentAttribute attribute)
        {
            var currentHostViewModelType = GetCurrentActivityViewModelType();
            if (attribute.ActivityHostViewModelType != currentHostViewModelType)
            {
                var viewType = ViewsContainer.GetViewType(attribute.ActivityHostViewModelType);
                if (!viewType.IsSubclassOf(typeof(FragmentActivity)))
                    throw new MvxException("The host activity doesnt inherit FragmentActivity");

                var hostViewModelRequest = MvxViewModelRequest.GetDefaultRequest(attribute.ActivityHostViewModelType);
                Show(hostViewModelRequest);

                int tries = 10;
                while ((GetCurrentActivityViewModelType() != attribute.ActivityHostViewModelType) && (tries > 0))
                {
                    await Task.Delay(1);
                    tries--;
                }
                if (tries == 0)
                    throw new MvxException("Cannot load activity for Fragment");
            }
        }

        protected virtual void ShowTab(
            Type view,
            MvxTabAttribute attribute,
            MvxViewModelRequest request)
        {
            if (attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();

            Task.Run(async () => {
                await ShowHostActivity(attribute);
            }).ContinueWith((result) => {
                Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(() => {
                    if (CurrentActivity.FindViewById(attribute.FragmentContentId) == null)
                        throw new NullReferenceException("FrameLayout to show Fragment not found");

                    var viewPager = CurrentActivity.FindViewById<ViewPager>(attribute.ViewPagerResourceId);
                    var tabLayout = CurrentActivity.FindViewById<TabLayout>(attribute.TabLayoutResourceId);
                    if (viewPager != null && tabLayout != null)
                    {
                        if (viewPager.Adapter is MvxCachingFragmentStatePagerAdapter adapter)
                        {
                            //TODO: Check if adapter already contains this Tab and just navigate to it
                            //var index = adapter.Fragments.FindIndex(f => f.Tag == attribute.Title);
                            //viewPager.CurrentItem = index > -1 ? index : 0;

                            adapter.Fragments.Add(new MvxCachingFragmentStatePagerAdapter.FragmentInfo(attribute.Title, attribute.ViewType, attribute.ViewModelType));
                        }
                        else
                        {
                            var fragments = new List<MvxCachingFragmentStatePagerAdapter.FragmentInfo>
                            {
                                new MvxCachingFragmentStatePagerAdapter.FragmentInfo(attribute.Title, attribute.ViewType, attribute.ViewModelType),
                            };
                            //TODO: Maybe we need to use ChildFragmentManager here if it is nested
                            viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(CurrentActivity, CurrentFragmentManager, fragments);
                        }
                        tabLayout.SetupWithViewPager(viewPager);
                    }
                    else
                        throw new MvxException("ViewPager or TabLayout not found");
                });
            });
        }

        protected override void ShowFragment(Type view,
            MvxFragmentAttribute attribute,
            MvxViewModelRequest request)
        {
            if (attribute.ActivityHostViewModelType == null)
                attribute.ActivityHostViewModelType = GetCurrentActivityViewModelType();
            
            Task.Run(async () => {
                await ShowHostActivity(attribute);
            }).ContinueWith((result) => {
                Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(() => {
                    if (CurrentActivity.FindViewById(attribute.FragmentContentId) == null)
                        throw new NullReferenceException("FrameLayout to show Fragment not found");

                    var fragmentName = FragmentJavaName(attribute.ViewType);
                    var fragment = CreateFragment(fragmentName);

                    var ft = CurrentFragmentManager.BeginTransaction();
                    if (attribute.SharedElements != null)
                    {
                        foreach (var item in attribute.SharedElements)
                        {
                            string name = item.Key;
                            if (string.IsNullOrEmpty(name))
                                name = ViewCompat.GetTransitionName(item.Value);
                            ft.AddSharedElement(item.Value, name);
                        }
                    }
                    if (!attribute.CustomAnimations.Equals((int.MinValue, int.MinValue, int.MinValue, int.MinValue)))
                    {
                        var customAnimations = attribute.CustomAnimations;
                        ft.SetCustomAnimations(customAnimations.enter, customAnimations.exit, customAnimations.popEnter, customAnimations.popExit);
                    }
                    if (attribute.TransitionStyle != int.MinValue)
                        ft.SetTransitionStyle(attribute.TransitionStyle);

                    ft.Replace(attribute.FragmentContentId, (Fragment)fragment, fragmentName);
                    ft.CommitNowAllowingStateLoss();
                });
            });
        }

        protected override MvxBasePresentationAttribute GetAttributeForViewModel(Type viewModelType)
        {
            IList<MvxBasePresentationAttribute> attributes;
            if (ViewModelToPresentationAttributeMap.TryGetValue(viewModelType, out attributes))
            {
                var attribute = attributes.FirstOrDefault();
                if (attribute.ViewType?.GetInterfaces().OfType<IMvxOverridePresentationAttribute>().FirstOrDefault() is IMvxOverridePresentationAttribute view)
                {
                    var presentationAttribute = view.PresentationAttribute();

                    if (presentationAttribute != null)
                        return presentationAttribute;
                }
                return attribute;
            }

            var viewType = ViewsContainer.GetViewType(viewModelType);
            if (viewType.GetInterfaces().Contains(typeof(Android.Content.IDialogInterface)))
                return new MvxDialogAttribute();
            if (viewType.IsSubclassOf(typeof(Fragment)))
                return new MvxFragmentAttribute(GetCurrentActivityViewModelType());

            return new MvxActivityAttribute() { ViewModelType = viewModelType };
        }
    }
}
