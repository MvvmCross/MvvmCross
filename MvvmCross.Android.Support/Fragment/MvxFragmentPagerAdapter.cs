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
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using JavaString = Java.Lang.String;

namespace MvvmCross.Droid.Support.V4
{
    [Register("mvvmcross.droid.support.v4.MvxFragmentPagerAdapter")]
    public class MvxFragmentPagerAdapter : FragmentPagerAdapter
    {
        private readonly Context _context;

        public IEnumerable<MvxViewPagerFragmentInfo> Fragments { get; }

        public override int Count => Fragments.Count();

        protected MvxFragmentPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public MvxFragmentPagerAdapter(
            Context context, FragmentManager fragmentManager, IEnumerable<MvxViewPagerFragmentInfo> fragments)
            : base(fragmentManager)
        {
            _context = context;
            Fragments = fragments;
        }

        public override Fragment GetItem(int position)
        {
            var fragmentInfo = Fragments.ElementAt(position);

            if (fragmentInfo.CachedFragment != null)
            {
                return fragmentInfo.CachedFragment;
            }

            fragmentInfo.CachedFragment = 
                Fragment.Instantiate(_context, fragmentInfo.FragmentType.FragmentJavaName());

            if (!(fragmentInfo.CachedFragment is IMvxFragmentView mvxFragment))
            {
                return fragmentInfo.CachedFragment;
            }

            mvxFragment.ViewModel = GetViewModel(fragmentInfo);

            fragmentInfo.CachedFragment.Arguments = GetArguments(fragmentInfo);

            return fragmentInfo.CachedFragment;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new JavaString(Fragments.ElementAt(position).Title);
        }

        public override void RestoreState(IParcelable state, ClassLoader loader)
        {
            //Don't call restore to prevent crash on rotation
            //base.RestoreState (state, loader);
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
    }
}
