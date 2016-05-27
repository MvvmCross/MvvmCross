using MvvmCross.Droid.Shared.Fragments;
using System;

namespace MvvmCross.Droid.Shared.Caching
{
    public interface IMvxCachedFragmentInfo
    {
        string Tag { get; set; }
        Type FragmentType { get; set; }
        Type ViewModelType { get; set; }
        IMvxFragmentView CachedFragment { get; set; }
        bool CacheFragment { get; set; }
        int ContentId { get; set; }
        bool AddToBackStack { get; set; }
    }
}