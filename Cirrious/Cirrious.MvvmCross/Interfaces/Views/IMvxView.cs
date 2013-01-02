// IMvxView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxView
    {
        bool IsVisible { get; }
    }

    public interface IMvxView<TViewModel>
        : IMvxView
        where TViewModel : class, IMvxViewModel
    {
        TViewModel ViewModel { get; set; }
    }
}