// MvxResourceProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Plugins.ResourceLoader
{
    public abstract class MvxResourceProvider
    {
        protected static string MakeLookupKey(string namespaceKey, string typeKey)
        {
            return string.Format("{0}|{1}", namespaceKey, typeKey);
        }

        protected static string MakeLookupKey(string namespaceKey, string typeKey, string name)
        {
            return string.Format("{0}|{1}|{2}", namespaceKey, typeKey, name);
        }
    }
}