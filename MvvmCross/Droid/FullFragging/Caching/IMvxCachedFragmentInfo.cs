using System;
using Android.App;

namespace MvvmCross.Droid.FullFragging.Caching
{
    public interface IMvxCachedFragmentInfo
    {
        string Tag { get; set; }
        Type FragmentType { get; set; }
        Type ViewModelType { get; set; }
        Fragment CachedFragment { get; set; }
        bool CacheFragment { get; set; }
        int ContentId { get; set; }
        bool AddToBackStack { get; set; }
    }
}