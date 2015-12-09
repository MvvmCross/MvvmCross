using System;
using Android.Support.V4.App;

namespace MvvmCross.Droid.Support.V7.Fragging
{
    public interface IMvxCachedFragmentInfo
    {
        string Tag { get; set;  }
        Type FragmentType { get; set; }
        Type ViewModelType { get; set; }
        Fragment CachedFragment { get; set; }
        int ContentId { get; set; }
        bool AddToBackStack { get; set; }

    }
}