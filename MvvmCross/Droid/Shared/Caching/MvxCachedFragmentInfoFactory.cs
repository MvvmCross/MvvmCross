using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;
using System;
using MvvmCross.Droid.Shared.Fragments;

namespace MvvmCross.Droid.Support.V7.Fragging.Caching
{
    public class MvxCachedFragmentInfoFactory : IMvxCachedFragmentInfoFactory
    {
        /// <summary>
        /// If you override this method make sure you override ConvertSerializableCachedFragmentInfo and GetObjectToSerializeFromCachedFragmentInfo
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fragmentType"></param>
        /// <param name="viewModelType"></param>
        /// <param name="cacheFragment"></param>
        /// <param name="addToBackstack"></param>
        /// <returns></returns>
        public virtual IMvxCachedFragmentInfo CreateFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool cacheFragment = true, bool addToBackstack = false)
        {
            if (!typeof(IMvxFragmentView).IsAssignableFrom(fragmentType))
                throw new InvalidOperationException($"Registered fragment isn't an IMvxFragmentView. Received: {fragmentType}");

            if (!typeof(IMvxViewModel).IsAssignableFrom(viewModelType))
                throw new InvalidOperationException($"Registered view model isn't an IMvxViewModel. Received: {viewModelType}");

            return new MvxCachedFragmentInfo(tag, fragmentType, viewModelType, cacheFragment, addToBackstack);
        }

        public virtual SerializableMvxCachedFragmentInfo GetSerializableFragmentInfo(IMvxCachedFragmentInfo objectToSerialize)
        {
            return new SerializableMvxCachedFragmentInfo()
            {
                Tag = objectToSerialize.Tag,
                FragmentType = objectToSerialize.FragmentType,
                ViewModelType = objectToSerialize.ViewModelType,
                CacheFragment = objectToSerialize.CacheFragment,
                ContentId = objectToSerialize.ContentId,
                AddToBackStack = objectToSerialize.AddToBackStack
            };
        }

        public virtual IMvxCachedFragmentInfo ConvertSerializableFragmentInfo(
            SerializableMvxCachedFragmentInfo fromSerializableMvxCachedFragmentInfo)
        {
            return new MvxCachedFragmentInfo(fromSerializableMvxCachedFragmentInfo.Tag,
                           fromSerializableMvxCachedFragmentInfo.FragmentType,
                           fromSerializableMvxCachedFragmentInfo.ViewModelType,
                           fromSerializableMvxCachedFragmentInfo.CacheFragment,
                           fromSerializableMvxCachedFragmentInfo.AddToBackStack)
            {
                ContentId = fromSerializableMvxCachedFragmentInfo.ContentId
            };
        }
    }
}