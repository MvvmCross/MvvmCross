#region Copyright

// <copyright file="MvxWpfView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Windows.Controls;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Wpf.Interfaces;

namespace Cirrious.MvvmCross.Wpf.Views
{
    public class MvxWpfView : UserControl, IMvxWpfView
    {
        // TODO - warning IMvxView.IsVisible is implemented here by UserControl! 

        private IMvxViewModel _viewModel;

        public IMvxViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                DataContext = value;
            }
        }
    }
}