// IMvxResourceObjectLoaderConfiguration.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Plugins.ResourceLoader
{
    public interface IMvxResourceObjectLoaderConfiguration<T>
        where T : IMvxResourceObject
    {
        void SetRootLocation(string location);

        void SetRootLocation(string namespaceKey, string typeKey, string location);
    }
}