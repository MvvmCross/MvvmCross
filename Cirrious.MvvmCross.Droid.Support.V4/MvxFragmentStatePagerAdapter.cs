using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using Cirrious.MvvmCross.ViewModels;
using Java.Lang;

namespace Cirrious.MvvmCross.Droid.Support.V4
{
    public class MvxFragmentStatePagerAdapter
        : FragmentStatePagerAdapter
    {
        private readonly Context _context;

        public IEnumerable<FragmentInfo> Fragments { get; private set; }

        public override int Count
        {
            get { return Fragments.Count(); }
        }

        protected MvxFragmentStatePagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public MvxFragmentStatePagerAdapter(
            Context context, FragmentManager fragmentManager, IEnumerable<FragmentInfo> fragments)
            : base(fragmentManager)
        {
            _context = context;
            Fragments = fragments;
        }

        public override Fragment GetItem(int position)
        {
            var fragmentInfo = Fragments.ElementAt(position);
            var fragment = Fragment.Instantiate(_context,
                FragmentJavaName(fragmentInfo.FragmentType));
            ((MvxFragment)fragment).ViewModel = fragmentInfo.ViewModel;
            return fragment;
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

        public override void RestoreState (IParcelable state, ClassLoader loader)
        {
            //Don't call restore to prevent crash on rotation
            //base.RestoreState (state, loader);
        }

        public class FragmentInfo
        {
            public string Title { get; set; }

            public Type FragmentType { get; set; }

            public IMvxViewModel ViewModel { get; set; }
        }
    }
}

