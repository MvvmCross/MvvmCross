// IMvxHttpFileDownloader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Threading.Tasks;

namespace Cirrious.MvvmCross.Plugins.DownloadCache
{
    public interface IMvxHttpFileDownloader
    {
        Task RequestDownload(string url, string downloadPath);
    }
}