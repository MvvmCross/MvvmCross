using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.FullFragging.Fragments;

namespace MvvmCross.Droid.FullFragging.Caching
{
    public class MvxCachedFragmentInfoFactory : IMvxCachedFragmentInfoFactory
    {
        /// <summary>
        /// If you override this method make sure you override ConvertSerializableCachedFragmentInfo and GetObjectToSerializeFromCachedFragmentInfo
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fragmentType"></param>
        /// <param name="viewModelType"></param>
        /// <param name="addToBackstack"></param>
        /// <returns></returns>
        public IMvxCachedFragmentInfo CreateFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool addToBackstack = false)
        {
            if (!typeof(IMvxFragmentView).IsAssignableFrom(fragmentType))
                throw new InvalidOperationException($"Registered fragment isn't an IMvxFragmentView. Received: {fragmentType}");

            if (!typeof(IMvxViewModel).IsAssignableFrom(viewModelType))
                throw new InvalidOperationException($"Registered view model isn't an IMvxViewModel. Received: {viewModelType}");

            return new MvxCachedFragmentInfo(tag, fragmentType, viewModelType, addToBackstack);
        }

        public SerializableMvxCachedFragmentInfo GetSerializableFragmentInfo(IMvxCachedFragmentInfo objectToSerialize)
        {
            return new SerializableMvxCachedFragmentInfo()
            {
                Tag = objectToSerialize.Tag,
                FragmentType = objectToSerialize.FragmentType,
                ViewModelType = objectToSerialize.ViewModelType,
                ContentId = objectToSerialize.ContentId,
                AddToBackStack = objectToSerialize.AddToBackStack
            };
        }

        public IMvxCachedFragmentInfo ConvertSerializableFragmentInfo(
            SerializableMvxCachedFragmentInfo fromSerializableMvxCachedFragmentInfo)
        {
            return new MvxCachedFragmentInfo(fromSerializableMvxCachedFragmentInfo.Tag,
                           fromSerializableMvxCachedFragmentInfo.FragmentType,
                           fromSerializableMvxCachedFragmentInfo.ViewModelType,
                           fromSerializableMvxCachedFragmentInfo.AddToBackStack)
            {
                ContentId = fromSerializableMvxCachedFragmentInfo.ContentId
            };
        }
    }
}