#region Copyright
// <copyright file="IMvxPssView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Pss.Interfaces
{
    public interface IMvxPssView : IMvxView
    {
        void HackSetViewModel(object viewModel);
        bool HandleInput(string input);
    }
}