using System;
using Android.Support.V4.App;

namespace Cirrious.MvvmCross.Droid.Support.Fragging
{
    public interface IMvxCachedFragmentInfo
    {
        string Tag { get; }
        Type FragmentType { get; }
        Type ViewModelType { get; }
        Fragment CachedFragment { get; set; }
        int ContentId { get; set; }
    }
}