#region Copyright
// <copyright file="MvxConsoleContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Console.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Console.Views
{
    public class MvxConsoleContainer
        : MvxBaseConsoleContainer 
        , IMvxConsoleNavigation
        , IMvxServiceConsumer<IMvxViewModelLoader>
        , IMvxServiceConsumer<IMvxConsoleCurrentView>        
    {
        private readonly Stack<MvxShowViewModelRequest> _navigationStack = new Stack<MvxShowViewModelRequest>();

        #region IMvxConsoleNavigation Members

        public override void Navigate(MvxShowViewModelRequest request)
        {
            lock (this)
            {
                var viewType = GetViewType(request.ViewModelType);
                if (viewType == null)
                {
                    throw new MvxException("View Type not found for " + request.ViewModelType);
                }
                var view = (IMvxConsoleView) Activator.CreateInstance(viewType);
                var viewModelLoader = this.GetService<IMvxViewModelLoader>();
                var viewModel = viewModelLoader.LoadViewModel(request);
                view.HackSetViewModel(viewModel);
                this.GetService<IMvxConsoleCurrentView>().CurrentView = view;
                _navigationStack.Push(request);
            }
        }

        public override void GoBack()
        {
            lock (this)
            {
                if (!CanGoBack())
                {
                    System.Console.WriteLine("Back not possible");
                    return;
                }

                // pop off the current view
                _navigationStack.Pop();

                // prepare to re-push the current view
                var backTo = _navigationStack.Pop();

                // re-display the view
                Navigate(backTo);
            }
        }

        public override void RemoveBackEntry()
        {
            throw new NotImplementedException("RemoveBackEntry not supported on console currently");
        }

        public override bool CanGoBack()
        {
            lock (this)
            {
                if (_navigationStack.Count > 1)
                    return true;
                else
                    return false;
            }
        }

        #endregion
    }
}