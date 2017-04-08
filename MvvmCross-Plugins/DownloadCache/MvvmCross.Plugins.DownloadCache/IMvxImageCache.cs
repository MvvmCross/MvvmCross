// IMvxImageCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Threading.Tasks;

namespace MvvmCross.Plugins.DownloadCache
{
    public interface IMvxImageCache<T>
    {
        bool ContainsImage(string url);
        Task<T> RequestImage(string url);
    }
}