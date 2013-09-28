// MvxConsoleContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Console.Views
{
    public class MvxConsoleContainer
        : MvxBaseConsoleContainer
    {
        private readonly Stack<MvxViewModelRequest> _navigationStack = new Stack<MvxViewModelRequest>();

        public override void Show(MvxViewModelRequest request)
        {
            lock (this)
            {
                var viewType = GetViewType(request.ViewModelType);
                if (viewType == null)
                {
                    throw new MvxException("View Type not found for " + request.ViewModelType);
                }
                var view = (IMvxConsoleView) Activator.CreateInstance(viewType);
                var viewModelLoader = Mvx.Resolve<IMvxViewModelLoader>();
                IMvxBundle savedState = null;
                var viewModel = viewModelLoader.LoadViewModel(request, savedState);
                view.HackSetViewModel(viewModel);
                Mvx.Resolve<IMvxConsoleCurrentView>().CurrentView = view;
                _navigationStack.Push(request);
            }
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
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
                Show(backTo);
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
    }
}