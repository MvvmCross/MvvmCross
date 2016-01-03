using System;
using Android.App;

namespace MvvmCross.Droid.FullFragging.Caching
{
    public class MvxCachedFragmentInfo : IMvxCachedFragmentInfo
    {
        public MvxCachedFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool addToBackstack)
        {
            Tag = tag;
            FragmentType = fragmentType;
            ViewModelType = viewModelType;
            AddToBackStack = addToBackstack;
        }

        public string Tag { get; set; }
        public Type FragmentType { get; set; }
        public Type ViewModelType { get; set; }
        public Fragment CachedFragment { get; set; }
        public int ContentId { get; set; }
        public bool AddToBackStack { get; set; }

    }
}