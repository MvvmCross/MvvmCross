using System;

namespace Cirrious.MvvmCross.Droid.FullFragging.Caching
{
    public class SerializableMvxCachedFragmentInfo
    {
        public string Tag { get; set; }
        public Type FragmentType { get; set; }
        public Type ViewModelType { get; set; }
        public int ContentId { get; set; }
        public bool AddToBackStack { get; set; }
    }
}