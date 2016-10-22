using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using Java.Lang;

namespace MvvmCross.Droid.Support.V4
{
    [Register("mvvmcross.droid.support.v4.MvxFragmentPagerAdapter")]
    public class MvxFragmentPagerAdapter : FragmentPagerAdapter
    {
        private readonly Context _context;
        public IEnumerable<FragmentInfo> Fragments { get; private set; }

        public override int Count => Fragments.Count();

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

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            var fragInfo = Fragments.ElementAt(position);

            if (fragInfo.CachedFragment == null)
            {
                fragInfo.CachedFragment = Android.Support.V4.App.Fragment.Instantiate(_context, FragmentJavaName(fragInfo.FragmentType));

                var request = new MvxViewModelRequest(fragInfo.ViewModelType, null, null, null);
                ((IMvxView)fragInfo.CachedFragment).ViewModel = Mvx.Resolve<IMvxViewModelLoader>().LoadViewModel(request, null);
            }

            return fragInfo.CachedFragment;
        }

        protected static string FragmentJavaName(Type fragmentType)
        {
            var namespaceText = fragmentType.Namespace ?? "";
            if (namespaceText.Length > 0)
                namespaceText = namespaceText.ToLowerInvariant() + ".";
            return namespaceText + fragmentType.Name;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(Fragments.ElementAt(position).Title);
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
            public Type FragmentType { get; private set; }
            public Type ViewModelType { get; private set; }
            public Android.Support.V4.App.Fragment CachedFragment { get; set; }
        }
    }
}