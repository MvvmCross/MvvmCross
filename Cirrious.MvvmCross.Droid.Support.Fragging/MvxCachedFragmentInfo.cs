using System;
using Android.Support.V4.App;

namespace Cirrious.MvvmCross.Droid.Support.Fragging
{
    public class MvxCachedFragmentInfo : IMvxCachedFragmentInfo
    {
        public MvxCachedFragmentInfo(string tag, Type fragmentType, Type viewModelType)
        {
            Tag = tag;
            FragmentType = fragmentType;
            ViewModelType = viewModelType;
        }

        public string Tag { get; set; }
        public Type FragmentType { get; set; }
        public Type ViewModelType { get; set; }
        public Fragment CachedFragment { get; set; }
        public int ContentId { get; set; }
        public bool AddToBackStack { get; set; }

    }
}