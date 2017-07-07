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
            List<FragmentInfo> fragments) : base(fragmentManager)
        {
            _context = context;
            Fragments = fragments;
        }

        public override int Count => Fragments.Count();

        public List<FragmentInfo> Fragments { get; }

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

        public class FragmentInfo
        {
            public FragmentInfo(string title, Type fragmentType, Type viewModelType, object parameterValuesObject = null)
                : this(title, null, fragmentType, viewModelType, parameterValuesObject)
            {
            }

            public FragmentInfo(string title, string tag, Type fragmentType, Type viewModelType,
                                object parameterValuesObject = null)
            {
                Title = title;
                Tag = tag ?? title;
                FragmentType = fragmentType;
                ViewModelType = viewModelType;
                ParameterValuesObject = parameterValuesObject;
            }
            
            public FragmentInfo(string title, Type fragmentType, IMvxViewModel viewModel, object parameterValuesObject = null)
		        : this(title, null, fragmentType, viewModel.GetType(), parameterValuesObject)
	        {
		        ViewModel = viewModel;
	        }

	        public FragmentInfo(string title, string tag, Type fragmentType, IMvxViewModel viewModel, object parameterValuesObject = null)
		        : this(title, tag, fragmentType, viewModel.GetType(), parameterValuesObject)
	        {  
		        ViewModel = viewModel;
	        }

            public Type FragmentType { get; }

            public object ParameterValuesObject { get; }

            public string Tag { get; }

            public string Title { get; }

            public Type ViewModelType { get; }
            
            public IMvxViewModel ViewModel { get; }
        }
    }
}
