#region Copyright
// <copyright file="MvxPssView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Pss.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Pss.Views
{
    public class MvxPssView<T> 
        : IMvxPssView
        , IMvxServiceConsumer 
        where T : IMvxViewModel
    {
        public T ViewModel { get; set; }

        public Type ViewModelType
        {
            get { return typeof(T); }
        }

#warning Need to get the Pss implementation level with the mobile implementations again - HackSetViewModel is a Hack
        public void HackSetViewModel(object viewModel)
        {
            ViewModel = (T)viewModel;
            OnViewModelChanged();
        }

        public virtual bool HandleInput(string input)
        {
            return false;
        }

        protected virtual void OnViewModelChanged() { }

        public bool IsVisible { get { return this.GetService<IMvxPssCurrentView>().CurrentView == this; }}
    }
}
