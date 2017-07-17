using System;
using MvvmCross.Droid.Shared.Fragments;

namespace MvvmCross.Droid.Shared.Caching
{
    public class MvxCachedFragmentInfo : IMvxCachedFragmentInfo
    {
        public MvxCachedFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool cacheFragment, bool addToBackstack)
        {
            Tag = tag;
            FragmentType = fragmentType;
            ViewModelType = viewModelType;
            CacheFragment = cacheFragment;
            AddToBackStack = addToBackstack;
        }

        public string Tag { get; set; }
        public Type FragmentType { get; set; }
        public Type ViewModelType { get; set; }
        public IMvxFragmentView CachedFragment { get; set; }
        public bool CacheFragment { get; set; }
        public int ContentId { get; set; }
        public bool AddToBackStack { get; set; }
    }
}