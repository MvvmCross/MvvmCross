// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.OS;
using AndroidX.Fragment.App;
using Java.Interop;
using Java.Lang;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.ViewModels;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using JavaObject = Java.Lang.Object;
using JavaString = Java.Lang.String;

namespace MvvmCross.Platforms.Android.Views.ViewPager
{
    [Register("mvvmcross.platforms.android.views.viewpager.MvxCachingFragmentStatePagerAdapter")]
    public class MvxCachingFragmentStatePagerAdapter : MvxCachingFragmentPagerAdapter
    {
        public const string ViewPagerFragmentsInfoBundleKey = "__mvxViewPagerFragmentsInfo";

        private readonly Type _activityType;

        public List<MvxViewPagerFragmentInfo> FragmentsInfo { get; }

        public override int Count => FragmentsInfo?.Count ?? 0;

        protected MvxCachingFragmentStatePagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            _activityType = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity.GetType();
        }

        public MvxCachingFragmentStatePagerAdapter(FragmentManager fragmentManager,
            List<MvxViewPagerFragmentInfo> fragmentsInfo) : base(fragmentManager)
        {
            FragmentsInfo = fragmentsInfo;
            _activityType = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity.GetType();
        }

        public override Fragment GetItem(int position, Fragment.SavedState fragmentSavedState = null)
        {
            var fragmentInfo = FragmentsInfo[position];
            var fragmentClass = Class.FromType(fragmentInfo.FragmentType);
            var fragment = FragmentFactory.Instantiate(
                fragmentClass.ClassLoader,
                fragmentClass.Name
            );

            if (fragment is not IMvxFragmentView mvxFragment)
            {
                return fragment;
            }

            if (mvxFragment.GetType().IsFragmentCacheable(_activityType) && fragmentSavedState != null)
            {
                return fragment;
            }

            mvxFragment.ViewModel = GetViewModel(fragmentInfo);

            fragment.Arguments = GetArguments(fragmentInfo);

            // If the MvxViewPagerFragmentInfo for this position doesn't have the ViewModel, overwrite it with a new MvxViewPagerFragmentInfo that has the ViewModel we just created.
            // Not doing this means the ViewModel gets recreated every time the Fragment gets recreated!
            if (fragmentInfo is { Request: not MvxViewModelInstanceRequest })
            {
                var viewModelInstanceRequest = new MvxViewModelInstanceRequest(mvxFragment.ViewModel);
                var newFragInfo = new MvxViewPagerFragmentInfo(fragmentInfo.Title, fragmentInfo.Tag, fragmentInfo.FragmentType, viewModelInstanceRequest);
                FragmentsInfo[position] = newFragInfo;
            }

            return fragment;
        }

        public override int GetItemPosition(JavaObject @object)
        {
            return PositionNone;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new JavaString(FragmentsInfo[position].Title);
        }

        protected override string GetTag(int position)
        {
            return FragmentsInfo[position].Tag;
        }

        private static IMvxViewModel GetViewModel(MvxViewPagerFragmentInfo fragmentInfo)
        {
            if (fragmentInfo.Request is MvxViewModelInstanceRequest instanceRequest)
            {
                return instanceRequest.ViewModelInstance;
            }

            var viewModelLoader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();

            return viewModelLoader.LoadViewModel(fragmentInfo.Request, null);
        }

        private static Bundle GetArguments(MvxViewPagerFragmentInfo fragmentInfo)
        {
            var navigationSerializer = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();

            var serializedRequest = navigationSerializer.Serializer.SerializeObject(fragmentInfo.Request);

            var bundle = new Bundle();

            bundle.PutString(MvxAndroidViewPresenter.ViewModelRequestBundleKey, serializedRequest);

            return bundle;
        }

        public override IParcelable SaveState()
        {
            var bundle = base.SaveState() as Bundle;

            SaveFragmentsInfoState(bundle);

            return bundle;
        }

        public override void RestoreState(IParcelable state, ClassLoader loader)
        {
            base.RestoreState(state, loader);

            RestoreFragmentsInfoState(state as Bundle);
        }

        private void SaveFragmentsInfoState(Bundle bundle)
        {
            if (bundle == null || FragmentsInfo == null || FragmentsInfo.Count == 0)
                return;

            var fragmentInfoParcelables = new IParcelable[FragmentsInfo.Count];

            for (var i = 0; i < FragmentsInfo.Count; i++)
            {
                var fragInfo = FragmentsInfo[i];
                var parcelable = new ViewPagerFragmentInfoParcelable()
                {
                    FragmentType = fragInfo.FragmentType,
                    ViewModelType = fragInfo.Request.ViewModelType,
                    Title = fragInfo.Title,
                    Tag = fragInfo.Tag
                };
                fragmentInfoParcelables[i] = parcelable;
            }

            bundle.PutParcelableArray(ViewPagerFragmentsInfoBundleKey, fragmentInfoParcelables);
        }

        private void RestoreFragmentsInfoState(Bundle bundle)
        {
            if (bundle == null || FragmentsInfo == null)
                return;

            var fragmentInfoParcelables = BundleCompat.GetParcelableArray(bundle, ViewPagerFragmentsInfoBundleKey, Class.FromType(typeof(ViewPagerFragmentInfoParcelable)));

            if (fragmentInfoParcelables == null)
                return;

            var fragments = Fragments;

            for (var i = 0; i < fragmentInfoParcelables.Length; i++)
            {
                var parcelable = (ViewPagerFragmentInfoParcelable)fragmentInfoParcelables[i];

                MvxViewPagerFragmentInfo fragInfo = null;

                if (i < fragments.Count && fragments[i] is IMvxFragmentView mvxFragment && mvxFragment.ViewModel != null)
                {
                    // The fragment was already restored by Android with its old ViewModel (cached by MvvmCross).
                    // Add the ViewModel to the FragmentInfo object so the adapter won't instantiate a new one.
                    var viewModelInstanceRequest = new MvxViewModelInstanceRequest(mvxFragment.ViewModel);
                    fragInfo = new MvxViewPagerFragmentInfo(parcelable.Title, parcelable.Tag, parcelable.FragmentType, viewModelInstanceRequest);
                }

                if (fragInfo == null)
                {
                    // Either the fragment doesn't exist or it doesn't have a ViewModel. 
                    // Fall back to a FragmentInfo with the ViewModelType. The adapter will create a ViewModel in GetItem where we will add it to the FragmentInfo.
                    var viewModelRequest = new MvxViewModelRequest(parcelable.ViewModelType);
                    fragInfo = new MvxViewPagerFragmentInfo(parcelable.Title, parcelable.Tag, parcelable.FragmentType, viewModelRequest);
                }

                FragmentsInfo.Add(fragInfo);
            }

            NotifyDataSetChanged();
        }

        private sealed class ViewPagerFragmentInfoParcelable : JavaObject, IParcelable
        {
            public Type FragmentType { get; init; }
            public Type ViewModelType { get; init; }
            public string Title { get; init; }
            public string Tag { get; init; }

            [ExportField("CREATOR")]
            public static ViewPagerFragmentInfoParcelableCreator InititalizeCreator()
            {
                return new ViewPagerFragmentInfoParcelableCreator();
            }

            public ViewPagerFragmentInfoParcelable()
            {
            }

            public ViewPagerFragmentInfoParcelable(Parcel source)
            {
                string fragmentType = source.ReadString();
                string viewModelType = source.ReadString();
                Title = source.ReadString();
                Tag = source.ReadString();

                FragmentType = Type.GetType(fragmentType);
                ViewModelType = Type.GetType(viewModelType);
            }

            public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
            {
                dest.WriteString(FragmentType.AssemblyQualifiedName);
                dest.WriteString(ViewModelType.AssemblyQualifiedName);
                dest.WriteString(Title);
                dest.WriteString(Tag);
            }

            public int DescribeContents()
            {
                return 0;
            }
        }

        private sealed class ViewPagerFragmentInfoParcelableCreator : JavaObject, IParcelableCreator
        {
            public JavaObject CreateFromParcel(Parcel source)
            {
                return new ViewPagerFragmentInfoParcelable(source);
            }

            public JavaObject[] NewArray(int size)
            {
                return new ViewPagerFragmentInfoParcelable[size];
            }
        }
    }
}
