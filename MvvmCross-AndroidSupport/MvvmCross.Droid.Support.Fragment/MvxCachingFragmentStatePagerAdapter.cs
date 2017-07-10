using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.App;
using Java.Lang;
using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using String = Java.Lang.String;

namespace MvvmCross.Droid.Support.V4
{
    [Register("mvvmcross.droid.support.v4.MvxCachingFragmentStatePagerAdapter")]
    public class MvxCachingFragmentStatePagerAdapter
		: MvxCachingFragmentPagerAdapter
    {
        private readonly Context _context;

		protected MvxCachingFragmentStatePagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

		public MvxCachingFragmentStatePagerAdapter(Context context, FragmentManager fragmentManager,
            List<MvxViewPagerFragment> fragments) : base(fragmentManager)
        {
            _context = context;
            Fragments = fragments;
        }

        public override int Count => Fragments.Count();

        public List<MvxViewPagerFragment> Fragments { get; }

        protected static string FragmentJavaName(Type fragmentType)
        {
            return Class.FromType(fragmentType).Name;
        }

        public override Fragment GetItem(int position, Fragment.SavedState fragmentSavedState = null)
        {
            var fragInfo = Fragments.ElementAt(position);
            var fragment = Fragment.Instantiate(_context, FragmentJavaName(fragInfo.FragmentType));

            var mvxFragment = fragment as MvxFragment;
            if (mvxFragment == null)
                return fragment;

			if (mvxFragment.GetType().IsFragmentCacheable(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.GetType()) && fragmentSavedState != null)
                return fragment;

            var viewModel = fragInfo.ViewModel ?? CreateViewModel(position);
            mvxFragment.ViewModel = viewModel;

            return fragment;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new String(Fragments.ElementAt(position).Title);
        }

        protected override string GetTag(int position)
        {
            return Fragments.ElementAt(position).Tag;
        }

        private IMvxViewModel CreateViewModel(int position)
        {
            var fragInfo = Fragments.ElementAt(position);

            MvxBundle mvxBundle = null;
            if (fragInfo.ParameterValuesObject != null)
                mvxBundle = new MvxBundle(fragInfo.ParameterValuesObject.ToSimplePropertyDictionary());

            var request = new MvxViewModelRequest(fragInfo.ViewModelType, mvxBundle, null);

            return Mvx.Resolve<IMvxViewModelLoader>().LoadViewModel(request, null);
        }
    }
}
