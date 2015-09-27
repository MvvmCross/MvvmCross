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

        public string Tag { get; private set; }
        public Type FragmentType { get; private set; }
        public Type ViewModelType { get; private set; }
        public Fragment CachedFragment { get; set; }
        public int ContentId { get; set; }
    }
}