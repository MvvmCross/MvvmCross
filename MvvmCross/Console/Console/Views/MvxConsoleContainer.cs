﻿// MvxConsoleContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Console.Views
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

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
                var view = (IMvxConsoleView)Activator.CreateInstance(viewType);
                var viewModelLoader = Mvx.Resolve<IMvxViewModelLoader>();
                IMvxBundle savedState = null;
                var viewModel = viewModelLoader.LoadViewModel(request, savedState);
                view.HackSetViewModel(viewModel);
                Mvx.Resolve<IMvxConsoleCurrentView>().CurrentView = view;
                this._navigationStack.Push(request);
            }
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (this.HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint)
            {
                this.Close((hint as MvxClosePresentationHint).ViewModelToClose);
                return;
            }

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }

        public void Close(IMvxViewModel viewModel)
        {
            var currentView = Mvx.Resolve<IMvxConsoleCurrentView>().CurrentView;

            if (currentView == null)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe has no current page");
                return;
            }

            if (currentView.ViewModel != viewModel)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return;
            }

            this.GoBack();
        }

        public override void GoBack()
        {
            lock (this)
            {
                if (!this.CanGoBack())
                {
                    System.Console.WriteLine("Back not possible");
                    return;
                }

                // pop off the current view
                this._navigationStack.Pop();

                // prepare to re-push the current view
                var backTo = this._navigationStack.Pop();

                // re-display the view
                this.Show(backTo);
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
                if (this._navigationStack.Count > 1)
                    return true;
                else
                    return false;
            }
        }
    }
}