// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Console.Views
{
    public class MvxConsoleContainer
        : MvxBaseConsoleContainer
    {
        private readonly Stack<MvxViewModelRequest> _navigationStack = new Stack<MvxViewModelRequest>();
        private readonly object _targetLocker = new object();

        public override ValueTask<bool> Show(MvxViewModelRequest request)
        {
            lock (_targetLocker)
            {
                var viewType = GetViewType(request.ViewModelType);
                if (viewType == null)
                {
                    throw new MvxException("View Type not found for " + request.ViewModelType);
                }
                var view = (IMvxConsoleView)Activator.CreateInstance(viewType);
                var viewModelLoader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
                IMvxBundle? savedState = null;
                var viewModel = viewModelLoader.LoadViewModel(request, savedState);
                view.HackSetViewModel(viewModel);
                Mvx.IoCProvider.Resolve<IMvxConsoleCurrentView>().CurrentView = view;
                _navigationStack.Push(request);
            }
            return new ValueTask<bool>(true);
        }

        public override async ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (await HandlePresentationChange(hint).ConfigureAwait(false)) return true;

            if (hint is MvxClosePresentationHint closePresentationhint)
            {
                return await Close(closePresentationhint.ViewModelToClose).ConfigureAwait(false);
            }

            MvxLog.Instance.Warn("Hint ignored {0}", hint.GetType().Name);
            return false;
        }

        public override ValueTask<bool> Close(IMvxViewModel viewModel)
        {
            var currentView = Mvx.IoCProvider.Resolve<IMvxConsoleCurrentView>().CurrentView;

            if (currentView == null)
            {
                MvxLog.Instance.Warn("Ignoring close for viewmodel - rootframe has no current page");
                return new ValueTask<bool>(true);
            }

            if (currentView.ViewModel != viewModel)
            {
                MvxLog.Instance.Warn("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return new ValueTask<bool>(true);
            }

            return GoBack();
        }

        public override ValueTask<bool> GoBack()
        {
            lock (_targetLocker)
            {
                if (!CanGoBack())
                {
                    System.Console.WriteLine("Back not possible");
                    return new ValueTask<bool>(true);
                }

                // pop off the current view
                _navigationStack.Pop();

                // prepare to re-push the current view
                var backTo = _navigationStack.Pop();

                // re-display the view
                return Show(backTo);
            }
        }

        public override void RemoveBackEntry()
        {
            throw new NotImplementedException("RemoveBackEntry not supported on console currently");
        }

        public override bool CanGoBack()
        {
            lock (_targetLocker)
            {
                if (_navigationStack.Count > 1)
                    return true;
                else
                    return false;
            }
        }
    }
}
