// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Java.Lang;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using JavaObject = Java.Lang.Object;
using JavaString = Java.Lang.String;

namespace MvvmCross.Droid.Support.V4
{
    [Register("mvvmcross.droid.support.v4.MvxCachingFragmentStatePagerAdapter")]
    public class MvxCachingFragmentStatePagerAdapter : MvxCachingFragmentPagerAdapter
    {
        private readonly Context _context;
        private readonly Type _activityType;

        public List<MvxViewPagerFragmentInfo> FragmentsInfo { get; }

        public override int Count => FragmentsInfo?.Count ?? 0;

        protected MvxCachingFragmentStatePagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            _activityType = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity.GetType();
        }

        public MvxCachingFragmentStatePagerAdapter(Context context, FragmentManager fragmentManager,
            List<MvxViewPagerFragmentInfo> fragmentsInfo) : base(fragmentManager)
        {
            _context = context;
            FragmentsInfo = fragmentsInfo;
            _activityType = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity.GetType();
        }

        public override Fragment GetItem(int position, Fragment.SavedState fragmentSavedState = null)
        {
            var fragmentInfo = FragmentsInfo.ElementAt(position);
            var fragment = Fragment.Instantiate(_context, fragmentInfo.FragmentType.FragmentJavaName());

            if (!(fragment is IMvxFragmentView mvxFragment))
            {
                return fragment;
            }

            if (mvxFragment.GetType().IsFragmentCacheable(_activityType) && fragmentSavedState != null)
            {
                return fragment;
            }

            mvxFragment.ViewModel = GetViewModel(fragmentInfo);

            return fragment;
        }

        public override int GetItemPosition(JavaObject @object)
        {
            return PositionNone;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new JavaString(FragmentsInfo.ElementAt(position).Title);
        }

        protected override string GetTag(int position)
        {
            return FragmentsInfo.ElementAt(position).Tag;
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
    }
}
