﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using AppKit;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Presenters;
using MvvmCross.Logging;
using MvvmCross.Platforms.Mac.Presenters;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Platforms.Mac.Presenters
{
    public class MvxFormsMacViewPresenter
        : MvxMacViewPresenter
        , IMvxFormsViewPresenter
    {
        public MvxFormsMacViewPresenter(INSApplicationDelegate applicationDelegate, Application formsApplication) : base(applicationDelegate)
        {
            FormsApplication = formsApplication ?? throw new ArgumentNullException(nameof(formsApplication), "MvxFormsApplication cannot be null");
        }

        private Application _formsApplication;
        public Application FormsApplication
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
                    throw new ArgumentNullException(nameof(FormsPagePresenter), "IMvxFormsPagePresenter cannot be null. Set the value in CreateViewPresenter in the setup.");
                return _formsPagePresenter;
            }
            set { _formsPagePresenter = value; }
        }

        public override Task<bool> Show(MvxViewModelRequest request)
        {
            return FormsPagePresenter.Show(request);
        }

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();

            FormsPagePresenter.RegisterAttributeTypes();
        }

        public override async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (!await FormsPagePresenter.ChangePresentation(hint)) return false;
            return await base.ChangePresentation(hint);
        }

        public override Task<bool> Close(IMvxViewModel viewModel)
        {
            return FormsPagePresenter.Close(viewModel);
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
