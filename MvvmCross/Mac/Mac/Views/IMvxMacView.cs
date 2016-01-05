#region Copyright

// <copyright file="IMvxMacView.cs" company="">
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//

#endregion Copyright


namespace MvvmCross.Mac.Views
{
    using global::MvvmCross.Binding.BindingContext;
    using global::MvvmCross.Core.ViewModels;
    using global::MvvmCross.Core.Views;

    public interface IMvxMacView
        : IMvxView
            , IMvxCanCreateMacView
            , IMvxBindingContextOwner
    {
        MvxViewModelRequest Request { get; set; }
    }
}