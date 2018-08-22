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
using MvvmCross.ViewModels;
using MvvmCross.Views;
using String = Java.Lang.String;

namespace MvvmCross.Droid.Support.V4
{
    [Register("mvvmcross.droid.support.v4.MvxFragmentStatePagerAdapter")]
    public class MvxFragmentStatePagerAdapter
        : FragmentStatePagerAdapter
    {
        private readonly Context _context;
        public IEnumerable<MvxViewPagerFragmentInfo> Fragments { get; private set; }

        public override int Count => Fragments.Count();

        protected MvxFragmentStatePagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public MvxFragmentStatePagerAdapter(
            Context context, FragmentManager fragmentManager, IEnumerable<MvxViewPagerFragmentInfo> fragments)
            : base(fragmentManager)
        {
            _context = context;
            Fragments = fragments;
        }

        public override Fragment GetItem(int position)
        {
            var fragInfo = Fragments.ElementAt(position);

            if (fragInfo.CachedFragment == null)
            {
                fragInfo.CachedFragment = Fragment.Instantiate(_context, FragmentJavaName(fragInfo.FragmentType));

                var fragmentAsView = (IMvxView)fragInfo.CachedFragment;

                fragmentAsView.ViewModel = fragInfo.ViewModel ?? Mvx.IoCProvider.Resolve<IMvxViewModelLoader>()
                    .LoadViewModel(new MvxViewModelRequest(fragInfo.ViewModelType, null, null), null);
            }

            return fragInfo.CachedFragment;
        }

        protected static string FragmentJavaName(Type fragmentType)
        {
            return Class.FromType(fragmentType).Name;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new String(Fragments.ElementAt(position).Title);
        }

        public override void RestoreState (IParcelable state, ClassLoader loader)
        {
            //Don't call restore to prevent crash on rotation
            //base.RestoreState (state, loader);
        }
    }
}
