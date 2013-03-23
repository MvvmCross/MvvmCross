// MvxViewModel.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.ViewModels
{
    public abstract class MvxViewModel
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

        public void Init(IMvxBundle parameters)
        {
            InitFromBundle(parameters);
        }

        public void ReloadState(IMvxBundle state)
        {
            ReloadFromBundle(state);
        }

        public virtual void Start()
        {
        }

        public void SaveState(IMvxBundle state)
        {
            SaveStateToBundle(state);
        }

        protected virtual void InitFromBundle(IMvxBundle parameters)
        {
        }

        protected virtual void ReloadFromBundle(IMvxBundle state)
        {
        }

        protected virtual void SaveStateToBundle(IMvxBundle bundle)
        {
        }
    }
}