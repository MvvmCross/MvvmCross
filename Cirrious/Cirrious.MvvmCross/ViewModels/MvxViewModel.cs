// MvxViewModel.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxViewModel
        : MvxNavigatingObject
        , IMvxViewModel
    {
        private readonly Dictionary<IMvxView, bool> _views = new Dictionary<IMvxView, bool>();

        protected MvxViewModel()
        {
            RequestedBy = MvxRequestedBy.Unknown;
        }

        protected bool HasViews
        {
            get
            {
                lock (this)
                {
                    return _views.Any();
                }
            }
        }

        public MvxRequestedBy RequestedBy { get; set; }

        public virtual void Init(IMvxBundle parameters)
        {
        }

        public virtual void ReloadState(IMvxBundle state)
        {
        }

        public virtual void Start()
        {
        }

        public virtual void SaveState(IMvxBundle state)
        {
        }
    }
}