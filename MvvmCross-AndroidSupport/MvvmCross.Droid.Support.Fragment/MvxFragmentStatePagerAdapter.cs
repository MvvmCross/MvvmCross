// MvxFragmentStatePagerAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Java.Lang;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using String = Java.Lang.String;

namespace MvvmCross.Droid.Support.V4
{
    [Register("mvvmcross.droid.support.v4.MvxFragmentStatePagerAdapter")]
    public class MvxFragmentStatePagerAdapter
        : FragmentStatePagerAdapter
    {
        private readonly Context _context;
        public IEnumerable<MvxViewPagerFragment> Fragments { get; private set; }

        public override int Count => Fragments.Count();

        protected MvxFragmentStatePagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

		[Obsolete("MvxFragmentStatePagerAdapter is deprecated, please use MvxCachingFragmentStatePagerAdapter instead.")]
        public MvxFragmentStatePagerAdapter(
            Context context, FragmentManager fragmentManager, IEnumerable<MvxViewPagerFragment> fragments)
            : base(fragmentManager)
        {
            _context = context;
            Fragments = fragments;

            throw new InvalidOperationException("MvxFragmentStatePagerAdapter has broken cache implementation, use MvxFragmentPagerAdapter at this moment.");
        }

        public override Fragment GetItem(int position)
        {
            var fragInfo = Fragments.ElementAt(position);

            if (fragInfo.CachedFragment == null)
            {
                fragInfo.CachedFragment = Fragment.Instantiate(_context, FragmentJavaName(fragInfo.FragmentType));

                var request = new MvxViewModelRequest (fragInfo.ViewModelType, null, null);
                ((IMvxView)fragInfo.CachedFragment).ViewModel = Mvx.Resolve<IMvxViewModelLoader>().LoadViewModel(request, null);
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