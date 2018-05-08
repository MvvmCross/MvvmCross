// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Forms.Presenters;
using System;
using MvvmCross.Forms.Core;
using MvvmCross.Logging;
using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Xamarin.Forms;
using System.Windows.Controls;

namespace MvvmCross.Forms.Platforms.Wpf.Presenters
{
    public class MvxFormsWpfViewPresenter
        : MvxWpfViewPresenter
        , IMvxFormsViewPresenter
    {
        public MvxFormsWpfViewPresenter(Application formsApplication) : base()
        {
            FormsApplication = formsApplication ?? throw new ArgumentNullException(nameof(formsApplication), "MvxFormsApplication cannot be null");
        }

        public MvxFormsWpfViewPresenter(ContentControl contentControl, Application formsApplication) : base(contentControl)
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

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();
            FormsPagePresenter.RegisterAttributeTypes();
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            FormsPagePresenter.ChangePresentation(hint);
            base.ChangePresentation(hint);
        }

        public override void Show(MvxViewModelRequest request)
        {
            FormsPagePresenter.Show(request);
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
