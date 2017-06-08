// IMvxSingletonCache.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core
{
    public interface IMvxSingletonCache
    {
        IMvxSettings Settings { get; }
        IMvxInpcInterceptor InpcInterceptor { get; }
        IMvxStringToTypeParser Parser { get; }
    }
}