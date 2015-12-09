// MvxViewModel.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    public abstract class MvxViewModel
        : MvxNavigatingObject
          , IMvxViewModel
    {
        protected MvxViewModel()
        {
            this.RequestedBy = MvxRequestedBy.Unknown;
        }

        public MvxRequestedBy RequestedBy { get; set; }

        public void Init(IMvxBundle parameters)
        {
            this.InitFromBundle(parameters);
        }

        public void ReloadState(IMvxBundle state)
        {
            this.ReloadFromBundle(state);
        }

        public virtual void Start()
        {
        }

        public void SaveState(IMvxBundle state)
        {
            this.SaveStateToBundle(state);
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