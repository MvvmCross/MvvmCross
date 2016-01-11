using System;

namespace MvvmCross.Droid.Support.V7.Fragging.Caching
{
    // ok so now first quick note from @thefex
    // Why does this interface exists and what it is for?
    // It turns out that IMvxCachedFragmentInfo is not a serializable object - because it contains Fragment property.
    // We does not to serialize whole Fragment anyway - we just need information about registered fragments so we can restore that on recreation.
    // That's a interface instead of abstract class to force client to reimplement all the methods.

    /// <summary>
    /// You should implement this interface if you are using custom IMvxCachedFragmentInfo.
    /// </summary>
    public interface IMvxCachedFragmentInfoFactory
    {
        IMvxCachedFragmentInfo CreateFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool cacheFragment = true,
            bool addToBackstack = false);

        SerializableMvxCachedFragmentInfo GetSerializableFragmentInfo(IMvxCachedFragmentInfo objectToSerialize);

        IMvxCachedFragmentInfo ConvertSerializableFragmentInfo(SerializableMvxCachedFragmentInfo fromSerializableMvxCachedFragmentInfo);
    }
}