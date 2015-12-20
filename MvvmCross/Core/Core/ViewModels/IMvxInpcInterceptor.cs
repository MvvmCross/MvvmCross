// IMvxInpcInterceptor.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System.ComponentModel;

    public interface IMvxInpcInterceptor
    {
        MvxInpcInterceptionResult Intercept(IMvxNotifyPropertyChanged sender, PropertyChangedEventArgs args);
    }
}