using Android.Support.V4.App;
using System;

namespace MvvmCross.Droid.Support.V7.Fragging.Caching
{
    public interface IMvxCachedFragmentInfo
    {
        string Tag { get; set; }
        Type FragmentType { get; set; }
        Type ViewModelType { get; set; }
        Fragment CachedFragment { get; set; }
        int ContentId { get; set; }
        bool AddToBackStack { get; set; }
    }
}