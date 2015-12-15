using System;

namespace MvvmCross.Droid.Support.V7.Fragging.Caching
{
    public class SerializableMvxCachedFragmentInfo
    {
        public SerializableMvxCachedFragmentInfo()
        {
        }

        public SerializableMvxCachedFragmentInfo(
            SerializableMvxCachedFragmentInfo serializableMvxCachedFragmentInfoToClone)
        {
            Tag = serializableMvxCachedFragmentInfoToClone.Tag;
            FragmentType = serializableMvxCachedFragmentInfoToClone.FragmentType;
            ViewModelType = serializableMvxCachedFragmentInfoToClone.ViewModelType;
            ContentId = serializableMvxCachedFragmentInfoToClone.ContentId;
            AddToBackStack = serializableMvxCachedFragmentInfoToClone.AddToBackStack;
        }

        public string Tag { get; set; }
        public Type FragmentType { get; set; }
        public Type ViewModelType { get; set; }
        public int ContentId { get; set; }
        public bool AddToBackStack { get; set; }
    }
}