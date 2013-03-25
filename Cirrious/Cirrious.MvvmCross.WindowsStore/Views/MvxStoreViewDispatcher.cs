﻿// MvxStoreViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WindowsStore.Views
{
    public class MvxStoreViewDispatcher
        : MvxStoreMainThreadDispatcher
          , IMvxViewDispatcher
    {
        private readonly IMvxStorePresenter _presenter;
        private readonly Frame _rootFrame;

        public MvxStoreViewDispatcher(IMvxStorePresenter presenter, Frame rootFrame)
            : base(rootFrame.Dispatcher)
        {
            _presenter = presenter;
            _rootFrame = rootFrame;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            return RequestMainThreadAction(() => _presenter.Show(request));
        }
    }
}