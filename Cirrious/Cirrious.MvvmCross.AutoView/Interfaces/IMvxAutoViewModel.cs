#region Copyright

// <copyright file="IMvxAutoViewModel.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using CrossUI.Core.Descriptions;

namespace Cirrious.MvvmCross.AutoView.Interfaces
{
    public interface IMvxAutoViewModel
    {
        bool SupportsAutoView(string type);
        KeyedDescription GetAutoView(string type);
    }

    public interface IMvxAutoDialogViewModel : IMvxAutoViewModel
    {
    }

    public interface IMvxAutoListViewModel : IMvxAutoViewModel
    {
    }
}