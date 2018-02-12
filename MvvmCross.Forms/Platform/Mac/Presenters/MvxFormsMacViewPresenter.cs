﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Presenters;
using MvvmCross.Logging;
using MvvmCross.Platform.Mac.Presenters;
using MvvmCross.ViewModels;

namespace MvvmCross.Forms.Platform.Mac.Presenters
{
    public class MvxFormsMacViewPresenter
        : MvxMacViewPresenter
        , IMvxFormsViewPresenter
    {
        public MvxFormsMacViewPresenter(INSApplicationDelegate applicationDelegate, MvxFormsApplication formsApplication) : base(applicationDelegate)
        {
            FormsApplication = formsApplication ?? throw new ArgumentNullException(nameof(formsApplication), "MvxFormsApplication cannot be null");
        }

        private MvxFormsApplication _formsApplication;
        public MvxFormsApplication FormsApplication
        {
            get { return _formsApplication; }
            set { _formsApplication = value; }
        }

        private IMvxFormsPagePresenter _formsPagePresenter;
        public virtual IMvxFormsPagePresenter FormsPagePresenter
        {
            get
            {
                if (_formsPagePresenter == null)
                {
                    _formsPagePresenter = new MvxFormsPagePresenter(this);
                    Mvx.RegisterSingleton(_formsPagePresenter);
                }
                return _formsPagePresenter;
            }
            set
            {
                _formsPagePresenter = value;
            }
        }

        public override void Show(MvxViewModelRequest request)
        {
            FormsPagePresenter.Show(request);
        }

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();

            FormsPagePresenter.RegisterAttributeTypes();
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            FormsPagePresenter.ChangePresentation(hint);
        }

        public override void Close(IMvxViewModel viewModel)
        {
            FormsPagePresenter.Close(viewModel);
        }

        public virtual bool ShowPlatformHost(Type hostViewModel = null)
        {
            MvxFormsLog.Instance.Trace($"Showing of native host View in Forms is not supported.");
            return false;
        }

        public virtual bool ClosePlatformViews()
        {
            MvxFormsLog.Instance.Trace($"Closing of native Views in Forms is not supported.");
            return false;
        }
    }
}
