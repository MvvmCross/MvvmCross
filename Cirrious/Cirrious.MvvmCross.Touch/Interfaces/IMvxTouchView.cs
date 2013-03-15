// IMvxTouchView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Touch.Interfaces
{
    public interface IMvxTouchView
        : IMvxView
    {
        MvxShowViewModelRequest ShowRequest { get; set; }
    }
}