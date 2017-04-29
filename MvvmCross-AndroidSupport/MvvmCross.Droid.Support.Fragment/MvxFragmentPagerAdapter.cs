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
    [Register("mvvmcross.droid.support.v4.MvxFragmentPagerAdapter")]
    public class MvxFragmentPagerAdapter : FragmentPagerAdapter
    {
        private readonly Context _context;

        protected MvxFragmentPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        [Obsolete("MvxFragmentPagerAdapter is deprecated, please use MvxCachingFragmentPagerAdapter instead.")]
        public MvxFragmentPagerAdapter(
            Context context, FragmentManager fragmentManager, IEnumerable<FragmentInfo> fragments)
            : base(fragmentManager)
        {
            _context = context;
            Fragments = fragments;
        }

        public IEnumerable<FragmentInfo> Fragments { get; }

        public override int Count => Fragments.Count();

        public override Fragment GetItem(int position)
        {
            var fragInfo = Fragments.ElementAt(position);

            if (fragInfo.CachedFragment == null)
            {
                fragInfo.CachedFragment = Fragment.Instantiate(_context, FragmentJavaName(fragInfo.FragmentType));

                var request = new MvxViewModelRequest(fragInfo.ViewModelType, null, null);
                ((IMvxView) fragInfo.CachedFragment).ViewModel =
                    Mvx.Resolve<IMvxViewModelLoader>().LoadViewModel(request, null);
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

        public override void RestoreState(IParcelable state, ClassLoader loader)
        {
            //Don't call restore to prevent crash on rotation
            //base.RestoreState (state, loader);
        }

        public class FragmentInfo
        {
            public FragmentInfo(string title, Type fragmentType, Type viewModelType)
            {
                Title = title;
                FragmentType = fragmentType;
                ViewModelType = viewModelType;
            }

            public string Title { get; set; }
            public Type FragmentType { get; }
            public Type ViewModelType { get; }
            public Fragment CachedFragment { get; set; }
        }
    }
}